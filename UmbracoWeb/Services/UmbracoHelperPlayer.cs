using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;
using UmbracoWeb.Configuration;
using UmbracoWeb.Interfaces;
using UmbracoWeb.Models;

namespace UmbracoWeb.Services
{
    public class UmbracoHelperPlayer : UmbracoHelper<PlayerViewModel>

    {
        public UmbracoHelperPlayer(IContentService contentService, IContentTypeService contentTypeService, IMapper mapper,
             IControllerHelper controllerServices) : base(contentService, contentTypeService, mapper, controllerServices)
        {

        }

        public override PlayerViewModel MapUmbracoContentToModel(IPublishedContent content)
        {
            return new PlayerViewModel()
            {
                Id = content.Id,
                Name = content.Value(UmbracoAliasConfiguration.Player.PlayerName).ToString(),
                Age = Int32.Parse(content.Value(UmbracoAliasConfiguration.Player.PlayerAge).ToString())

            };
        }


    }
}
