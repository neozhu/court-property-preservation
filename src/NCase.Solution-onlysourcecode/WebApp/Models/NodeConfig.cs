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
  //节点配置
  public partial class NodeConfig:Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name ="节点",Description = "节点")]
    [MaxLength(20)]
    [Required]
    [Index("IX_NODECONFIG",IsUnique =true,Order =1)]
    public string Node { get; set; }
    [Display(Name = "状态", Description = "状态")]
    [MaxLength(20)]
    [Required]
    [Index("IX_NODECONFIG", IsUnique = true, Order = 2)]
    public string Status { get; set; }
    [Display(Name = "描述", Description = "描述")]
    [MaxLength(200)]
    public string Description { get; set; }

    [Display(Name = "限期(天)", Description = "限期(天)")]
    [DefaultValue(10)]
    public int Expires { get; set; }

    [Display(Name = "授权角色", Description = "授权角色")]
    [MaxLength(20)]
    public string Roles { get; set; }
    [Display(Name = "授权用户", Description = "授权用户")]
    [MaxLength(20)]
    public string Users { get; set; }
    [Display(Name = "下个节点", Description = "下个节点")]
    [MaxLength(20)]
    public string NextNode { get; set; }
    [Display(Name = "下个状态", Description = "下个状态")]
    [MaxLength(20)]
    public string NextStatus { get; set; }



  }
}