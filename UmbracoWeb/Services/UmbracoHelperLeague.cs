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
    public class UmbracoHelperLeague : UmbracoHelper<LeagueViewModel, TeamViewModel>

    {
        public UmbracoHelperLeague(IContentService contentService, IContentTypeService contentTypeService, IMapper mapper,
             IControllerHelper controllerServices) : base(contentService, contentTypeService, mapper, controllerServices)
        {

        }

        public override TeamViewModel MapUmbracoContentToModel(IPublishedContent content)
        {
            return new TeamViewModel()
            {
                Id = content.Id,
                Name = content.Value(UmbracoAliasConfiguration.Team.TeamName).ToString(),
                StadiumName = content.Value(UmbracoAliasConfiguration.Team.TeamStadium).ToString()
            };
        }
    }
}
