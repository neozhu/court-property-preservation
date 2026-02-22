using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Repositories;
using System.Threading.Tasks;
using Service.Pattern;
using WebApp.Models;
using WebApp.Repositories;
using System.Data;
using System.IO;
using WebApp.Models.ViewModel;

namespace WebApp.Services
{
/// <summary>
/// File: ILegalCaseService.cs
/// Purpose: Service interfaces. Services expose a service interface
/// to which all inbound messages are sent. You can think of a service interface
/// as a façade that exposes the business logic implemented in the application
/// Created Date: 3/4/2020 9:28:07 AM
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 All Rights Reserved
/// </summary>
    public interface ILegalCaseService:IService<LegalCase>
    {
 
		Task ImportDataTableAsync(DataTable datatable,string username="");
		Task<Stream> ExportExcelAsync( string filterRules = "",string sort = "Id", string order = "asc");
	    void Delete(int[] id);
    Task<LegalCase> InitItemAsync(string name);
    Task Register(LegalCase item);

    Task DoComplete(int id,string user);
    Task<string> ExportWord(int id,string templatefile, string destpath);

    Task Assigning(AssigningViewModel assigning,string user);
    Task ChangeAssigning(AssigningViewModel assigning, string user);
    Task AssigningTask(LegalCase item,string user);

    Task BackStep1(int id, string user);

    Task<Tuple<string, int,int>> GetCaseId(string year, string serial, string category);
    Task<Tuple<int, int>> GetCaseExpires(string year, string serial, string category);
    Task UploadFile(int caseid, string filename, string ext, string path);
    Task DeleteFile(int caseid, string filename);
    //案件执行
    Task ExecutionTask(LegalCase legalcase,string user);

    Task ManagerTask(LegalCase legalcase,string username);
    Task CloseTask(LegalCase legalcase,string username);

    Task GoBack(int id,string user);
    Task ExtractionTask(int id, string user);
    Task ArchiveTask(int id, string user);

    Task ToNext(int[] id, string user);

    Task<bool> ValidateCaseId(string caseid);
  }
}