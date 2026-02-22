using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  //案件档案
  public partial class LegalCase : Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "案号", Description = "档案编号")]
    [DefaultValue("（2020）粤")]
    [MaxLength(128)]
    [Index(IsUnique =true)]
    [Required]
    public string CaseId { get; set; }
    [Display(Name = "案件名称", Description = "案件名称")]
    [DefaultValue("")]
    [MaxLength(256)]
    public string Project { get; set; }
      
    [Display(Name = "案件类型", Description = "案件类型")]
    [MaxLength(20)]
    [Required]
    public string Category { get; set; }
    [Display(Name = "状态", Description = "状态")]
    [MaxLength(10)]
    [DefaultValue("立案")]
    [Required]
    public string Status { get; set; }
    [Display(Name = "节点", Description = "节点")]
    [MaxLength(10)]
    [DefaultValue("立案")]
    public string Node { get; set; }

    [Display(Name = "限期", Description = "限期")]
    [DefaultValue(10)]
    public int Expires { get; set; }
    [Display(Name = "案由", Description = "案件原由")]
    //[MaxLength(MAX)]
    public string Cause { get; set; }
    [Display(Name = "案件特征", Description = "案件特征")]
    [MaxLength(256)]
    public string Feature { get; set; }
    [Display(Name = "执行依据文号", Description = "执行依据文号")]
    //[MaxLength(100)]
    public string BasedOn {get;set;}
    [Display(Name = "执行主体", Description = "执行主体")]
    //[MaxLength(100)]
    public string Subject { get; set; }
    [Display(Name = "立案部门", Description = "立案部门")]
    [MaxLength(20)]
    public string FromDepartment { get; set; }
    [Display(Name = "承办部门", Description = "承办部门")]
    [MaxLength(20)]
    public string ToDepartment { get; set; }

    [Display(Name = "承办人", Description = "承办人")]
    [MaxLength(20)]
    public string ToUser { get; set; }
    [Display(Name = "书记员", Description = "书记员")]
    [MaxLength(20)]
    public string Recorder { get; set; }
    [Display(Name = "立案审查人", Description = "立案审查人")]
    [MaxLength(20)]
    public string Examiner { get; set; }
    [Display(Name = "原案（执行）案号", Description = "原案（执行）案号")]
    [MaxLength(512)]
    public string OriginCaseId { get; set; }

    [Display(Name = "收案日期", Description = "收案日期")]
    [DefaultValue(null)]
    public DateTime? ReceiveDate { get; set; }

    [Display(Name = "立案日期", Description = "立案日期")]
    [DefaultValue("now")]
    public DateTime RegisterDate { get; set; }
    [Display(Name = "申请执行标的", Description = "申请执行标的")]
    public decimal PreUnderlyingAsset { get; set; }
    [Display(Name = "分案时间", Description = "分案时间")]
    [DefaultValue(null)]
    public DateTime? AllocateDate { get; set; }

    [Display(Name = "应结案日期", Description = "应结案日期")]
    [DefaultValue(null)]
    public DateTime? PreCloseDate { get; set; }

    [Display(Name = "结案日期", Description = "结案日期")]
    [DefaultValue(null)]
    public DateTime? ClosedDate { get; set; }
    [Display(Name = "结案方式", Description = "结案方式")]
    [DefaultValue(null)]
    [MaxLength(50)]
    public string CloseType { get; set; }

    [Display(Name = "结案标的", Description = "结案标的")]
    public decimal UnderlyingAsset { get; set; }

    [Display(Name = "承办法庭", Description = "承办法庭")]
    [MaxLength(50)]
    public string Court { get; set; }
    [Display(Name = "所属法院", Description = "所属法院")]
    [MaxLength(50)]
    public string Org { get; set; }

    #region 申请表信息
    [Display(Name = "申请保全人", Description = "申请保全人")]
    //[MaxLength(250)]
    public string Accuser { get; set; }
    [Display(Name = "申请保全人地址", Description = "申请保全人地址")]
    //[MaxLength(250)]
    public string AccuserAddress { get; set; }

    [Display(Name = "被保全人", Description = "被保全人")]
   // [MaxLength(250)]
    public string Defendant { get; set; }
    [Display(Name = "被保全人地址", Description = "被保全人地址")]
    //[MaxLength(250)]
    public string DefendantAddress { get; set; }
    [Display(Name = "应收执行费", Description = "应收执行费")]
    public decimal Receivable { get; set; }
    [Display(Name = "已收执行费", Description = "已收执行费")]
    public decimal Received { get; set; }

    [Display(Name = "承办人意见", Description = "承办人意见")]
    public string Opinion1 { get; set; }

    [Display(Name = "立案组意见", Description = "立案组意见")]
    public string Opinion2 { get; set; }
    [Display(Name = "部门领导批示", Description = "部门领导批示")]
    public string Opinion3 { get; set; }
    [Display(Name = "备注", Description = "备注")]
    public string Comment { get; set; }
    [Display(Name = "填表人", Description = "填表人")]
    [DefaultValue("user")]
    [MaxLength(20)]
    public string Proposer { get; set; }
    [Display(Name = "填表日期", Description = "填表日期")]
    [DefaultValue("now")]
    public DateTime FilingDate { get; set; }
    [Display(Name = "节点开始时间", Description = "节点开始时间")]
    [DefaultValue("now")]
    public DateTime ToDoDate { get; set; }
    [Display(Name = "字号", Description = "字号")]
    [MaxLength(5)]
    public string SerialNumber { get; set; }
    [Display(Name = "需要部门领导审批", Description = "需要部门领导审批")]
    [DefaultValue(true)]
    public bool ToManager { get; set; }
    [Display(Name = "部门领导", Description = "部门领导")]
    [MaxLength(20)]
    public string Manager { get; set; }
    #endregion


    #region 预留字段
    #endregion

  }
}