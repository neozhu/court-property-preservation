using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  public partial class TempCaseId:Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "案号", Description = "档案编号")]
    [MaxLength(50)]
    public string CaseId { get; set; }
    [Display(Name = "案件类型", Description = "案件类型")]
    [MaxLength(20)]
    public string Category { get; set; }
    [Display(Name = "字号", Description = "字号")]
    [MaxLength(5)]
    public string SerialNumber { get; set; }
    [Display(Name = "标记", Description = "标记")]
    public int Flag { get; set; }
    [MaxLength(10)]
    public string Year { get; set; }
    public int Expires { get; set; }
  }
}