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
    public class UmbracoHelperLeague : UmbracoHelper<LeagueViewModel>

    {
        private readonly IUmbracoHelper<TeamViewModel> _umbracoTeamHelper;
        public UmbracoHelperLeague(IContentService contentService, IContentTypeService contentTypeService, IMapper mapper,
             IControllerHelper controllerServices, IUmbracoHelper<TeamViewModel> umbracoTeamHelper) : base(contentService, contentTypeService, mapper, controllerServices)
        {
            _umbracoTeamHelper = umbracoTeamHelper;
        }

        public override LeagueViewModel MapUmbracoContentToModel(IPublishedContent content)
        {
            var leagueTeams = _umbracoTeamHelper.GetDescendantsContentByAncestorId(content.Id, UmbracoAliasConfiguration.Team.Alias).ToList();
            return new LeagueViewModel()
            {
                Id = content.Id,
                Name = content.Value(UmbracoAliasConfiguration.League.LeagueName).ToString(),
                Teams = leagueTeams
            };

        }
    }
}
