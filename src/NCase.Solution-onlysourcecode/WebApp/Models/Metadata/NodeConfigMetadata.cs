using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApp.Models
{
// <copyright file="NodeConfigMetadata.cs" tool="martCode MVC5 Scaffolder">
// Copyright (c) 2020 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>3/3/2020 11:26:35 AM </date>
// <summary>Class representing a Metadata entity </summary>
    //[MetadataType(typeof(NodeConfigMetadata))]
    public partial class NodeConfig
    {
    }

    public partial class NodeConfigMetadata
    {
        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id",Description ="Id",Prompt = "Id",ResourceType = typeof(resource.NodeConfig))]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 节点状态")]
        [Display(Name = "Node",Description ="节点状态",Prompt = "节点状态",ResourceType = typeof(resource.NodeConfig))]
        [MaxLength(20)]
        public string Node { get; set; }

        [Display(Name = "Description",Description ="描述",Prompt = "描述",ResourceType = typeof(resource.NodeConfig))]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter : 限期(天)")]
        [Display(Name = "Expires",Description ="限期(天)",Prompt = "限期(天)",ResourceType = typeof(resource.NodeConfig))]
        public int Expires { get; set; }

        [Display(Name = "Roles",Description ="授权角色",Prompt = "授权角色",ResourceType = typeof(resource.NodeConfig))]
        [MaxLength(20)]
        public string Roles { get; set; }

        [Display(Name = "Users",Description ="授权用户",Prompt = "授权用户",ResourceType = typeof(resource.NodeConfig))]
        [MaxLength(20)]
        public string Users { get; set; }

        [Display(Name = "NextNode",Description ="下个节点",Prompt = "下个节点",ResourceType = typeof(resource.NodeConfig))]
        [MaxLength(20)]
        public string NextNode { get; set; }

        [Display(Name = "CreatedDate",Description ="创建时间",Prompt = "创建时间",ResourceType = typeof(resource.NodeConfig))]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy",Description ="创建用户",Prompt = "创建用户",ResourceType = typeof(resource.NodeConfig))]
        [MaxLength(20)]
        public string CreatedBy { get; set; }

        [Display(Name = "LastModifiedDate",Description ="最后更新时间",Prompt = "最后更新时间",ResourceType = typeof(resource.NodeConfig))]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "LastModifiedBy",Description ="最后更新用户",Prompt = "最后更新用户",ResourceType = typeof(resource.NodeConfig))]
        [MaxLength(20)]
        public string LastModifiedBy { get; set; }

        [Required(ErrorMessage = "Please enter : Tenant Id")]
        [Display(Name = "TenantId",Description ="Tenant Id",Prompt = "Tenant Id",ResourceType = typeof(resource.NodeConfig))]
        public int TenantId { get; set; }

    }

}
