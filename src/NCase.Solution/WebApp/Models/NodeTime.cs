using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  //案件节点时间设置
  public partial class NodeTime:Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "案件类型", Description = "案件类型")]
    [MaxLength(20)]
    [Required]
    [DefaultValue("")]
    public string Category { get; set; }
    [Display(Name = "节点", Description = "节点")]
    [MaxLength(10)]
    [Required]
    [DefaultValue("")]
    public string Node { get; set; }
    [Display(Name = "天数", Description = "天数")]
    public int Days { get; set; }
  }
}