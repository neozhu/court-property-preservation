using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="AttachmentMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/3/2020 1:33:25 PM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(AttachmentMetadata))]
    public partial class Attachment
    {
    }

    public partial class AttachmentMetadata
    {
        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.Attachment))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 案件号")]
        [Display(Name = "CaseId",Description ="案件号",Prompt = "案件号",ResourceType = typeof(resource.Attachment))]
        [MaxLength(50)]
        public string CaseId { get; set; }

        [Display(Name = "Description",Description ="描述",Prompt = "描述",ResourceType = typeof(resource.Attachment))]
        [MaxLength(50)]
        public string Description { get; set; }

        [Display(Name = "DocId",Description ="文件名称",Prompt = "文件名称",ResourceType = typeof(resource.Attachment))]
        [MaxLength(500)]
        public string DocId { get; set; }

        [Display(Name = "Type",Description ="附件类型",Prompt = "附件类型",ResourceType = typeof(resource.Attachment))]
        [MaxLength(50)]
        public string Type { get; set; }

        [Required(ErrorMessage = "Please enter : 保存路径")]
        [Display(Name = "Path",Description ="保存路径",Prompt = "保存路径",ResourceType = typeof(resource.Attachment))]
        [MaxLength(50)]
        public string Path { get; set; }

        [Display(Name = "Ext",Description ="文件类型",Prompt = "文件类型",ResourceType = typeof(resource.Attachment))]
        [MaxLength(5)]
        public string Ext { get; set; }

        [Display(Name = "ExpireDate",Description ="过期日",Prompt = "过期日",ResourceType = typeof(resource.Attachment))]
        public DateTime ExpireDate { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.Attachment))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.Attachment))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.Attachment))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.Attachment))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.Attachment))]
        public int TenantId { get; set; }

    }

}
