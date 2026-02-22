using System;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Repositories;
using Repository.Pattern.Infrastructure;
using Service.Pattern;
using WebApp.Models;
using WebApp.Repositories;
using AutoMapper;
using WebApp.Models.ViewModel;

namespace WebApp.Services
{
                          public class LegalCaseService : Service<LegalCase>, ILegalCaseService
  {
    private readonly IRepositoryAsync<LegalCase> repository;
    private readonly IDataTableImportMappingService mappingservice;
    private readonly NLog.ILogger logger;
    private readonly ICompanyService companyService;
    private readonly INodeConfigService nodeConfigService;
    private readonly ITrackHistoryService trackHistoryService;
    private readonly IMapper mapper;
    private readonly IAttachmentService attachmentService;
    private readonly INodeTimeService nodeTimeService;
    private readonly ITempCaseIdService tempCaseIdService;
    private readonly SqlHelper2.IDatabaseAsync db;
    public LegalCaseService(
      ITempCaseIdService tempCaseIdService,
      INodeConfigService nodeConfigService,
      ICompanyService companyService,
      IRepositoryAsync<LegalCase> repository,
      IDataTableImportMappingService mappingservice,
      ITrackHistoryService trackHistoryService,
      NLog.ILogger logger,
      IMapper mapper,
      INodeTimeService nodeTimeService,
      IAttachmentService attachmentService,
      SqlHelper2.IDatabaseAsync db
      )
        : base(repository)
    {
      this.db = db;
      this.repository = repository;
      this.mappingservice = mappingservice;
      this.logger = logger;
      this.nodeConfigService = nodeConfigService;
      this.companyService = companyService;
      this.trackHistoryService = trackHistoryService;
      this.mapper = mapper;
      this.attachmentService = attachmentService;
      this.nodeTimeService = nodeTimeService;
      this.tempCaseIdService = tempCaseIdService;
    }



    public async Task ImportDataTableAsync(DataTable datatable, string username)
    {
      var mapping = await this.mappingservice.Queryable()
                        .Where(x => x.EntitySetName == "LegalCase" &&
                           ( x.IsEnabled == true || ( x.IsEnabled == false && x.DefaultValue != null ) )
                           ).ToListAsync();
      if (mapping.Count == 0)
      {
        throw new KeyNotFoundException("没有找到LegalCase对象的Excel导入配置信息，请执行[系统管理/Excel导入配置]");
      }
      foreach (DataRow row in datatable.Rows)
      {

        var requiredfield = mapping.Where(x => x.IsRequired == true && x.IsEnabled == true && x.DefaultValue == null).FirstOrDefault()?.SourceFieldName;
        if (requiredfield != null || !row.IsNull(requiredfield))
        {
          var item = new LegalCase();
          foreach (var field in mapping)
          {
            var defval = field.DefaultValue;
            var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
            if (contain && !row.IsNull(field.SourceFieldName))
            {
              var legalcasetype = item.GetType();
              var propertyInfo = legalcasetype.GetProperty(field.FieldName);
              var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
              var safeValue = ( row[field.SourceFieldName] == null ) ? null : Convert.ChangeType(row[field.SourceFieldName], safetype);
              propertyInfo.SetValue(item, safeValue, null);
            }
            else if (!string.IsNullOrEmpty(defval))
            {
              var legalcasetype = item.GetType();
              var propertyInfo = legalcasetype.GetProperty(field.FieldName);
              if (string.Equals(defval, "now", StringComparison.OrdinalIgnoreCase) && ( propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(Nullable<DateTime>) ))
              {
                var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                var safeValue = Convert.ChangeType(DateTime.Now, safetype);
                propertyInfo.SetValue(item, safeValue, null);
              }
              else if (string.Equals(defval, "guid", StringComparison.OrdinalIgnoreCase))
              {
                propertyInfo.SetValue(item, Guid.NewGuid().ToString(), null);
              }
              else if (string.Equals(defval, "user", StringComparison.OrdinalIgnoreCase))
              {
                propertyInfo.SetValue(item, username, null);
              }
              else
              {
                var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                var safeValue = Convert.ChangeType(defval, safetype);
                propertyInfo.SetValue(item, safeValue, null);
              }
            }
          }

          if (string.IsNullOrEmpty(item.CaseId))
          {
            var result = await this.GetCaseId($"（{DateTime.Now.Year}）", "粤0605", item.Category);
            item.CaseId = result.Item1;
            item.Expires = result.Item3;
            item.PreCloseDate = item.RegisterDate.AddDays(result.Item3);
          }
          else
          {
            var expires = await this.nodeTimeService.Queryable()
          .Where(x => x.Category == item.Category).SumAsync(x => x.Days);
            item.Expires = expires;
            item.PreCloseDate = item.RegisterDate.AddDays(expires);
          }
          
          item.SerialNumber = "粤0605";
          item.Court = item.FromDepartment;
          this.Insert(item);
          var nodeconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);
          var track = this.mapper.Map<TrackHistory>(item);
          track.Owner = item.Proposer;
          track.BeginDate = item.FilingDate;
          track.Expires = nodeconfig.Expires;
          this.trackHistoryService.Insert(track);
          await this.DeleteTempCaseId(item.CaseId);
        }
      }
    }
        private async Task DeleteTempCaseId(string caseid) {
      var temp =await this.tempCaseIdService.Queryable().Where(x => x.CaseId == caseid).FirstOrDefaultAsync();
      if (temp != null)
      {
        this.tempCaseIdService.Delete(temp);
      }
    }

