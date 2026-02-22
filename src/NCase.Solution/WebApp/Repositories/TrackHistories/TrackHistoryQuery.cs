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

namespace WebApp.Repositories
{
/// <summary>
/// File: TrackHistoryQuery.cs
/// Purpose: easyui datagrid filter query 
/// Created Date: 3/4/2020 12:00:39 PM
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 All Rights Reserved
/// </summary>
   public class TrackHistoryQuery:QueryObject<TrackHistory>
   {
		public TrackHistoryQuery Withfilter(IEnumerable<filterRule> filters)
        {
           if (filters != null)
           {
               foreach (var rule in filters)
               {
						if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
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
                            case "greaterorequal" :
                                And(x => x.Id >= val);
                                break;
                            default:
                                And(x => x.Id == val);
                                break;
                        }
						}
						if (rule.field == "CaseId"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.CaseId.Contains(rule.value));
						}
						if (rule.field == "Status"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Status.Contains(rule.value));
						}
						if (rule.field == "Node"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Node.Contains(rule.value));
						}
						if (rule.field == "NodeStatus"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.NodeStatus.Contains(rule.value));
						}
						if (rule.field == "Owner"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Owner.Contains(rule.value));
						}
						if (rule.field == "ToUser"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.ToUser.Contains(rule.value));
						}
						if (rule.field == "BeginDate" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.BeginDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.BeginDate) <= 0);
						    }
						}
						if (rule.field == "CompletedDate" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.CompletedDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.CompletedDate) <= 0);
						    }
						}
						if (rule.field == "Expires" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
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
                            case "greaterorequal" :
                                And(x => x.Expires >= val);
                                break;
                            default:
                                And(x => x.Expires == val);
                                break;
                        }
						}
						if (rule.field == "DoDate" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.DoDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.DoDate) <= 0);
						    }
						}
						if (rule.field == "Elapsed" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Elapsed == val);
                                break;
                            case "notequal":
                                And(x => x.Elapsed != val);
                                break;
                            case "less":
                                And(x => x.Elapsed < val);
                                break;
                            case "lessorequal":
                                And(x => x.Elapsed <= val);
                                break;
                            case "greater":
                                And(x => x.Elapsed > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Elapsed >= val);
                                break;
                            default:
                                And(x => x.Elapsed == val);
                                break;
                        }
						}
						if (rule.field == "State"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.State.Contains(rule.value));
						}
						if (rule.field == "Comment"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Comment.Contains(rule.value));
						}
						if (rule.field == "Created" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.Created) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.Created) <= 0);
						    }
						}
						if (rule.field == "CreatedDate" && !string.IsNullOrEmpty(rule.value) )
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
						if (rule.field == "CreatedBy"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.CreatedBy.Contains(rule.value));
						}
						if (rule.field == "LastModifiedDate" && !string.IsNullOrEmpty(rule.value) )
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
						if (rule.field == "LastModifiedBy"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.LastModifiedBy.Contains(rule.value));
						}
						if (rule.field == "TenantId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
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
                            case "greaterorequal" :
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
