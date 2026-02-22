using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  public partial class Attachment : Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "案件号", Description = "案件号")]
    [MaxLength(50)]
    [Required]
    public string CaseId { get; set; }
    [Display(Name = "描述", Description = "描述")]
    //[MaxLength(150)]
    public string Description { get; set; }
    [Display(Name = "文件名称", Description = "文件名称")]
    [MaxLength(500)]
    public string DocId { get; set; }
    [Display(Name = "附件类型", Description = "附件类型")]
    [MaxLength(50)]
    [DefaultValue("")]
    public string Type { get; set; }
    
    [Display(Name = "保存路径", Description = "保存路径")]
    [DefaultValue("")]
    [Required]
    public string Path { get; set; }
    [Display(Name = "文件类型", Description = "文件类型")]
    [MaxLength(5)]
    [DefaultValue(".")]
    public string Ext { get; set; }
    [Display(Name = "过期日", Description = "过期日")]
    [DefaultValue(null)]
    public DateTime? ExpireDate{get;set;}


    





  }
}