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
    public class UmbracoHelperTeam : UmbracoHelper< TeamViewModel>
    {
        private readonly IUmbracoHelper<PlayerViewModel> _umbracoPlayerHelper;
        public UmbracoHelperTeam(IContentService contentService, IContentTypeService contentTypeService, IMapper mapper,
             IControllerHelper controllerServices, IUmbracoHelper<PlayerViewModel> umbracoPlayerHelper) : base(contentService, contentTypeService, mapper, controllerServices)
        {
            _umbracoPlayerHelper = umbracoPlayerHelper;
        }

        public override TeamViewModel MapUmbracoContentToModel(IPublishedContent content)
        {
            var teamPlayers = _umbracoPlayerHelper.GetDescendantsContentByAncestorId(content.Id, UmbracoAliasConfiguration.Player.Alias).ToList();
            return new TeamViewModel()
            {
                Id = content.Id,
                Name = content.Value(UmbracoAliasConfiguration.Team.TeamName).ToString(),
                StadiumName = content.Value(UmbracoAliasConfiguration.Team.TeamStadium).ToString(),
                Players = teamPlayers
            };
        }

        //public override TeamViewModel MapUmbracoDescendansContentToModel(IPublishedContent content)
        //{
        //    return new TeamViewModel()
        //    {
        //        Id = content.Id,
        //        Name = content.Value(UmbracoAliasConfiguration.Team.TeamName).ToString(),
        //        StadiumName = content.Value(UmbracoAliasConfiguration.Team.TeamStadium).ToString()
        //    };
        //}

        //public override LeagueViewModel MapUmbracoContentToModel(IPublishedContent content)
        //{
        //    // prepare team list first
        //    var leagueTeamsContent = _controllerService.GetChildrensByAlias(content, UmbracoAliasConfiguration.Team.Alias);
        //    List<TeamViewModel> leagueTeamsList = new List<TeamViewModel>();
        //    foreach (var team in leagueTeamsContent)
        //    {
        //        leagueTeamsList.Add(new TeamViewModel()
        //        {
        //            Id=team.Id, 
        //            Name=team.Value(UmbracoAliasConfiguration.Team.TeamName).ToString(),
        //        }
        //        );
        //    }

        //    return new LeagueViewModel()
        //    {
        //        Id = content.Id,
        //        Name = content.Value(UmbracoAliasConfiguration.League.LeagueName).ToString(),
        //        Teams = leagueTeamsList
        //    };

        //}
    }
}
