using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Umbraco.Core;
using Umbraco.Core.Composing;
using UmbracoWeb.Models;

namespace UmbracoWeb.App_Start
{
    public class WebApiComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            GlobalConfiguration.Configuration.MapHttpAttributeRoutes();
            GlobalConfiguration.Configuration.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            composition.Register<PlayerViewModel>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new APIMapperProfiler());
            });

            composition.Register(c=>mapperConfig.CreateMapper());
        }
    }
}