using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="CourtMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/3/2020 10:49:58 AM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(CourtMetadata))]
    public partial class Court
    {
    }

    public partial class CourtMetadata
    {
        [Display(Name = "Company",Description ="归属法院",Prompt = "归属法院",ResourceType = typeof(resource.Court))]
        public Company Company { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.Court))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 法庭名称")]
        [Display(Name = "Name",Description ="法庭名称",Prompt = "法庭名称",ResourceType = typeof(resource.Court))]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Zone",Description ="所在地",Prompt = "所在地",ResourceType = typeof(resource.Court))]
        [MaxLength(50)]
        public string Zone { get; set; }

        [Display(Name = "Address",Description ="地址",Prompt = "地址",ResourceType = typeof(resource.Court))]
        [MaxLength(150)]
        public string Address { get; set; }

        [Display(Name = "Contect",Description ="联系方式",Prompt = "联系方式",ResourceType = typeof(resource.Court))]
        [MaxLength(150)]
        public string Contect { get; set; }

        [Required(ErrorMessage = "Please enter : 法院ID")]
        [Display(Name = "CompanyId",Description ="法院ID",Prompt = "法院ID",ResourceType = typeof(resource.Court))]
        public int CompanyId { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.Court))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.Court))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.Court))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.Court))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.Court))]
        public int TenantId { get; set; }

    }

}
