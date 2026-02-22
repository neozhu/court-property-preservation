using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="TempCaseIdMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>2020/4/17 11:43:27 </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(TempCaseIdMetadata))]
    public partial class TempCaseId
    {
    }

    public partial class TempCaseIdMetadata
    {
        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.TempCaseId))]
        public int Id { get; set; }

        [Display(Name = "CaseId",Description ="档案编号",Prompt = "档案编号",ResourceType = typeof(resource.TempCaseId))]
        [MaxLength(50)]
        public string CaseId { get; set; }

        [Display(Name = "Category",Description ="案件类型",Prompt = "案件类型",ResourceType = typeof(resource.TempCaseId))]
        [MaxLength(20)]
        public string Category { get; set; }

        [Display(Name = "SerialNumber",Description ="字号",Prompt = "字号",ResourceType = typeof(resource.TempCaseId))]
        [MaxLength(5)]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "Please enter : 标记")]
        [Display(Name = "Flag",Description ="标记",Prompt = "标记",ResourceType = typeof(resource.TempCaseId))]
        public int Flag { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.TempCaseId))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.TempCaseId))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.TempCaseId))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.TempCaseId))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.TempCaseId))]
        public int TenantId { get; set; }

    }

}
