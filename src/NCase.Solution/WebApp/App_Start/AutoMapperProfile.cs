using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using WebApp.Models;

namespace WebApp
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {

      _ = this.CreateMap<LegalCase, TrackHistory>()
        .ForMember(x => x.Status, opt => opt.MapFrom(x => x.Status))
        .ForMember(x => x.Owner, opt => opt.MapFrom(x => x.Proposer))
        .ForMember(x => x.BeginDate, opt => opt.MapFrom(x => x.RegisterDate))
        .ForMember(x=>x.NodeStatus,opt=>opt.MapFrom(x=>"待处理"))
        .ForMember(x => x.Created, opt => opt.MapFrom(x => DateTime.Now));



    }
  }
   
}