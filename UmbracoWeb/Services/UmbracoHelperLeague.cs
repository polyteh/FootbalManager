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
    public class UmbracoHelperLeague : UmbracoHelper<TeamViewModel>

    {
        public UmbracoHelperLeague(IContentService contentService, IContentTypeService contentTypeService, IMapper mapper,
             IControllerHelper controllerServices) : base(contentService, contentTypeService, mapper, controllerServices)
        {

        }

        //public override PlayerViewModel MapUmbracoDescendansContentToModel(IPublishedContent content)
        //{
        //    return new PlayerViewModel()
        //    {
        //        Id = content.Id,
        //        Name = content.Value(UmbracoAliasConfiguration.Player.PlayerName).ToString(),
        //        Age = Int32.Parse(content.Value(UmbracoAliasConfiguration.Player.PlayerAge).ToString())
        //    };
        //}

        public override TeamViewModel MapUmbracoContentToModel(IPublishedContent content)
        {
            // prepare player list first
            //var teamPlayersContent = _controllerService.GetChildrensByAlias(content, UmbracoAliasConfiguration.Player.Alias);
            //List<PlayerViewModel> teamPlayersList = new List<PlayerViewModel>();
            //foreach (var team in teamPlayersContent)
            //{
            //    teamPlayersList.Add(new TeamViewModel()
            //    {
            //        Id = team.Id,
            //        Name = team.Value(UmbracoAliasConfiguration.Team.TeamName).ToString(),
            //    }
            //    );
            //}

            //return new LeagueViewModel()
            //{
            //    Id = content.Id,
            //    Name = content.Value(UmbracoAliasConfiguration.League.LeagueName).ToString(),
            //    Teams = teamPlayersList
            //};
            return null;

        }
    }
}
