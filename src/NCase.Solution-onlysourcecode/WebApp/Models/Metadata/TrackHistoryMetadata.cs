using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="TrackHistoryMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/4/2020 12:00:48 PM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(TrackHistoryMetadata))]
    public partial class TrackHistory
    {
    }

    public partial class TrackHistoryMetadata
    {
        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.TrackHistory))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 档案编号")]
        [Display(Name = "CaseId",Description ="档案编号",Prompt = "档案编号",ResourceType = typeof(resource.TrackHistory))]
        [MaxLength(50)]
        public string CaseId { get; set; }

        [Required(ErrorMessage = "Please enter : 处理状态")]
        [Display(Name = "Status",Description ="处理状态",Prompt = "处理状态",ResourceType = typeof(resource.TrackHistory))]
        [MaxLength(10)]
        public string Status { get; set; }

        [Required(ErrorMessage = "Please enter : 节点")]
        [Display(Name = "Node",Description ="节点",Prompt = "节点",ResourceType = typeof(resource.TrackHistory))]
        [MaxLength(10)]
        public string Node { get; set; }

        [Required(ErrorMessage = "Please enter : 案件状态")]
        [Display(Name = "NodeStatus",Description ="案件状态",Prompt = "案件状态",ResourceType = typeof(resource.TrackHistory))]
        [MaxLength(10)]
        public string NodeStatus { get; set; }

        [Required(ErrorMessage = "Please enter : 处理人")]
        [Display(Name = "Owner",Description ="处理人",Prompt = "处理人",ResourceType = typeof(resource.TrackHistory))]
        [MaxLength(20)]
        public string Owner { get; set; }

        [Display(Name = "ToUser",Description ="指派给",Prompt = "指派给",ResourceType = typeof(resource.TrackHistory))]
        [MaxLength(20)]
        public string ToUser { get; set; }

        [Required(ErrorMessage = "Please enter : 开始时间")]
        [Display(Name = "BeginDate",Description ="开始时间",Prompt = "开始时间",ResourceType = typeof(resource.TrackHistory))]
        public DateTime BeginDate { get; set; }

        [Display(Name = "CompletedDate",Description ="完成时间",Prompt = "完成时间",ResourceType = typeof(resource.TrackHistory))]
        public DateTime CompletedDate { get; set; }

        [Required(ErrorMessage = "Please enter : 限期")]
        [Display(Name = "Expires",Description ="限期",Prompt = "限期",ResourceType = typeof(resource.TrackHistory))]
        public int Expires { get; set; }

        [Display(Name = "DoDate",Description ="届满日期",Prompt = "届满日期",ResourceType = typeof(resource.TrackHistory))]
        public DateTime DoDate { get; set; }

        [Required(ErrorMessage = "Please enter : 实际用时")]
        [Display(Name = "Elapsed",Description ="实际用时",Prompt = "实际用时",ResourceType = typeof(resource.TrackHistory))]
        public int Elapsed { get; set; }

        [Display(Name = "State",Description ="剩余或超时",Prompt = "剩余或超时",ResourceType = typeof(resource.TrackHistory))]
        [MaxLength(50)]
        public string State { get; set; }

        [Display(Name = "Comment",Description ="卷宗",Prompt = "卷宗",ResourceType = typeof(resource.TrackHistory))]
        [MaxLength(50)]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Please enter : 创建时间")]
        [Display(Name = "Created",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.TrackHistory))]
        public DateTime Created { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.TrackHistory))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.TrackHistory))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.TrackHistory))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.TrackHistory))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.TrackHistory))]
        public int TenantId { get; set; }

    }

}