    public async Task<Stream> ExportExcelAsync(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var expcolopts = await this.mappingservice.Queryable()
             .Where(x => x.EntitySetName == "LegalCase")
             .Select(x => new ExpColumnOpts()
             {
               EntitySetName = x.EntitySetName,
               FieldName = x.FieldName,
               IgnoredColumn = x.IgnoredColumn,
               SourceFieldName = x.SourceFieldName
             }).ToArrayAsync();

      var legalcases = this.Query(new LegalCaseQuery().Withfilter(filters)).OrderBy(n => n.OrderBy(sort, order)).Select().ToList();
      var datarows = legalcases.Select(n => new
      {

        Id = n.Id,
        CaseId = n.CaseId,
        Project = n.Project,
        Category = n.Category,
        Status = n.Status,
        Node = n.Node,
        Expires = n.Expires,
        Cause = n.Cause,
        Feature = n.Feature,
        BasedOn = n.BasedOn,
        Subject = n.Subject,
        FromDepartment = n.FromDepartment,
        ToDepartment = n.ToDepartment,
        ToUser = n.ToUser,
        Recorder = n.Recorder,
        Examiner = n.Examiner,
        OriginCaseId = n.OriginCaseId,
        ReceiveDate = n.ReceiveDate?.ToString("yyyy-MM-dd HH:mm:ss"),
        RegisterDate = n.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss"),
        PreUnderlyingAsset = n.PreUnderlyingAsset,
        AllocateDate = n.AllocateDate?.ToString("yyyy-MM-dd HH:mm:ss"),
        PreCloseDate = n.PreCloseDate?.ToString("yyyy-MM-dd HH:mm:ss"),
        ClosedDate = n.ClosedDate?.ToString("yyyy-MM-dd HH:mm:ss"),
        CloseType = n.CloseType,
        UnderlyingAsset = n.UnderlyingAsset,
        Court = n.Court,
        Org = n.Org,
        Accuser = n.Accuser,
        AccuserAddress = n.AccuserAddress,
        Defendant = n.Defendant,
        DefendantAddress = n.DefendantAddress,
        Receivable = n.Receivable,
        Received = n.Received,
        Opinion1 = n.Opinion1,
        Opinion2 = n.Opinion2,
        Opinion3 = n.Opinion3,
        Comment = n.Comment,
        Proposer = n.Proposer,
        FilingDate = n.FilingDate.ToString("yyyy-MM-dd HH:mm:ss"),
        ToDoDate = n.ToDoDate.ToString("yyyy-MM-dd HH:mm:ss")
      }).ToList();
      return await NPOIHelper.ExportExcelAsync(typeof(LegalCase), datarows, expcolopts);
    }
    public void Delete(int[] id)
    {
      var items = this.Queryable().Where(x => id.Contains(x.Id));
     
      foreach (var item in items)
      {
        var temp = new TempCaseId()
        {
          CaseId = item.CaseId,
          SerialNumber = item.SerialNumber,
          Category = item.Category,
          Expires = item.Expires

        };

        var tracks = this.trackHistoryService.Queryable().Where(x => x.CaseId == item.CaseId).ToList();
        foreach (var track in tracks)
        {
          this.trackHistoryService.Delete(track);
        }
        this.logger.Info($"{item.CaseId}-状态:{item.Status} 被删除");
        this.tempCaseIdService.Insert(temp);
        this.Delete(item);
      }

    }
    private async Task<TempCaseId> getdelcaseid(string year,string category, string serialnumber) {
      var prefix = year + serialnumber + category;
      var item =await this.tempCaseIdService.Queryable().Where(x => x.CaseId.Contains(prefix))
        .OrderBy(x => x.CaseId).FirstOrDefaultAsync();
      return item;
    }
    public async Task<LegalCase> InitItemAsync(string name) {
      const string NODENAME = "立案";
      var now = DateTime.Now;
      var node = await this.nodeConfigService.Queryable().Where(x => x.Node == NODENAME).FirstOrDefaultAsync();
      var company = await this.companyService.Queryable().FirstOrDefaultAsync();
      if (node != null)
      {
        return new LegalCase()
        {
          CaseId = $"（{now.Year}）",
          Project = "",
           FromDepartment= "执行局",
           Court= "执行局",
          Status = node.Status,
          Node = NODENAME,
          Expires = node.Expires,
          ReceiveDate = now,
          RegisterDate = now,
          AllocateDate = null,
          PreCloseDate = null,
          ClosedDate = null,
          CloseType = null,
          Proposer = name,
          FilingDate = now,
          ToDoDate = now,
          Org = company?.Name
        };
      }
      else
      {
        return new LegalCase()
        {
          CaseId = "（2020）粤",
          Project = "",
          Status = NODENAME,
          Node = NODENAME,
          Expires = 10,
          ReceiveDate = now,
          RegisterDate = now,
          AllocateDate = null,
          PreCloseDate = null,
          ClosedDate = null,
          CloseType = null,
          Proposer = name,
          FilingDate = now,
          ToDoDate = now,
          Org = company?.Name
        };
      }
    }
    public async Task Register(LegalCase item) {
            item.Subject = $"{item.Accuser}，{item.Defendant}";
      item.Project = item.CaseId;
           
      this.Insert(item);
      var nodeconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);
      var track = this.mapper.Map<TrackHistory>(item);
      track.Owner = item.Proposer;
      track.BeginDate = item.FilingDate;
      track.Expires = nodeconfig.Expires;
      this.trackHistoryService.Insert(track);
      await Task.FromResult("");
    }

    public async Task DoComplete(int id, string user) {
      var now = DateTime.Now;
      var item = await this.FindAsync(id);
      var nodeconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);
      var track = await this.trackHistoryService.GetTrackItem(item.CaseId, item.Node, item.Status);
      if (!string.IsNullOrEmpty(nodeconfig.NextNode))
      {
        item.Node = nodeconfig.NextNode;
        item.Status = nodeconfig.NextStatus;
        item.ToDoDate = now;
        this.Update(item);
        track.NodeStatus = "完成";
        track.CompletedDate = now;
        track.Owner = user;
                track.Elapsed = calDays(item.PreCloseDate.Value, now);
        if (track.Elapsed >= 0)
        {
          track.State = "限期内完成";
        }
        else
        {
          track.State = $"延期{ Math.Abs(track.Elapsed)}天完成";
        }
        this.trackHistoryService.Update(track);
        var nextconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);
        var newtrack = this.mapper.Map<TrackHistory>(item);
        newtrack.Owner = null;
        newtrack.BeginDate = now;
        newtrack.Expires = nextconfig.Expires;
        this.trackHistoryService.Insert(newtrack);
      }
      else
      {

        item.ToDoDate = now;
        item.Status = "完成归档";
        this.Update(item);
        track.NodeStatus = "完成";
        track.CompletedDate = now;
        track.Owner = user;
        track.Elapsed = calDays(item.PreCloseDate.Value, now);
        if (track.Elapsed >= 0)
        {
          track.State = "限期内完成";
        }
        else
        {
          track.State = $"延期{ Math.Abs(track.Elapsed)}天完成";
        }

        this.trackHistoryService.Update(track);
      }


    }
    private int calDays(DateTime dt1, DateTime dt2) {

      var diff = dt1.Subtract(dt2);
      return diff.Days;

    }
    public async Task<string> ExportWord(int id, string templatefile, string destpath) {


      var item = await this.FindAsync(id);
      var filename = $"{item.CaseId}-{item.Status}-{DateTime.Now.ToString("yyyyMMddHHmmss")}.docx";
      var savePath = Path.Combine(destpath, $"{filename}");
            string[] fields = { "Court", "ReceiveDate", "CaseId",
        "Accuser", "AccuserAddress", "Defendant",
        "DefendantAddress", "BasedOn", "Cause",
        "PreUnderlyingAsset", "Receivable",
        "Received", "Opinion1", "ToUser" ,
        "Opinion2", "Proposer", "RegisterDate",
        "Category", "OriginCaseId", "Proposer",
        "FilingDate"};
            string[] values = {item.Court, item.ReceiveDate?.ToString("yyyy年MM月dd日"),item.CaseId,
        item.Accuser,item.AccuserAddress,item.Defendant,
        item.DefendantAddress,item.BasedOn,item.Cause,
        item.PreUnderlyingAsset.ToString("0.0"),item.Receivable==0?"":item.Receivable.ToString("0.0"),
        item.Received==0?"":item.Received.ToString("0.0"),item.Opinion1,item.ToUser,
        item.Opinion2,item.Proposer,item.RegisterDate.ToString("yyyy年MM月dd日"),
        item.Category,item.OriginCaseId,item.Proposer,
        item.FilingDate.ToString("yyyy年MM月dd日")};


      var doc = new Aspose.Words.Document(templatefile);



      if (fields != null && values != null)
      {
        doc.MailMerge.Execute(fields, values);
      }
      doc.Save(savePath, Aspose.Words.SaveFormat.Docx);

      var downloadPath = $"/UploadFiles/{filename}";
      var description = "";
      if (item.Node == "分案" && item.Status == "移交")
      {
        description = $"{item.CaseId}";
      }
      else
      {
        description = $"{item.CaseId}";
      }
      var att = new Attachment()
      {
        CaseId = item.CaseId,
        Description = description,
        DocId = filename,
        Path = downloadPath,
        Ext = ".docx",
        Type = "立案申请表",

      };

      this.attachmentService.Insert(att);


      return await Task.FromResult(downloadPath);
    }

    public async Task BackStep1(int id,string user) {
      var now = DateTime.Now;
      var item = await this.FindAsync(id);
      var nodeconfig = await this.nodeConfigService.GetPrevConfig(item.Node, item.Status);
      var track = await this.trackHistoryService.GetTrackItem(item.CaseId, item.Node, item.Status);
      item.Node = nodeconfig.Node;
      item.Status = nodeconfig.Status;
      item.ToDoDate = track.BeginDate;
      item.ToDepartment = null;
      item.PreCloseDate = null;
      item.ToUser = null;
      item.Recorder = null;
      item.AllocateDate = null;
      item.Opinion2 = null;
            track.NodeStatus = "退回";
      track.CompletedDate = now;
      track.Owner = user;
      track.Comment = "退回 " + nodeconfig.Status;
      track.Elapsed = calDays(track.BeginDate, now);
      this.Update(item);
      this.trackHistoryService.Update(track);
      var newtrack = this.mapper.Map<TrackHistory>(item);
      this.trackHistoryService.Insert(newtrack);


    }
    public async Task ChangeAssigning(AssigningViewModel assigning, string user)
    {
      var list = await this.Queryable().Where(x => assigning.SelectedIds.Contains(x.Id)).ToListAsync();
      var now = DateTime.Now;
      foreach (var item in list)
      {
        var nodeconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);
        var track = await this.trackHistoryService.GetTrackItem(item.CaseId, item.Node, item.Status);
                        item.ToDoDate = now;
        item.ToDepartment = assigning.ToDepartment;
        item.ToUser = assigning.ToUser;
        item.Recorder = assigning.Recorder;
                item.Court = assigning.ToDepartment;
        this.Update(item);
        track.NodeStatus = "变更";
        track.CompletedDate = now;
        track.Owner = user;
        track.Elapsed = calDays(item.PreCloseDate.Value, now);

        if (track.Elapsed >=0  )
        {
          track.State = "变更承办人,新承办人:" + assigning.ToUser;
        }
        else
        {
          track.State = $"延期{ Math.Abs(track.Elapsed) }天 变更承办人,新承办人:" + assigning.ToUser;
        }
        this.trackHistoryService.Update(track);
        var nextconfig = await this.nodeConfigService.GetConfig(nodeconfig.NextNode, nodeconfig.NextStatus);
        var newtrack = this.mapper.Map<TrackHistory>(item);
        newtrack.BeginDate = now;
        newtrack.Owner = null;
        newtrack.Expires = nextconfig.Expires;
        this.trackHistoryService.Insert(newtrack);
      }
    }
    public async Task Assigning(AssigningViewModel assigning,string user) {
      var list =await this.Queryable().Where(x => assigning.SelectedIds.Contains(x.Id)).ToListAsync();
      var now = DateTime.Now;
      foreach (var item in list)
      {
        var nodeconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);
        var track = await this.trackHistoryService.GetTrackItem(item.CaseId, item.Node, item.Status);
        item.Node = nodeconfig.NextNode;
        item.Status = nodeconfig.NextStatus;
        item.ToDoDate = now;
        item.ToDepartment = assigning.ToDepartment;
                                item.ToUser = assigning.ToUser;
        item.Recorder = assigning.Recorder;
        item.AllocateDate = assigning.AllocateDate;
        item.Court = assigning.ToDepartment;
        this.Update(item);
        track.NodeStatus = "完成";
        track.CompletedDate = now;
        track.Owner = user;
        track.Elapsed = calDays(item.PreCloseDate.Value, now);
        if (track.Elapsed >= 0)
        {
          track.State = "限期内完成";
        }
        else
        {
          track.State = $"延期{ Math.Abs(track.Elapsed)}天完成";
        }
        this.trackHistoryService.Update(track);
        var nextconfig = await this.nodeConfigService.GetConfig(nodeconfig.NextNode, nodeconfig.NextStatus);
        var newtrack = this.mapper.Map<TrackHistory>(item);
        newtrack.BeginDate = now;
        newtrack.Owner = null;
        newtrack.Expires = nextconfig.Expires;
        this.trackHistoryService.Insert(newtrack);
      }
    }
    public async Task AssigningTask(LegalCase legalcase,string user){
      var now = DateTime.Now;
      var item =await this.FindAsync(legalcase.Id);
      var nodeconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);
      var track = await this.trackHistoryService.GetTrackItem(item.CaseId, item.Node, item.Status);
      item.Node = nodeconfig.NextNode;
      item.Status = nodeconfig.NextStatus;
      item.ToDoDate = now;
            item.ToDepartment = legalcase.ToDepartment;
      item.ToUser = legalcase.ToUser;
      item.Recorder = legalcase.Recorder;
      item.AllocateDate = legalcase.AllocateDate;
                  item.Court = legalcase.ToDepartment;
      this.Update(item);

      track.NodeStatus = "完成";
      track.CompletedDate = now;
      track.Owner = user;
      track.Elapsed = calDays(item.PreCloseDate.Value, now);
      if (track.Elapsed >= 0)
      {
        track.State = "限期内完成";
      }
      else
      {
        track.State = $"延期{ Math.Abs(track.Elapsed)}天完成";
      }
      this.trackHistoryService.Update(track);
      var nextconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);
     
      var newtrack = this.mapper.Map<TrackHistory>(item);
      newtrack.Owner = null;
      newtrack.BeginDate = now;
      newtrack.Expires = nextconfig.Expires;
      this.trackHistoryService.Insert(newtrack);

    }
    public async Task<Tuple<string,int,int>> GetCaseId(string year, string serial, string category) {
      var prefix = year + serial + category;
                  var delcaseid =true;      if (delcaseid)
      {
                var expires = await this.nodeTimeService.Queryable()
          .Where(x => x.Category == category).SumAsync(x => x.Days);
        var maxcaseid = await this.Queryable().Where(x => x.CaseId.Contains(prefix))
          .Select(x => x.CaseId)
          .MaxAsync();
                var num = 0;
        try
        {
          var sql = $@"select top 1 CONVERT(int,SUBSTRING(caseid,14, Len(CaseId)-14)) num from dbo.LegalCases
where CaseId like N'{prefix}%'
order by  CONVERT(int, SUBSTRING(caseid, 14, Len(CaseId) - 14)) desc";
          var maxnum = this.db.ExecuteScalar<int>(sql);
          num = Convert.ToInt32(maxnum) + 1;
        }
        catch
        {
          num = 1;
        }
        return new Tuple<string, int, int>(prefix + num.ToString() + "号", num, expires);

      }
      else
      {
        return null;
        }
                                    
    }
    public async Task<Tuple<int, int>> GetCaseExpires(string year, string serial, string category)
    {
        var expires = await this.nodeTimeService.Queryable()
          .Where(x => x.Category == category).SumAsync(x => x.Days);
      var prefix = year + serial + category;
      var num = 0;
      try
      {
        var sql = $@"select top 1 CONVERT(int,SUBSTRING(caseid,14, Len(CaseId)-14)) num from dbo.LegalCases
where CaseId like N'{prefix}%'
order by  CONVERT(int, SUBSTRING(caseid, 14, Len(CaseId) - 14)) desc";
        var maxnum = this.db.ExecuteScalar<int>(sql);
        num = Convert.ToInt32(maxnum) + 1;
      }
      catch
      {
        num = 1;
      }


      return new Tuple<int, int>(expires, num);

    }
    public async Task UploadFile(int caseid, string filename, string ext, string path)
    {
      var item = await this.FindAsync(caseid);
      var att = new Attachment()
      {
        CaseId = item.CaseId,
        DocId = filename,
        Description = item.CaseId,
        Ext = ext,
        Path = path,
        Type = "案件相关材料"

      };
      this.attachmentService.Insert(att);

    }
    public async Task DeleteFile(int caseid, string filename) {
      var item = await this.FindAsync(caseid);
      await this.attachmentService.DeleteItem(item.CaseId, filename);
    }

    public async Task ExecutionTask(LegalCase legalcase,string user) {

      var now = DateTime.Now;
      var item = await this.FindAsync(legalcase.Id);
      var nodeconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);
      var istomanager = legalcase.ToManager;
      if (istomanager == false)
      {
        nodeconfig = await this.nodeConfigService.GetConfig(nodeconfig.NextNode, nodeconfig.NextStatus);
        item.ClosedDate = now;
        item.CloseType = "正常结案";
      }
      var track = await this.trackHistoryService.GetTrackItem(item.CaseId, item.Node, item.Status);
      item.Node = nodeconfig.NextNode;
      item.Status = nodeconfig.NextStatus;
      item.ToDoDate = now;
            item.ToManager = istomanager;
      item.Manager = legalcase.Manager;
      this.Update(item);
      track.NodeStatus = "完成";
      track.CompletedDate = now;
      track.Owner = user;
      track.Elapsed = calDays(item.PreCloseDate.Value, now);
      if (track.Elapsed >= 0)
      {
        track.State = "限期内完成";
      }
      else
      {
        track.State = $"延期{ Math.Abs(track.Elapsed)}天完成";
        item.CloseType = "延期结案";
      }

      var nextconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);

      this.trackHistoryService.Update(track);
      var newtrack = this.mapper.Map<TrackHistory>(item);
      newtrack.Owner = null;
      newtrack.BeginDate = now;
      newtrack.Expires = nextconfig.Expires;
      this.trackHistoryService.Insert(newtrack);
    }
    public async Task ManagerTask(LegalCase legalcase,string username) {
      var now = DateTime.Now;
      var item = await this.FindAsync(legalcase.Id);
      var nodeconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);
      var track = await this.trackHistoryService.GetTrackItem(item.CaseId, item.Node, item.Status);
      item.Node = nodeconfig.NextNode;
      item.Status = nodeconfig.NextStatus;
      item.ToDoDate = now;
      item.Opinion3 = legalcase.Opinion3??"同意结案";
      item.ClosedDate = now;
      item.CloseType = "正常结案";
      track.NodeStatus = "完成";
      track.CompletedDate = now;
      track.Owner = username;
      track.Elapsed = calDays(item.PreCloseDate.Value, now);
      if (track.Elapsed >=0)
      {
        track.State = "限期内完成";
      }
      else
      {
        track.State = $"延期{ Math.Abs(track.Elapsed)}天完成";
        item.CloseType = "延期结案";
      }

      this.Update(item);
      this.trackHistoryService.Update(track);
      var nextconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);
      var newtrack = this.mapper.Map<TrackHistory>(item);
      newtrack.Owner = null;
      newtrack.BeginDate = now;
      newtrack.Expires = nextconfig.Expires;
      this.trackHistoryService.Insert(newtrack);
    }
    public async Task CloseTask(LegalCase legalcase,string username)
    {
      var now = DateTime.Now;
      var item = await this.FindAsync(legalcase.Id);
      var nodeconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);
      var track = await this.trackHistoryService.GetTrackItem(item.CaseId, item.Node, item.Status);
      item.Node = nodeconfig.NextNode;
      item.Status = nodeconfig.NextStatus;
      item.ToDoDate = now;
      item.ClosedDate = legalcase.ClosedDate??now;
      item.CloseType = legalcase.CloseType??"正常结案";
      item.UnderlyingAsset = legalcase.UnderlyingAsset==0?item.PreUnderlyingAsset: legalcase.UnderlyingAsset;
      this.Update(item);
      track.NodeStatus = "完成";
      track.CompletedDate = now;
      track.Owner = username;
      track.Elapsed = calDays(item.PreCloseDate.Value, now);
      if (track.Elapsed >= 0)
      {
        track.State = "限期内完成";
      }
      else
      {
        track.State = $"延期{ Math.Abs(track.Elapsed)}天完成";
      }

      this.trackHistoryService.Update(track);
      var nextconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);
      var newtrack = this.mapper.Map<TrackHistory>(item);
      newtrack.Owner = null;
      newtrack.BeginDate = now;
      newtrack.Expires = nextconfig.Expires;
      this.trackHistoryService.Insert(newtrack);
    }

    public async Task GoBack(int id,string user) {
      var now = DateTime.Now;
      var item = await this.FindAsync(id);
      var nodeconfig = await this.nodeConfigService.GetPrevConfig(item.Node, item.Status);
      var track = await this.trackHistoryService.GetTrackItem(item.CaseId, item.Node, item.Status);
      item.Node = nodeconfig.Node;
      item.Status = nodeconfig.Status;
      item.ToDoDate = now;
                                                this.Update(item);
      if (track != null)
      {
        track.NodeStatus = "退回";
        track.CompletedDate = now;
        track.Owner = user;
        track.Comment = "退回至" + nodeconfig.Status;
        track.Elapsed = calDays(track.BeginDate, now);
        
        this.trackHistoryService.Update(track);
      }
      var newtrack = this.mapper.Map<TrackHistory>(item);
      newtrack.Owner = null;
      newtrack.BeginDate = now;
      this.trackHistoryService.Insert(newtrack);
    }
        public async Task ExtractionTask(int id, string user) {
      var now = DateTime.Now;
      var item = await this.FindAsync(id);
      var nodeconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);
      var track = await this.trackHistoryService.GetTrackItem(item.CaseId, item.Node, item.Status);
      item.Node = nodeconfig.NextNode;
      item.Status = nodeconfig.NextStatus;
      item.ToDoDate = now;
      this.Update(item);
      track.NodeStatus = "完成";
      track.CompletedDate = now;
      track.Owner = user;
      track.Elapsed = calDays(item.PreCloseDate.Value, now);
      if (track.Elapsed >= 0)
      {
        track.State = "限期内完成";
      }
      else
      {
        track.State = $"延期{ Math.Abs(track.Elapsed)}天完成";
      }

      this.trackHistoryService.Update(track);
      var nextconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);
      var newtrack = this.mapper.Map<TrackHistory>(item);
      newtrack.Owner = null;
      newtrack.BeginDate = now;
      newtrack.Expires = nextconfig.Expires;
      this.trackHistoryService.Insert(newtrack);

    }

    public async Task ArchiveTask(int id, string user)
    {
      var now = DateTime.Now;
      var item = await this.FindAsync(id);
      var nodeconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);
      var track = await this.trackHistoryService.GetTrackItem(item.CaseId, item.Node, item.Status);
      item.Node = nodeconfig.NextNode;
      item.Status = nodeconfig.NextStatus;
      item.ToDoDate = now;
      this.Update(item);
      track.NodeStatus = "完成";
      track.CompletedDate = now;
      track.Owner = user;
      track.Elapsed = calDays(item.PreCloseDate.Value, now);
      if (track.Elapsed >= 0)
      {
        track.State = "限期内完成";
      }
      else
      {
        track.State = $"延期{ Math.Abs(track.Elapsed)}天完成";
      }
      this.trackHistoryService.Update(track);
      var newtrack = this.mapper.Map<TrackHistory>(item);
      newtrack.Owner = user;
      newtrack.BeginDate = now;
      newtrack.CompletedDate = now;
      newtrack.NodeStatus = "归档完成";
      this.trackHistoryService.Insert(newtrack);

    }
        public async Task<bool> ValidateCaseId(string caseid) {
      var exist =await this.Queryable().Where(x => x.CaseId == caseid).AnyAsync();
      return exist;
      }
    public async Task ToNext(int[] id ,string user) {
      var now = DateTime.Now;
      var items =await this.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
      foreach (var item in items)
      {
        var nodeconfig = await this.nodeConfigService.GetConfig(item.Node, item.Status);
       
        var track = await this.trackHistoryService.GetTrackItem(item.CaseId, item.Node, item.Status);
        if (!string.IsNullOrEmpty(nodeconfig.NextNode))
        {
          item.Node = nodeconfig.NextNode;
          item.Status = nodeconfig.NextStatus;
          item.ToDoDate = now;
          this.Update(item);
          track.NodeStatus = "完成";
          track.CompletedDate = now;
          track.Owner = user;
          track.Elapsed = calDays(item.PreCloseDate.Value, now);
          if (track.Elapsed >= 0 )
          {
            track.State = "限期内完成";
          }
          else
          {
            track.State = $"延期{ Math.Abs(track.Elapsed)}天完成";
          }
          this.trackHistoryService.Update(track);
          var nextconfig = await this.nodeConfigService.GetConfig(nodeconfig.NextNode, nodeconfig.NextStatus);
          var newtrack = this.mapper.Map<TrackHistory>(item);
          newtrack.BeginDate = now;
          newtrack.Owner = null;
          newtrack.Expires = nextconfig.Expires;
          this.trackHistoryService.Insert(newtrack);
        }
        else
        {

                              item.ToDoDate = now;
          item.Status = "完成归档";
          this.Update(item);
          track.NodeStatus = "完成";
          track.CompletedDate = now;
          track.Owner = user;
          track.Elapsed = calDays(item.PreCloseDate.Value, now);
          if (track.Elapsed >= 0)
          {
            track.State = "限期内完成";
          }
          else
          {
            track.State = $"延期{ Math.Abs(track.Elapsed)}天完成";
          }

          this.trackHistoryService.Update(track);
        }
      }

      }
  }
}



