using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity.SqlServer;
using Repository.Pattern.Repositories;
using Repository.Pattern.Ef6;
using System.Web.WebPages;
using WebApp.Models;
using System.Security.Principal;

namespace WebApp.Repositories
{
  /// <summary>
  /// File: LegalCaseQuery.cs
  /// Purpose: easyui datagrid filter query 
  /// Created Date: 3/4/2020 9:28:05 AM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class LegalCaseQuery : QueryObject<LegalCase>
  {
    public LegalCaseQuery Step1Withfilter(IEnumerable<filterRule> filters)
    {
      this.And(x => x.Node == "立案");
      if (filters != null)
      {
        foreach (var rule in filters)
        {
          if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Id == val);
                break;
              case "notequal":
                And(x => x.Id != val);
                break;
              case "less":
                And(x => x.Id < val);
                break;
              case "lessorequal":
                And(x => x.Id <= val);
                break;
              case "greater":
                And(x => x.Id > val);
                break;
              case "greaterorequal":
                And(x => x.Id >= val);
                break;
              default:
                And(x => x.Id == val);
                break;
            }
          }
          if (rule.field == "CaseId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CaseId.Contains(rule.value));
          }
          if (rule.field == "Project" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Project.Contains(rule.value));
          }
          if (rule.field == "Category" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Category.Contains(rule.value));
          }
          if (rule.field == "Status" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Status.Contains(rule.value));
          }
          if (rule.field == "Node" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Node.Contains(rule.value));
          }
          if (rule.field == "Expires" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Expires == val);
                break;
              case "notequal":
                And(x => x.Expires != val);
                break;
              case "less":
                And(x => x.Expires < val);
                break;
              case "lessorequal":
                And(x => x.Expires <= val);
                break;
              case "greater":
                And(x => x.Expires > val);
                break;
              case "greaterorequal":
                And(x => x.Expires >= val);
                break;
              default:
                And(x => x.Expires == val);
                break;
            }
          }
          if (rule.field == "Cause" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Cause.Contains(rule.value));
          }
          if (rule.field == "Feature" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Feature.Contains(rule.value));
          }
          if (rule.field == "BasedOn" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.BasedOn.Contains(rule.value));
          }
          if (rule.field == "Subject" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Subject.Contains(rule.value));
          }
          if (rule.field == "FromDepartment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.FromDepartment.Contains(rule.value));
          }
          if (rule.field == "ToDepartment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.ToDepartment.Contains(rule.value));
          }
          if (rule.field == "ToUser" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.ToUser.Contains(rule.value));
          }
          if (rule.field == "Recorder" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Recorder.Contains(rule.value));
          }
          if (rule.field == "Examiner" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Examiner.Contains(rule.value));
          }
          if (rule.field == "OriginCaseId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.OriginCaseId.Contains(rule.value));
          }
          if (rule.field == "ReceiveDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ReceiveDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ReceiveDate) <= 0);
            }
          }
          if (rule.field == "RegisterDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.RegisterDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.RegisterDate) <= 0);
            }
          }
          if (rule.field == "PreUnderlyingAsset" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.PreUnderlyingAsset == val);
                break;
              case "notequal":
                And(x => x.PreUnderlyingAsset != val);
                break;
              case "less":
                And(x => x.PreUnderlyingAsset < val);
                break;
              case "lessorequal":
                And(x => x.PreUnderlyingAsset <= val);
                break;
              case "greater":
                And(x => x.PreUnderlyingAsset > val);
                break;
              case "greaterorequal":
                And(x => x.PreUnderlyingAsset >= val);
                break;
              default:
                And(x => x.PreUnderlyingAsset == val);
                break;
            }
          }
          if (rule.field == "AllocateDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.AllocateDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.AllocateDate) <= 0);
            }
          }
          if (rule.field == "PreCloseDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.PreCloseDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.PreCloseDate) <= 0);
            }
          }
          if (rule.field == "ClosedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ClosedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ClosedDate) <= 0);
            }
          }
          if (rule.field == "CloseType" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CloseType.Contains(rule.value));
          }
          if (rule.field == "UnderlyingAsset" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.UnderlyingAsset == val);
                break;
              case "notequal":
                And(x => x.UnderlyingAsset != val);
                break;
              case "less":
                And(x => x.UnderlyingAsset < val);
                break;
              case "lessorequal":
                And(x => x.UnderlyingAsset <= val);
                break;
              case "greater":
                And(x => x.UnderlyingAsset > val);
                break;
              case "greaterorequal":
                And(x => x.UnderlyingAsset >= val);
                break;
              default:
                And(x => x.UnderlyingAsset == val);
                break;
            }
          }
          if (rule.field == "Court" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Court.Contains(rule.value));
          }
          if (rule.field == "Org" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Org.Contains(rule.value));
          }
          if (rule.field == "Accuser" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Accuser.Contains(rule.value));
          }
          if (rule.field == "AccuserAddress" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.AccuserAddress.Contains(rule.value));
          }
          if (rule.field == "Defendant" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Defendant.Contains(rule.value));
          }
          if (rule.field == "DefendantAddress" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.DefendantAddress.Contains(rule.value));
          }
          if (rule.field == "Receivable" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Receivable == val);
                break;
              case "notequal":
                And(x => x.Receivable != val);
                break;
              case "less":
                And(x => x.Receivable < val);
                break;
              case "lessorequal":
                And(x => x.Receivable <= val);
                break;
              case "greater":
                And(x => x.Receivable > val);
                break;
              case "greaterorequal":
                And(x => x.Receivable >= val);
                break;
              default:
                And(x => x.Receivable == val);
                break;
            }
          }
          if (rule.field == "Received" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Received == val);
                break;
              case "notequal":
                And(x => x.Received != val);
                break;
              case "less":
                And(x => x.Received < val);
                break;
              case "lessorequal":
                And(x => x.Received <= val);
                break;
              case "greater":
                And(x => x.Received > val);
                break;
              case "greaterorequal":
                And(x => x.Received >= val);
                break;
              default:
                And(x => x.Received == val);
                break;
            }
          }
          if (rule.field == "Opinion1" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion1.Contains(rule.value));
          }
          if (rule.field == "Opinion2" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion2.Contains(rule.value));
          }
          if (rule.field == "Opinion3" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion3.Contains(rule.value));
          }
          if (rule.field == "Comment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Comment.Contains(rule.value));
          }
          if (rule.field == "Proposer" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Proposer.Contains(rule.value));
          }
          if (rule.field == "FilingDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.FilingDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.FilingDate) <= 0);
            }
          }
          if (rule.field == "ToDoDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ToDoDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ToDoDate) <= 0);
            }
          }
          if (rule.field == "CreatedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.CreatedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.CreatedDate) <= 0);
            }
          }
          if (rule.field == "CreatedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CreatedBy.Contains(rule.value));
          }
          if (rule.field == "LastModifiedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.LastModifiedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.LastModifiedDate) <= 0);
            }
          }
          if (rule.field == "LastModifiedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.LastModifiedBy.Contains(rule.value));
          }
          if (rule.field == "TenantId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.TenantId == val);
                break;
              case "notequal":
                And(x => x.TenantId != val);
                break;
              case "less":
                And(x => x.TenantId < val);
                break;
              case "lessorequal":
                And(x => x.TenantId <= val);
                break;
              case "greater":
                And(x => x.TenantId > val);
                break;
              case "greaterorequal":
                And(x => x.TenantId >= val);
                break;
              default:
                And(x => x.TenantId == val);
                break;
            }
          }

        }
      }
      return this;
    }
    public LegalCaseQuery Step2Withfilter(IEnumerable<filterRule> filters)
    {
      this.And(x => x.Node == "分案");
      if (filters != null)
      {
        foreach (var rule in filters)
        {
          if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Id == val);
                break;
              case "notequal":
                And(x => x.Id != val);
                break;
              case "less":
                And(x => x.Id < val);
                break;
              case "lessorequal":
                And(x => x.Id <= val);
                break;
              case "greater":
                And(x => x.Id > val);
                break;
              case "greaterorequal":
                And(x => x.Id >= val);
                break;
              default:
                And(x => x.Id == val);
                break;
            }
          }
          if (rule.field == "CaseId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CaseId.Contains(rule.value));
          }
          if (rule.field == "Project" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Project.Contains(rule.value));
          }
          if (rule.field == "Category" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Category.Contains(rule.value));
          }
          if (rule.field == "Status" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Status.Contains(rule.value));
          }
          if (rule.field == "Node" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Node.Contains(rule.value));
          }
          if (rule.field == "Expires" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Expires == val);
                break;
              case "notequal":
                And(x => x.Expires != val);
                break;
              case "less":
                And(x => x.Expires < val);
                break;
              case "lessorequal":
                And(x => x.Expires <= val);
                break;
              case "greater":
                And(x => x.Expires > val);
                break;
              case "greaterorequal":
                And(x => x.Expires >= val);
                break;
              default:
                And(x => x.Expires == val);
                break;
            }
          }
          if (rule.field == "Cause" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Cause.Contains(rule.value));
          }
          if (rule.field == "Feature" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Feature.Contains(rule.value));
          }
          if (rule.field == "BasedOn" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.BasedOn.Contains(rule.value));
          }
          if (rule.field == "Subject" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Subject.Contains(rule.value));
          }
          if (rule.field == "FromDepartment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.FromDepartment.Contains(rule.value));
          }
          if (rule.field == "ToDepartment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.ToDepartment.Contains(rule.value));
          }
          if (rule.field == "ToUser" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.ToUser.Contains(rule.value));
          }
          if (rule.field == "Recorder" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Recorder.Contains(rule.value));
          }
          if (rule.field == "Examiner" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Examiner.Contains(rule.value));
          }
          if (rule.field == "OriginCaseId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.OriginCaseId.Contains(rule.value));
          }
          if (rule.field == "ReceiveDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ReceiveDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ReceiveDate) <= 0);
            }
          }
          if (rule.field == "RegisterDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.RegisterDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.RegisterDate) <= 0);
            }
          }
          if (rule.field == "PreUnderlyingAsset" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.PreUnderlyingAsset == val);
                break;
              case "notequal":
                And(x => x.PreUnderlyingAsset != val);
                break;
              case "less":
                And(x => x.PreUnderlyingAsset < val);
                break;
              case "lessorequal":
                And(x => x.PreUnderlyingAsset <= val);
                break;
              case "greater":
                And(x => x.PreUnderlyingAsset > val);
                break;
              case "greaterorequal":
                And(x => x.PreUnderlyingAsset >= val);
                break;
              default:
                And(x => x.PreUnderlyingAsset == val);
                break;
            }
          }
          if (rule.field == "AllocateDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.AllocateDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.AllocateDate) <= 0);
            }
          }
          if (rule.field == "PreCloseDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.PreCloseDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.PreCloseDate) <= 0);
            }
          }
          if (rule.field == "ClosedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ClosedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ClosedDate) <= 0);
            }
          }
          if (rule.field == "CloseType" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CloseType.Contains(rule.value));
          }
          if (rule.field == "UnderlyingAsset" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.UnderlyingAsset == val);
                break;
              case "notequal":
                And(x => x.UnderlyingAsset != val);
                break;
              case "less":
                And(x => x.UnderlyingAsset < val);
                break;
              case "lessorequal":
                And(x => x.UnderlyingAsset <= val);
                break;
              case "greater":
                And(x => x.UnderlyingAsset > val);
                break;
              case "greaterorequal":
                And(x => x.UnderlyingAsset >= val);
                break;
              default:
                And(x => x.UnderlyingAsset == val);
                break;
            }
          }
          if (rule.field == "Court" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Court.Contains(rule.value));
          }
          if (rule.field == "Org" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Org.Contains(rule.value));
          }
          if (rule.field == "Accuser" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Accuser.Contains(rule.value));
          }
          if (rule.field == "AccuserAddress" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.AccuserAddress.Contains(rule.value));
          }
          if (rule.field == "Defendant" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Defendant.Contains(rule.value));
          }
          if (rule.field == "DefendantAddress" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.DefendantAddress.Contains(rule.value));
          }
          if (rule.field == "Receivable" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Receivable == val);
                break;
              case "notequal":
                And(x => x.Receivable != val);
                break;
              case "less":
                And(x => x.Receivable < val);
                break;
              case "lessorequal":
                And(x => x.Receivable <= val);
                break;
              case "greater":
                And(x => x.Receivable > val);
                break;
              case "greaterorequal":
                And(x => x.Receivable >= val);
                break;
              default:
                And(x => x.Receivable == val);
                break;
            }
          }
          if (rule.field == "Received" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Received == val);
                break;
              case "notequal":
                And(x => x.Received != val);
                break;
              case "less":
                And(x => x.Received < val);
                break;
              case "lessorequal":
                And(x => x.Received <= val);
                break;
              case "greater":
                And(x => x.Received > val);
                break;
              case "greaterorequal":
                And(x => x.Received >= val);
                break;
              default:
                And(x => x.Received == val);
                break;
            }
          }
          if (rule.field == "Opinion1" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion1.Contains(rule.value));
          }
          if (rule.field == "Opinion2" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion2.Contains(rule.value));
          }
          if (rule.field == "Opinion3" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion3.Contains(rule.value));
          }
          if (rule.field == "Comment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Comment.Contains(rule.value));
          }
          if (rule.field == "Proposer" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Proposer.Contains(rule.value));
          }
          if (rule.field == "FilingDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.FilingDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.FilingDate) <= 0);
            }
          }
          if (rule.field == "ToDoDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ToDoDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ToDoDate) <= 0);
            }
          }
          if (rule.field == "CreatedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.CreatedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.CreatedDate) <= 0);
            }
          }
          if (rule.field == "CreatedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CreatedBy.Contains(rule.value));
          }
          if (rule.field == "LastModifiedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.LastModifiedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.LastModifiedDate) <= 0);
            }
          }
          if (rule.field == "LastModifiedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.LastModifiedBy.Contains(rule.value));
          }
          if (rule.field == "TenantId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.TenantId == val);
                break;
              case "notequal":
                And(x => x.TenantId != val);
                break;
              case "less":
                And(x => x.TenantId < val);
                break;
              case "lessorequal":
                And(x => x.TenantId <= val);
                break;
              case "greater":
                And(x => x.TenantId > val);
                break;
              case "greaterorequal":
                And(x => x.TenantId >= val);
                break;
              default:
                And(x => x.TenantId == val);
                break;
            }
          }

        }
      }
      return this;
    }
    public LegalCaseQuery Step3Withfilter(IEnumerable<filterRule> filters)
    {
      var array = new string[] { "办案", "调取" };
      this.And(x => array.Contains(x.Node) );
      if (filters != null)
      {
        foreach (var rule in filters)
        {
          if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Id == val);
                break;
              case "notequal":
                And(x => x.Id != val);
                break;
              case "less":
                And(x => x.Id < val);
                break;
              case "lessorequal":
                And(x => x.Id <= val);
                break;
              case "greater":
                And(x => x.Id > val);
                break;
              case "greaterorequal":
                And(x => x.Id >= val);
                break;
              default:
                And(x => x.Id == val);
                break;
            }
          }
          if (rule.field == "CaseId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CaseId.Contains(rule.value));
          }
          if (rule.field == "Project" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Project.Contains(rule.value));
          }
          if (rule.field == "Category" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Category.Contains(rule.value));
          }
          if (rule.field == "Status" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Status.Contains(rule.value));
          }
          if (rule.field == "Node" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Node.Contains(rule.value));
          }
          if (rule.field == "Expires" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Expires == val);
                break;
              case "notequal":
                And(x => x.Expires != val);
                break;
              case "less":
                And(x => x.Expires < val);
                break;
              case "lessorequal":
                And(x => x.Expires <= val);
                break;
              case "greater":
                And(x => x.Expires > val);
                break;
              case "greaterorequal":
                And(x => x.Expires >= val);
                break;
              default:
                And(x => x.Expires == val);
                break;
            }
          }
          if (rule.field == "Cause" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Cause.Contains(rule.value));
          }
          if (rule.field == "Feature" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Feature.Contains(rule.value));
          }
          if (rule.field == "BasedOn" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.BasedOn.Contains(rule.value));
          }
          if (rule.field == "Subject" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Subject.Contains(rule.value));
          }
          if (rule.field == "FromDepartment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.FromDepartment.Contains(rule.value));
          }
          if (rule.field == "ToDepartment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.ToDepartment.Contains(rule.value));
          }
          if (rule.field == "ToUser" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.ToUser.Contains(rule.value));
          }
          if (rule.field == "Recorder" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Recorder.Contains(rule.value));
          }
          if (rule.field == "Examiner" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Examiner.Contains(rule.value));
          }
          if (rule.field == "OriginCaseId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.OriginCaseId.Contains(rule.value));
          }
          if (rule.field == "ReceiveDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ReceiveDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ReceiveDate) <= 0);
            }
          }
          if (rule.field == "RegisterDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.RegisterDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.RegisterDate) <= 0);
            }
          }
          if (rule.field == "PreUnderlyingAsset" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.PreUnderlyingAsset == val);
                break;
              case "notequal":
                And(x => x.PreUnderlyingAsset != val);
                break;
              case "less":
                And(x => x.PreUnderlyingAsset < val);
                break;
              case "lessorequal":
                And(x => x.PreUnderlyingAsset <= val);
                break;
              case "greater":
                And(x => x.PreUnderlyingAsset > val);
                break;
              case "greaterorequal":
                And(x => x.PreUnderlyingAsset >= val);
                break;
              default:
                And(x => x.PreUnderlyingAsset == val);
                break;
            }
          }
          if (rule.field == "AllocateDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.AllocateDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.AllocateDate) <= 0);
            }
          }
          if (rule.field == "PreCloseDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.PreCloseDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.PreCloseDate) <= 0);
            }
          }
          if (rule.field == "ClosedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ClosedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ClosedDate) <= 0);
            }
          }
          if (rule.field == "CloseType" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CloseType.Contains(rule.value));
          }
          if (rule.field == "UnderlyingAsset" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.UnderlyingAsset == val);
                break;
              case "notequal":
                And(x => x.UnderlyingAsset != val);
                break;
              case "less":
                And(x => x.UnderlyingAsset < val);
                break;
              case "lessorequal":
                And(x => x.UnderlyingAsset <= val);
                break;
              case "greater":
                And(x => x.UnderlyingAsset > val);
                break;
              case "greaterorequal":
                And(x => x.UnderlyingAsset >= val);
                break;
              default:
                And(x => x.UnderlyingAsset == val);
                break;
            }
          }
          if (rule.field == "Court" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Court.Contains(rule.value));
          }
          if (rule.field == "Org" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Org.Contains(rule.value));
          }
          if (rule.field == "Accuser" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Accuser.Contains(rule.value));
          }
          if (rule.field == "AccuserAddress" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.AccuserAddress.Contains(rule.value));
          }
          if (rule.field == "Defendant" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Defendant.Contains(rule.value));
          }
          if (rule.field == "DefendantAddress" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.DefendantAddress.Contains(rule.value));
          }
          if (rule.field == "Receivable" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Receivable == val);
                break;
              case "notequal":
                And(x => x.Receivable != val);
                break;
              case "less":
                And(x => x.Receivable < val);
                break;
              case "lessorequal":
                And(x => x.Receivable <= val);
                break;
              case "greater":
                And(x => x.Receivable > val);
                break;
              case "greaterorequal":
                And(x => x.Receivable >= val);
                break;
              default:
                And(x => x.Receivable == val);
                break;
            }
          }
          if (rule.field == "Received" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Received == val);
                break;
              case "notequal":
                And(x => x.Received != val);
                break;
              case "less":
                And(x => x.Received < val);
                break;
              case "lessorequal":
                And(x => x.Received <= val);
                break;
              case "greater":
                And(x => x.Received > val);
                break;
              case "greaterorequal":
                And(x => x.Received >= val);
                break;
              default:
                And(x => x.Received == val);
                break;
            }
          }
          if (rule.field == "Opinion1" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion1.Contains(rule.value));
          }
          if (rule.field == "Opinion2" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion2.Contains(rule.value));
          }
          if (rule.field == "Opinion3" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion3.Contains(rule.value));
          }
          if (rule.field == "Comment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Comment.Contains(rule.value));
          }
          if (rule.field == "Proposer" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Proposer.Contains(rule.value));
          }
          if (rule.field == "FilingDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.FilingDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.FilingDate) <= 0);
            }
          }
          if (rule.field == "ToDoDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ToDoDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ToDoDate) <= 0);
            }
          }
          if (rule.field == "CreatedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.CreatedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.CreatedDate) <= 0);
            }
          }
          if (rule.field == "CreatedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CreatedBy.Contains(rule.value));
          }
          if (rule.field == "LastModifiedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.LastModifiedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.LastModifiedDate) <= 0);
            }
          }
          if (rule.field == "LastModifiedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.LastModifiedBy.Contains(rule.value));
          }
          if (rule.field == "TenantId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.TenantId == val);
                break;
              case "notequal":
                And(x => x.TenantId != val);
                break;
              case "less":
                And(x => x.TenantId < val);
                break;
              case "lessorequal":
                And(x => x.TenantId <= val);
                break;
              case "greater":
                And(x => x.TenantId > val);
                break;
              case "greaterorequal":
                And(x => x.TenantId >= val);
                break;
              default:
                And(x => x.TenantId == val);
                break;
            }
          }

        }
      }
      return this;
    }
    public LegalCaseQuery Step3WithMefilter(string touser,bool ismanager,IEnumerable<filterRule> filters)
    {
      var array = new string[] { "办案"};
      this.And(x => array.Contains(x.Node) && x.Status!="结案");
      this.And(x => x.ToUser == touser || x.Recorder == touser);
      if (!ismanager)
      {
       
      }
      if (filters != null)
      {
        foreach (var rule in filters)
        {
          if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Id == val);
                break;
              case "notequal":
                And(x => x.Id != val);
                break;
              case "less":
                And(x => x.Id < val);
                break;
              case "lessorequal":
                And(x => x.Id <= val);
                break;
              case "greater":
                And(x => x.Id > val);
                break;
              case "greaterorequal":
                And(x => x.Id >= val);
                break;
              default:
                And(x => x.Id == val);
                break;
            }
          }
          if (rule.field == "CaseId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CaseId.Contains(rule.value));
          }
          if (rule.field == "Project" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Project.Contains(rule.value));
          }
          if (rule.field == "Category" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Category.Contains(rule.value));
          }
          if (rule.field == "Status" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Status.Contains(rule.value));
          }
          if (rule.field == "Node" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Node.Contains(rule.value));
          }
          if (rule.field == "Expires" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Expires == val);
                break;
              case "notequal":
                And(x => x.Expires != val);
                break;
              case "less":
                And(x => x.Expires < val);
                break;
              case "lessorequal":
                And(x => x.Expires <= val);
                break;
              case "greater":
                And(x => x.Expires > val);
                break;
              case "greaterorequal":
                And(x => x.Expires >= val);
                break;
              default:
                And(x => x.Expires == val);
                break;
            }
          }
          if (rule.field == "Cause" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Cause.Contains(rule.value));
          }
          if (rule.field == "Feature" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Feature.Contains(rule.value));
          }
          if (rule.field == "BasedOn" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.BasedOn.Contains(rule.value));
          }
          if (rule.field == "Subject" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Subject.Contains(rule.value));
          }
          if (rule.field == "FromDepartment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.FromDepartment.Contains(rule.value));
          }
          if (rule.field == "ToDepartment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.ToDepartment.Contains(rule.value));
          }
          if (rule.field == "ToUser" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.ToUser.Contains(rule.value));
          }
          if (rule.field == "Recorder" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Recorder.Contains(rule.value));
          }
          if (rule.field == "Examiner" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Examiner.Contains(rule.value));
          }
          if (rule.field == "OriginCaseId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.OriginCaseId.Contains(rule.value));
          }
          if (rule.field == "ReceiveDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ReceiveDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ReceiveDate) <= 0);
            }
          }
          if (rule.field == "RegisterDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.RegisterDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.RegisterDate) <= 0);
            }
          }
          if (rule.field == "PreUnderlyingAsset" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.PreUnderlyingAsset == val);
                break;
              case "notequal":
                And(x => x.PreUnderlyingAsset != val);
                break;
              case "less":
                And(x => x.PreUnderlyingAsset < val);
                break;
              case "lessorequal":
                And(x => x.PreUnderlyingAsset <= val);
                break;
              case "greater":
                And(x => x.PreUnderlyingAsset > val);
                break;
              case "greaterorequal":
                And(x => x.PreUnderlyingAsset >= val);
                break;
              default:
                And(x => x.PreUnderlyingAsset == val);
                break;
            }
          }
          if (rule.field == "AllocateDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.AllocateDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.AllocateDate) <= 0);
            }
          }
          if (rule.field == "PreCloseDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.PreCloseDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.PreCloseDate) <= 0);
            }
          }
          if (rule.field == "ClosedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ClosedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ClosedDate) <= 0);
            }
          }
          if (rule.field == "CloseType" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CloseType.Contains(rule.value));
          }
          if (rule.field == "UnderlyingAsset" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.UnderlyingAsset == val);
                break;
              case "notequal":
                And(x => x.UnderlyingAsset != val);
                break;
              case "less":
                And(x => x.UnderlyingAsset < val);
                break;
              case "lessorequal":
                And(x => x.UnderlyingAsset <= val);
                break;
              case "greater":
                And(x => x.UnderlyingAsset > val);
                break;
              case "greaterorequal":
                And(x => x.UnderlyingAsset >= val);
                break;
              default:
                And(x => x.UnderlyingAsset == val);
                break;
            }
          }
          if (rule.field == "Court" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Court.Contains(rule.value));
          }
          if (rule.field == "Org" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Org.Contains(rule.value));
          }
          if (rule.field == "Accuser" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Accuser.Contains(rule.value));
          }
          if (rule.field == "AccuserAddress" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.AccuserAddress.Contains(rule.value));
          }
          if (rule.field == "Defendant" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Defendant.Contains(rule.value));
          }
          if (rule.field == "DefendantAddress" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.DefendantAddress.Contains(rule.value));
          }
          if (rule.field == "Receivable" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Receivable == val);
                break;
              case "notequal":
                And(x => x.Receivable != val);
                break;
              case "less":
                And(x => x.Receivable < val);
                break;
              case "lessorequal":
                And(x => x.Receivable <= val);
                break;
              case "greater":
                And(x => x.Receivable > val);
                break;
              case "greaterorequal":
                And(x => x.Receivable >= val);
                break;
              default:
                And(x => x.Receivable == val);
                break;
            }
          }
          if (rule.field == "Received" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Received == val);
                break;
              case "notequal":
                And(x => x.Received != val);
                break;
              case "less":
                And(x => x.Received < val);
                break;
              case "lessorequal":
                And(x => x.Received <= val);
                break;
              case "greater":
                And(x => x.Received > val);
                break;
              case "greaterorequal":
                And(x => x.Received >= val);
                break;
              default:
                And(x => x.Received == val);
                break;
            }
          }
          if (rule.field == "Opinion1" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion1.Contains(rule.value));
          }
          if (rule.field == "Opinion2" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion2.Contains(rule.value));
          }
          if (rule.field == "Opinion3" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion3.Contains(rule.value));
          }
          if (rule.field == "Comment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Comment.Contains(rule.value));
          }
          if (rule.field == "Proposer" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Proposer.Contains(rule.value));
          }
          if (rule.field == "FilingDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.FilingDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.FilingDate) <= 0);
            }
          }
          if (rule.field == "ToDoDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ToDoDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ToDoDate) <= 0);
            }
          }
          if (rule.field == "CreatedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.CreatedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.CreatedDate) <= 0);
            }
          }
          if (rule.field == "CreatedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CreatedBy.Contains(rule.value));
          }
          if (rule.field == "LastModifiedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.LastModifiedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.LastModifiedDate) <= 0);
            }
          }
          if (rule.field == "LastModifiedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.LastModifiedBy.Contains(rule.value));
          }
          if (rule.field == "TenantId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.TenantId == val);
                break;
              case "notequal":
                And(x => x.TenantId != val);
                break;
              case "less":
                And(x => x.TenantId < val);
                break;
              case "lessorequal":
                And(x => x.TenantId <= val);
                break;
              case "greater":
                And(x => x.TenantId > val);
                break;
              case "greaterorequal":
                And(x => x.TenantId >= val);
                break;
              default:
                And(x => x.TenantId == val);
                break;
            }
          }

        }
      }
      return this;
    }
    public LegalCaseQuery Step6Withfilter(IEnumerable<filterRule> filters,string user)
    {
      this.And(x => x.Node == "办案" && x.Status=="审批" && x.Manager==user);
      if (filters != null)
      {
        foreach (var rule in filters)
        {
          if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Id == val);
                break;
              case "notequal":
                And(x => x.Id != val);
                break;
              case "less":
                And(x => x.Id < val);
                break;
              case "lessorequal":
                And(x => x.Id <= val);
                break;
              case "greater":
                And(x => x.Id > val);
                break;
              case "greaterorequal":
                And(x => x.Id >= val);
                break;
              default:
                And(x => x.Id == val);
                break;
            }
          }
          if (rule.field == "CaseId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CaseId.Contains(rule.value));
          }
          if (rule.field == "Project" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Project.Contains(rule.value));
          }
          if (rule.field == "Category" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Category.Contains(rule.value));
          }
          if (rule.field == "Status" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Status.Contains(rule.value));
          }
          if (rule.field == "Node" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Node.Contains(rule.value));
          }
          if (rule.field == "Expires" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Expires == val);
                break;
              case "notequal":
                And(x => x.Expires != val);
                break;
              case "less":
                And(x => x.Expires < val);
                break;
              case "lessorequal":
                And(x => x.Expires <= val);
                break;
              case "greater":
                And(x => x.Expires > val);
                break;
              case "greaterorequal":
                And(x => x.Expires >= val);
                break;
              default:
                And(x => x.Expires == val);
                break;
            }
          }
          if (rule.field == "Cause" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Cause.Contains(rule.value));
          }
          if (rule.field == "Feature" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Feature.Contains(rule.value));
          }
          if (rule.field == "BasedOn" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.BasedOn.Contains(rule.value));
          }
          if (rule.field == "Subject" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Subject.Contains(rule.value));
          }
          if (rule.field == "FromDepartment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.FromDepartment.Contains(rule.value));
          }
          if (rule.field == "ToDepartment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.ToDepartment.Contains(rule.value));
          }
          if (rule.field == "ToUser" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.ToUser.Contains(rule.value));
          }
          if (rule.field == "Recorder" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Recorder.Contains(rule.value));
          }
          if (rule.field == "Examiner" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Examiner.Contains(rule.value));
          }
          if (rule.field == "OriginCaseId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.OriginCaseId.Contains(rule.value));
          }
          if (rule.field == "ReceiveDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ReceiveDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ReceiveDate) <= 0);
            }
          }
          if (rule.field == "RegisterDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.RegisterDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.RegisterDate) <= 0);
            }
          }
          if (rule.field == "PreUnderlyingAsset" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.PreUnderlyingAsset == val);
                break;
              case "notequal":
                And(x => x.PreUnderlyingAsset != val);
                break;
              case "less":
                And(x => x.PreUnderlyingAsset < val);
                break;
              case "lessorequal":
                And(x => x.PreUnderlyingAsset <= val);
                break;
              case "greater":
                And(x => x.PreUnderlyingAsset > val);
                break;
              case "greaterorequal":
                And(x => x.PreUnderlyingAsset >= val);
                break;
              default:
                And(x => x.PreUnderlyingAsset == val);
                break;
            }
          }
          if (rule.field == "AllocateDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.AllocateDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.AllocateDate) <= 0);
            }
          }
          if (rule.field == "PreCloseDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.PreCloseDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.PreCloseDate) <= 0);
            }
          }
          if (rule.field == "ClosedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ClosedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ClosedDate) <= 0);
            }
          }
          if (rule.field == "CloseType" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CloseType.Contains(rule.value));
          }
          if (rule.field == "UnderlyingAsset" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.UnderlyingAsset == val);
                break;
              case "notequal":
                And(x => x.UnderlyingAsset != val);
                break;
              case "less":
                And(x => x.UnderlyingAsset < val);
                break;
              case "lessorequal":
                And(x => x.UnderlyingAsset <= val);
                break;
              case "greater":
                And(x => x.UnderlyingAsset > val);
                break;
              case "greaterorequal":
                And(x => x.UnderlyingAsset >= val);
                break;
              default:
                And(x => x.UnderlyingAsset == val);
                break;
            }
          }
          if (rule.field == "Court" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Court.Contains(rule.value));
          }
          if (rule.field == "Org" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Org.Contains(rule.value));
          }
          if (rule.field == "Accuser" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Accuser.Contains(rule.value));
          }
          if (rule.field == "AccuserAddress" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.AccuserAddress.Contains(rule.value));
          }
          if (rule.field == "Defendant" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Defendant.Contains(rule.value));
          }
          if (rule.field == "DefendantAddress" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.DefendantAddress.Contains(rule.value));
          }
          if (rule.field == "Receivable" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Receivable == val);
                break;
              case "notequal":
                And(x => x.Receivable != val);
                break;
              case "less":
                And(x => x.Receivable < val);
                break;
              case "lessorequal":
                And(x => x.Receivable <= val);
                break;
              case "greater":
                And(x => x.Receivable > val);
                break;
              case "greaterorequal":
                And(x => x.Receivable >= val);
                break;
              default:
                And(x => x.Receivable == val);
                break;
            }
          }
          if (rule.field == "Received" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Received == val);
                break;
              case "notequal":
                And(x => x.Received != val);
                break;
              case "less":
                And(x => x.Received < val);
                break;
              case "lessorequal":
                And(x => x.Received <= val);
                break;
              case "greater":
                And(x => x.Received > val);
                break;
              case "greaterorequal":
                And(x => x.Received >= val);
                break;
              default:
                And(x => x.Received == val);
                break;
            }
          }
          if (rule.field == "Opinion1" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion1.Contains(rule.value));
          }
          if (rule.field == "Opinion2" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion2.Contains(rule.value));
          }
          if (rule.field == "Opinion3" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion3.Contains(rule.value));
          }
          if (rule.field == "Comment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Comment.Contains(rule.value));
          }
          if (rule.field == "Proposer" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Proposer.Contains(rule.value));
          }
          if (rule.field == "FilingDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.FilingDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.FilingDate) <= 0);
            }
          }
          if (rule.field == "ToDoDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ToDoDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ToDoDate) <= 0);
            }
          }
          if (rule.field == "CreatedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.CreatedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.CreatedDate) <= 0);
            }
          }
          if (rule.field == "CreatedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CreatedBy.Contains(rule.value));
          }
          if (rule.field == "LastModifiedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.LastModifiedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.LastModifiedDate) <= 0);
            }
          }
          if (rule.field == "LastModifiedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.LastModifiedBy.Contains(rule.value));
          }
          if (rule.field == "TenantId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.TenantId == val);
                break;
              case "notequal":
                And(x => x.TenantId != val);
                break;
              case "less":
                And(x => x.TenantId < val);
                break;
              case "lessorequal":
                And(x => x.TenantId <= val);
                break;
              case "greater":
                And(x => x.TenantId > val);
                break;
              case "greaterorequal":
                And(x => x.TenantId >= val);
                break;
              default:
                And(x => x.TenantId == val);
                break;
            }
          }

        }
      }
      return this;
    }
    public LegalCaseQuery Step7Withfilter(IEnumerable<filterRule> filters, IPrincipal user)
    {
      var fullname = Auth.GetFullName(user.Identity.Name);

      var array = new string[] { "归档", "调取" };
      this.And(x => array.Contains(x.Node) || x.Status=="结案");
      if (user.IsInRole("内勤人员"))
      {
        this.And(x => true );
      }
      else if (user.IsInRole("承办人"))
      {
        this.And(x => x.ToUser == fullname　|| x.Recorder==fullname);
      }else if (user.IsInRole("书记员"))
      {
        this.And(x => x.Recorder == fullname || x.ToUser == fullname);
      }else if (user.IsInRole("立案人"))
      {
        this.And(x => x.Proposer == fullname);
      }else if (user.IsInRole("部门领导"))
      {
        this.And(x => x.Manager == fullname);
      }

        if (filters != null)
      {
        foreach (var rule in filters)
        {
          if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Id == val);
                break;
              case "notequal":
                And(x => x.Id != val);
                break;
              case "less":
                And(x => x.Id < val);
                break;
              case "lessorequal":
                And(x => x.Id <= val);
                break;
              case "greater":
                And(x => x.Id > val);
                break;
              case "greaterorequal":
                And(x => x.Id >= val);
                break;
              default:
                And(x => x.Id == val);
                break;
            }
          }
          if (rule.field == "CaseId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CaseId.Contains(rule.value));
          }
          if (rule.field == "Project" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Project.Contains(rule.value));
          }
          if (rule.field == "Category" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Category.Contains(rule.value));
          }
          if (rule.field == "Status" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Status.Contains(rule.value));
          }
          if (rule.field == "Node" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Node.Contains(rule.value));
          }
          if (rule.field == "Expires" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Expires == val);
                break;
              case "notequal":
                And(x => x.Expires != val);
                break;
              case "less":
                And(x => x.Expires < val);
                break;
              case "lessorequal":
                And(x => x.Expires <= val);
                break;
              case "greater":
                And(x => x.Expires > val);
                break;
              case "greaterorequal":
                And(x => x.Expires >= val);
                break;
              default:
                And(x => x.Expires == val);
                break;
            }
          }
          if (rule.field == "Cause" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Cause.Contains(rule.value));
          }
          if (rule.field == "Feature" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Feature.Contains(rule.value));
          }
          if (rule.field == "BasedOn" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.BasedOn.Contains(rule.value));
          }
          if (rule.field == "Subject" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Subject.Contains(rule.value));
          }
          if (rule.field == "FromDepartment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.FromDepartment.Contains(rule.value));
          }
          if (rule.field == "ToDepartment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.ToDepartment.Contains(rule.value));
          }
          if (rule.field == "ToUser" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.ToUser.Contains(rule.value));
          }
          if (rule.field == "Recorder" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Recorder.Contains(rule.value));
          }
          if (rule.field == "Examiner" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Examiner.Contains(rule.value));
          }
          if (rule.field == "OriginCaseId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.OriginCaseId.Contains(rule.value));
          }
          if (rule.field == "ReceiveDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ReceiveDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ReceiveDate) <= 0);
            }
          }
          if (rule.field == "RegisterDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.RegisterDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.RegisterDate) <= 0);
            }
          }
          if (rule.field == "PreUnderlyingAsset" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.PreUnderlyingAsset == val);
                break;
              case "notequal":
                And(x => x.PreUnderlyingAsset != val);
                break;
              case "less":
                And(x => x.PreUnderlyingAsset < val);
                break;
              case "lessorequal":
                And(x => x.PreUnderlyingAsset <= val);
                break;
              case "greater":
                And(x => x.PreUnderlyingAsset > val);
                break;
              case "greaterorequal":
                And(x => x.PreUnderlyingAsset >= val);
                break;
              default:
                And(x => x.PreUnderlyingAsset == val);
                break;
            }
          }
          if (rule.field == "AllocateDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.AllocateDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.AllocateDate) <= 0);
            }
          }
          if (rule.field == "PreCloseDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.PreCloseDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.PreCloseDate) <= 0);
            }
          }
          if (rule.field == "ClosedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ClosedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ClosedDate) <= 0);
            }
          }
          if (rule.field == "CloseType" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CloseType.Contains(rule.value));
          }
          if (rule.field == "UnderlyingAsset" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.UnderlyingAsset == val);
                break;
              case "notequal":
                And(x => x.UnderlyingAsset != val);
                break;
              case "less":
                And(x => x.UnderlyingAsset < val);
                break;
              case "lessorequal":
                And(x => x.UnderlyingAsset <= val);
                break;
              case "greater":
                And(x => x.UnderlyingAsset > val);
                break;
              case "greaterorequal":
                And(x => x.UnderlyingAsset >= val);
                break;
              default:
                And(x => x.UnderlyingAsset == val);
                break;
            }
          }
          if (rule.field == "Court" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Court.Contains(rule.value));
          }
          if (rule.field == "Org" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Org.Contains(rule.value));
          }
          if (rule.field == "Accuser" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Accuser.Contains(rule.value));
          }
          if (rule.field == "AccuserAddress" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.AccuserAddress.Contains(rule.value));
          }
          if (rule.field == "Defendant" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Defendant.Contains(rule.value));
          }
          if (rule.field == "DefendantAddress" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.DefendantAddress.Contains(rule.value));
          }
          if (rule.field == "Receivable" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Receivable == val);
                break;
              case "notequal":
                And(x => x.Receivable != val);
                break;
              case "less":
                And(x => x.Receivable < val);
                break;
              case "lessorequal":
                And(x => x.Receivable <= val);
                break;
              case "greater":
                And(x => x.Receivable > val);
                break;
              case "greaterorequal":
                And(x => x.Receivable >= val);
                break;
              default:
                And(x => x.Receivable == val);
                break;
            }
          }
          if (rule.field == "Received" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Received == val);
                break;
              case "notequal":
                And(x => x.Received != val);
                break;
              case "less":
                And(x => x.Received < val);
                break;
              case "lessorequal":
                And(x => x.Received <= val);
                break;
              case "greater":
                And(x => x.Received > val);
                break;
              case "greaterorequal":
                And(x => x.Received >= val);
                break;
              default:
                And(x => x.Received == val);
                break;
            }
          }
          if (rule.field == "Opinion1" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion1.Contains(rule.value));
          }
          if (rule.field == "Opinion2" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion2.Contains(rule.value));
          }
          if (rule.field == "Opinion3" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion3.Contains(rule.value));
          }
          if (rule.field == "Comment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Comment.Contains(rule.value));
          }
          if (rule.field == "Proposer" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Proposer.Contains(rule.value));
          }
          if (rule.field == "FilingDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.FilingDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.FilingDate) <= 0);
            }
          }
          if (rule.field == "ToDoDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ToDoDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ToDoDate) <= 0);
            }
          }
          if (rule.field == "CreatedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.CreatedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.CreatedDate) <= 0);
            }
          }
          if (rule.field == "CreatedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CreatedBy.Contains(rule.value));
          }
          if (rule.field == "LastModifiedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.LastModifiedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.LastModifiedDate) <= 0);
            }
          }
          if (rule.field == "LastModifiedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.LastModifiedBy.Contains(rule.value));
          }
          if (rule.field == "TenantId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.TenantId == val);
                break;
              case "notequal":
                And(x => x.TenantId != val);
                break;
              case "less":
                And(x => x.TenantId < val);
                break;
              case "lessorequal":
                And(x => x.TenantId <= val);
                break;
              case "greater":
                And(x => x.TenantId > val);
                break;
              case "greaterorequal":
                And(x => x.TenantId >= val);
                break;
              default:
                And(x => x.TenantId == val);
                break;
            }
          }

        }
      }
      return this;
    }
    public LegalCaseQuery Withfilter(IEnumerable<filterRule> filters)
    {
      if (filters != null)
      {
        foreach (var rule in filters)
        {
          if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Id == val);
                break;
              case "notequal":
                And(x => x.Id != val);
                break;
              case "less":
                And(x => x.Id < val);
                break;
              case "lessorequal":
                And(x => x.Id <= val);
                break;
              case "greater":
                And(x => x.Id > val);
                break;
              case "greaterorequal":
                And(x => x.Id >= val);
                break;
              default:
                And(x => x.Id == val);
                break;
            }
          }
          if (rule.field == "CaseId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CaseId.Contains(rule.value));
          }
          if (rule.field == "Project" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Project.Contains(rule.value));
          }
          if (rule.field == "Category" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Category.Contains(rule.value));
          }
          if (rule.field == "Status" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Status.Contains(rule.value));
          }
          if (rule.field == "Node" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Node.Contains(rule.value));
          }
          if (rule.field == "Expires" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Expires == val);
                break;
              case "notequal":
                And(x => x.Expires != val);
                break;
              case "less":
                And(x => x.Expires < val);
                break;
              case "lessorequal":
                And(x => x.Expires <= val);
                break;
              case "greater":
                And(x => x.Expires > val);
                break;
              case "greaterorequal":
                And(x => x.Expires >= val);
                break;
              default:
                And(x => x.Expires == val);
                break;
            }
          }
          if (rule.field == "Cause" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Cause.Contains(rule.value));
          }
          if (rule.field == "Feature" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Feature.Contains(rule.value));
          }
          if (rule.field == "BasedOn" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.BasedOn.Contains(rule.value));
          }
          if (rule.field == "Subject" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Subject.Contains(rule.value));
          }
          if (rule.field == "FromDepartment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.FromDepartment.Contains(rule.value));
          }
          if (rule.field == "ToDepartment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.ToDepartment.Contains(rule.value));
          }
          if (rule.field == "ToUser" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.ToUser.Contains(rule.value));
          }
          if (rule.field == "Recorder" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Recorder.Contains(rule.value));
          }
          if (rule.field == "Examiner" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Examiner.Contains(rule.value));
          }
          if (rule.field == "OriginCaseId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.OriginCaseId.Contains(rule.value));
          }
          if (rule.field == "ReceiveDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ReceiveDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ReceiveDate) <= 0);
            }
          }
          if (rule.field == "RegisterDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.RegisterDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.RegisterDate) <= 0);
            }
          }
          if (rule.field == "PreUnderlyingAsset" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.PreUnderlyingAsset == val);
                break;
              case "notequal":
                And(x => x.PreUnderlyingAsset != val);
                break;
              case "less":
                And(x => x.PreUnderlyingAsset < val);
                break;
              case "lessorequal":
                And(x => x.PreUnderlyingAsset <= val);
                break;
              case "greater":
                And(x => x.PreUnderlyingAsset > val);
                break;
              case "greaterorequal":
                And(x => x.PreUnderlyingAsset >= val);
                break;
              default:
                And(x => x.PreUnderlyingAsset == val);
                break;
            }
          }
          if (rule.field == "AllocateDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.AllocateDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.AllocateDate) <= 0);
            }
          }
          if (rule.field == "PreCloseDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.PreCloseDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.PreCloseDate) <= 0);
            }
          }
          if (rule.field == "ClosedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ClosedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ClosedDate) <= 0);
            }
          }
          if (rule.field == "CloseType" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CloseType.Contains(rule.value));
          }
          if (rule.field == "UnderlyingAsset" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.UnderlyingAsset == val);
                break;
              case "notequal":
                And(x => x.UnderlyingAsset != val);
                break;
              case "less":
                And(x => x.UnderlyingAsset < val);
                break;
              case "lessorequal":
                And(x => x.UnderlyingAsset <= val);
                break;
              case "greater":
                And(x => x.UnderlyingAsset > val);
                break;
              case "greaterorequal":
                And(x => x.UnderlyingAsset >= val);
                break;
              default:
                And(x => x.UnderlyingAsset == val);
                break;
            }
          }
          if (rule.field == "Court" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Court.Contains(rule.value));
          }
          if (rule.field == "Org" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Org.Contains(rule.value));
          }
          if (rule.field == "Accuser" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Accuser.Contains(rule.value));
          }
          if (rule.field == "AccuserAddress" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.AccuserAddress.Contains(rule.value));
          }
          if (rule.field == "Defendant" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Defendant.Contains(rule.value));
          }
          if (rule.field == "DefendantAddress" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.DefendantAddress.Contains(rule.value));
          }
          if (rule.field == "Receivable" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Receivable == val);
                break;
              case "notequal":
                And(x => x.Receivable != val);
                break;
              case "less":
                And(x => x.Receivable < val);
                break;
              case "lessorequal":
                And(x => x.Receivable <= val);
                break;
              case "greater":
                And(x => x.Receivable > val);
                break;
              case "greaterorequal":
                And(x => x.Receivable >= val);
                break;
              default:
                And(x => x.Receivable == val);
                break;
            }
          }
          if (rule.field == "Received" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Received == val);
                break;
              case "notequal":
                And(x => x.Received != val);
                break;
              case "less":
                And(x => x.Received < val);
                break;
              case "lessorequal":
                And(x => x.Received <= val);
                break;
              case "greater":
                And(x => x.Received > val);
                break;
              case "greaterorequal":
                And(x => x.Received >= val);
                break;
              default:
                And(x => x.Received == val);
                break;
            }
          }
          if (rule.field == "Opinion1" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion1.Contains(rule.value));
          }
          if (rule.field == "Opinion2" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion2.Contains(rule.value));
          }
          if (rule.field == "Opinion3" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Opinion3.Contains(rule.value));
          }
          if (rule.field == "Comment" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Comment.Contains(rule.value));
          }
          if (rule.field == "Proposer" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Proposer.Contains(rule.value));
          }
          if (rule.field == "FilingDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.FilingDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.FilingDate) <= 0);
            }
          }
          if (rule.field == "ToDoDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.ToDoDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.ToDoDate) <= 0);
            }
          }
          if (rule.field == "CreatedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.CreatedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.CreatedDate) <= 0);
            }
          }
          if (rule.field == "CreatedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CreatedBy.Contains(rule.value));
          }
          if (rule.field == "LastModifiedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.LastModifiedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.LastModifiedDate) <= 0);
            }
          }
          if (rule.field == "LastModifiedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.LastModifiedBy.Contains(rule.value));
          }
          if (rule.field == "TenantId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.TenantId == val);
                break;
              case "notequal":
                And(x => x.TenantId != val);
                break;
              case "less":
                And(x => x.TenantId < val);
                break;
              case "lessorequal":
                And(x => x.TenantId <= val);
                break;
              case "greater":
                And(x => x.TenantId > val);
                break;
              case "greaterorequal":
                And(x => x.TenantId >= val);
                break;
              default:
                And(x => x.TenantId == val);
                break;
            }
          }

        }
      }
      return this;
    }
  }
}
