using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlHelper2;

namespace WebApp
{
  public class Dashboard
  {
    private static Dashboard _db;
    public static Dashboard Instance
    {
      get
      {
        if (_db == null)
        {
          _db = new Dashboard();
        }
        return _db;

      }
    }
    private IDatabaseAsync db;
    private Dashboard() => this.db = SqlHelper2.DatabaseFactory.CreateDatabase();

    public bool IsToManager() {
      var sql = "select Code from  [dbo].[CodeItems] where codetype='ToManager'";
      var result = db.ExecuteScalar<string>(sql);
      return result=="1"?true:false;
    }
    //总案件数
    public int ALLCount()
    {
      var sql = "select count(1) from [dbo].[LegalCases]";
      return db.ExecuteScalar<int>(sql);
    }
    //分案件数
    public int S0Count()
    {
      var sql = "select count(1) from [dbo].[LegalCases] where Node=N'分案'";
      return db.ExecuteScalar<int>(sql);
    }
    //分案件数
    public int S5Count()
    {
      var sql = "select count(1) from [dbo].[LegalCases] where Node in(N'归档',N'调取' ) or Status=N'结案'";
      return db.ExecuteScalar<int>(sql);
    }
    //立案件数
    public int S1Count()
    {
      var sql = "select count(1) from [dbo].[LegalCases] where Node=N'立案'";
      return db.ExecuteScalar<int>(sql);
    }
    //办案件数
    public int S2Count(string user)
    {
      var sql = $"select count(1) from [dbo].[LegalCases] where Node=N'办案' and Status!=N'结案' and (ToUser=N'{user}' or Manager=N'{user}' or Recorder=N'{user}') ";
      return db.ExecuteScalar<int>(sql);
    }
    //审批件数
    public int S6Count(string user)
    {
      var sql = $"select count(1) from [dbo].[LegalCases] where Node=N'办案' and Status=N'审批' and  Manager=N'{user}'  ";
      return db.ExecuteScalar<int>(sql);
    }
    //归档件数
    public int S3Count()
    {
      var sql = "select count(1) from [dbo].[LegalCases] where Node in(N'归档',N'调取' ) or Status=N'结案'";
      return db.ExecuteScalar<int>(sql);
    }
  }
  }