using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="NodeTimeMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/19/2020 4:51:14 PM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(NodeTimeMetadata))]
    public partial class NodeTime
    {
    }

    public partial class NodeTimeMetadata
    {
        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.NodeTime))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 案件类型")]
        [Display(Name = "Category",Description ="案件类型",Prompt = "案件类型",ResourceType = typeof(resource.NodeTime))]
        [MaxLength(20)]
        public string Category { get; set; }

        [Required(ErrorMessage = "Please enter : 节点")]
        [Display(Name = "Node",Description ="节点",Prompt = "节点",ResourceType = typeof(resource.NodeTime))]
        [MaxLength(10)]
        public string Node { get; set; }

        [Required(ErrorMessage = "Please enter : 天数")]
        [Display(Name = "Days",Description ="天数",Prompt = "天数",ResourceType = typeof(resource.NodeTime))]
        public int Days { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.NodeTime))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.NodeTime))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.NodeTime))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.NodeTime))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.NodeTime))]
        public int TenantId { get; set; }

    }

}
