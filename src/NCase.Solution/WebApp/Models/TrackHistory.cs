using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
  /// <summary>
  /// 案件跟踪历史状态
  /// </summary>
  public partial class TrackHistory:Entity
  {
    [Key]
    public int Id { get; set; }
    [Display(Name = "案号", Description = "档案编号")]
    [DefaultValue("（2020）粤")]
    [MaxLength(50)]
    [Required]
    public string CaseId { get; set; }
    [Display(Name = "案件状态", Description = "案件状态")]
    [MaxLength(10)]
    [DefaultValue("立案")]
    [Required]
    public string Status { get; set; }
    [Display(Name = "节点", Description = "节点")]
    [MaxLength(10)]
    [DefaultValue("立案")]
    [Required]
    public string Node { get; set; }

    [Display(Name = "节点状态", Description = "节点状态")]
    [MaxLength(10)]
    [DefaultValue("待处理")]
    [Required]
    public string NodeStatus { get; set; }

    [Display(Name = "处理人", Description = "处理人")]
    [MaxLength(20)]
    //[Required]
    public string Owner { get; set; }
    [Display(Name = "承办人", Description = "承办人")]
    [MaxLength(20)]
    public string ToUser { get; set; }
    [Display(Name = "书记员", Description = "书记员")]
    [MaxLength(20)]
    public string Recorder { get; set; }
    [Display(Name = "开始时间", Description = "开始时间")]
    [DefaultValue("now")]
    public DateTime BeginDate { get; set; }
    [Display(Name = "完成时间", Description = "完成时间")]
    [DefaultValue(null)]
    public DateTime? CompletedDate { get; set; }

    [Display(Name = "限期", Description = "限期")]
    [DefaultValue(10)]
    public int Expires { get; set; }
    [Display(Name = "届满日期", Description = "届满日期")]
    public DateTime? DoDate { get; set; }
    [Display(Name = "剩余天数", Description = "剩余天数")]
    [DefaultValue(10)]
    public int Elapsed { get; set; }
    [Display(Name = "剩余或超时", Description = "剩余或超时")]
    [DefaultValue(10)]
    [MaxLength(50)]
    public string State { get; set; }
    [Display(Name = "卷宗", Description = "卷宗")]
    [MaxLength(50)]
    public string Comment { get; set; }
    [Display(Name = "创建时间", Description = "创建时间")]
    [DefaultValue("now")]
    public DateTime Created { get; set; }
  }
}