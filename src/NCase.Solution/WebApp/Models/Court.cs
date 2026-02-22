using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  //法庭
  public partial class Court:Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "法庭名称", Description = "法庭名称")]
    [MaxLength(50)]
    [Required]
    [Index(IsUnique =true)]
    public string Name { get; set; }
    [Display(Name = "所在地", Description = "所在地")]
    [MaxLength(50)]
    public string Zone { get; set; }
    [Display(Name = "地址", Description = "地址")]
    [MaxLength(150)]
    public string Address { get; set; }
    [Display(Name = "联系方式", Description = "联系方式")]
    [MaxLength(150)]
    public string Contect { get; set; }
    [Display(Name = "归属法院", Description = "归属法院")]
    public int CompanyId { get; set; }
    [Display(Name = "法院", Description = "法院")]
    [ForeignKey("CompanyId")]
    public Company Company { get; set; }
  }
}