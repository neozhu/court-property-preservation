using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.ViewModel
{
  public class AssigningViewModel
  {
    public int[] SelectedIds { get; set; }
    [Display(Name = "承办部门", Description = "承办部门")]
    [MaxLength(20)]
    public string ToDepartment { get; set; }
    [Display(Name = "承办人", Description = "承办人")]
    [MaxLength(20)]
    public string ToUser { get; set; }
    [Display(Name = "书记员", Description = "书记员")]
    [MaxLength(20)]
    public string Recorder { get; set; }
    [Display(Name = "分案时间", Description = "分案时间")]
    public DateTime AllocateDate { get; set; }
    [Display(Name = "填表人", Description = "填表人")]
    public string Proposer { get; set; }
    [Display(Name = "承办法庭", Description = "承办法庭")]
    public string Court { get; set; }
    [Display(Name = "填表时间", Description = "填表时间")]
    public DateTime ToDoDate { get; set; }
    [Display(Name = "应结案日期", Description = "应结案日期")]
    public DateTime PreCloseDate { get; set; }
    [Display(Name = "立案组意见", Description = "立案组意见")]
    public string Opinion2 { get; set; }
    [Display(Name = "承办人意见", Description = "承办人意见")]
    public string Opinion1 { get; set; }
  }
}