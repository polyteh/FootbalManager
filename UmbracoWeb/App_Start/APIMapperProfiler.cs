using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UmbracoWeb.Models;

namespace UmbracoWeb.App_Start
{
    internal class APIMapperProfiler:Profile
    {
        public APIMapperProfiler()
        {
            CreateMap<TeamViewModel, TeamNameViewModel>().ReverseMap();
        }
    }
}