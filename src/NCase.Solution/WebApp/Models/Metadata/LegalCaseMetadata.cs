using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="LegalCaseMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/4/2020 9:32:08 AM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(LegalCaseMetadata))]
    public partial class LegalCase
    {
    }

    public partial class LegalCaseMetadata
    {
        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.LegalCase))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 档案编号")]
        [Display(Name = "CaseId",Description ="档案编号",Prompt = "档案编号",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(50)]
        public string CaseId { get; set; }

        [Display(Name = "Project",Description ="案件名称",Prompt = "案件名称",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(200)]
        public string Project { get; set; }

        [Required(ErrorMessage = "Please enter : 案件类型")]
        [Display(Name = "Category",Description ="案件类型",Prompt = "案件类型",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(20)]
        public string Category { get; set; }

        [Required(ErrorMessage = "Please enter : 状态")]
        [Display(Name = "Status",Description ="状态",Prompt = "状态",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(10)]
        public string Status { get; set; }

        [Display(Name = "Node",Description ="节点",Prompt = "节点",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(10)]
        public string Node { get; set; }

        [Required(ErrorMessage = "Please enter : 限期")]
        [Display(Name = "Expires",Description ="限期",Prompt = "限期",ResourceType = typeof(resource.LegalCase))]
        public int Expires { get; set; }

        [Display(Name = "Cause",Description ="案件原由",Prompt = "案件原由",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(50)]
        public string Cause { get; set; }

        [Display(Name = "Feature",Description ="案件特征",Prompt = "案件特征",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(200)]
        public string Feature { get; set; }

        [Display(Name = "BasedOn",Description ="执行依据文号",Prompt = "执行依据文号",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(100)]
        public string BasedOn { get; set; }

        [Display(Name = "Subject",Description ="执行主体",Prompt = "执行主体",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(50)]
        public string Subject { get; set; }

        [Display(Name = "FromDepartment",Description ="立案部门",Prompt = "立案部门",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(20)]
        public string FromDepartment { get; set; }

        [Display(Name = "ToDepartment",Description ="承办部门",Prompt = "承办部门",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(20)]
        public string ToDepartment { get; set; }

        [Display(Name = "ToUser",Description ="承办人",Prompt = "承办人",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(20)]
        public string ToUser { get; set; }

        [Display(Name = "Recorder",Description ="书记员",Prompt = "书记员",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(20)]
        public string Recorder { get; set; }

        [Display(Name = "Examiner",Description ="立案审查人",Prompt = "立案审查人",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(20)]
        public string Examiner { get; set; }

        [Display(Name = "OriginCaseId",Description ="原案（执行）案号",Prompt = "原案（执行）案号",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(50)]
        public string OriginCaseId { get; set; }

        [Display(Name = "ReceiveDate",Description ="收案日期",Prompt = "收案日期",ResourceType = typeof(resource.LegalCase))]
        public DateTime ReceiveDate { get; set; }

        [Required(ErrorMessage = "Please enter : 立案日期")]
        [Display(Name = "RegisterDate",Description ="立案日期",Prompt = "立案日期",ResourceType = typeof(resource.LegalCase))]
        public DateTime RegisterDate { get; set; }

        [Required(ErrorMessage = "Please enter : 申请执行标的")]
        [Display(Name = "PreUnderlyingAsset",Description ="申请执行标的",Prompt = "申请执行标的",ResourceType = typeof(resource.LegalCase))]
        public decimal PreUnderlyingAsset { get; set; }

        [Display(Name = "AllocateDate",Description ="分案时间",Prompt = "分案时间",ResourceType = typeof(resource.LegalCase))]
        public DateTime AllocateDate { get; set; }

        [Display(Name = "PreCloseDate",Description ="应结案日期",Prompt = "应结案日期",ResourceType = typeof(resource.LegalCase))]
        public DateTime PreCloseDate { get; set; }

        [Display(Name = "ClosedDate",Description ="结案日期",Prompt = "结案日期",ResourceType = typeof(resource.LegalCase))]
        public DateTime ClosedDate { get; set; }

        [Display(Name = "CloseType",Description ="结案方式",Prompt = "结案方式",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(50)]
        public string CloseType { get; set; }

        [Required(ErrorMessage = "Please enter : 结案标的")]
        [Display(Name = "UnderlyingAsset",Description ="结案标的",Prompt = "结案标的",ResourceType = typeof(resource.LegalCase))]
        public decimal UnderlyingAsset { get; set; }

        [Display(Name = "Court",Description ="所属法庭",Prompt = "所属法庭",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(50)]
        public string Court { get; set; }

        [Display(Name = "Org",Description ="所属法院",Prompt = "所属法院",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(50)]
        public string Org { get; set; }

        [Display(Name = "Accuser",Description ="申请保全人",Prompt = "申请保全人",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(250)]
        public string Accuser { get; set; }

        [Display(Name = "AccuserAddress",Description ="申请保全人地址",Prompt = "申请保全人地址",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(250)]
        public string AccuserAddress { get; set; }

        [Display(Name = "Defendant",Description ="被保全人",Prompt = "被保全人",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(250)]
        public string Defendant { get; set; }

        [Display(Name = "DefendantAddress",Description ="被保全人地址",Prompt = "被保全人地址",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(250)]
        public string DefendantAddress { get; set; }

        [Required(ErrorMessage = "Please enter : 应收执行费")]
        [Display(Name = "Receivable",Description ="应收执行费",Prompt = "应收执行费",ResourceType = typeof(resource.LegalCase))]
        public decimal Receivable { get; set; }

        [Required(ErrorMessage = "Please enter : 已收执行费")]
        [Display(Name = "Received",Description ="已收执行费",Prompt = "已收执行费",ResourceType = typeof(resource.LegalCase))]
        public decimal Received { get; set; }

        [Display(Name = "Opinion1",Description ="承办人意见",Prompt = "承办人意见",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(50)]
        public string Opinion1 { get; set; }

        [Display(Name = "Opinion2",Description ="立案组意见",Prompt = "立案组意见",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(50)]
        public string Opinion2 { get; set; }

        [Display(Name = "Opinion3",Description ="院长批示",Prompt = "院长批示",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(50)]
        public string Opinion3 { get; set; }

        [Display(Name = "Comment",Description ="备注",Prompt = "备注",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(50)]
        public string Comment { get; set; }

        [Display(Name = "Proposer",Description ="填表人",Prompt = "填表人",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(20)]
        public string Proposer { get; set; }

        [Required(ErrorMessage = "Please enter : 填表日期")]
        [Display(Name = "FilingDate",Description ="填表日期",Prompt = "填表日期",ResourceType = typeof(resource.LegalCase))]
        public DateTime FilingDate { get; set; }

        [Required(ErrorMessage = "Please enter : 节点开始时间")]
        [Display(Name = "ToDoDate",Description ="节点开始时间",Prompt = "节点开始时间",ResourceType = typeof(resource.LegalCase))]
        public DateTime ToDoDate { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.LegalCase))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.LegalCase))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.LegalCase))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.LegalCase))]
        public int TenantId { get; set; }

    }

}
