using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using UmbracoWeb.Configuration;
using UmbracoWeb.Interfaces;
using UmbracoWeb.Models;

namespace UmbracoWeb.Controllers
{
    [RoutePrefix("api/footballmanager/team")]
    public class TeamApiController : UmbracoApiController
    {
        private readonly IContentService _contentService;
        private readonly IContentTypeService _contentTypeService;
        private readonly IMapper _mapper;
        private readonly IControllerHelper _controllerService;
        public TeamApiController(IContentService contentService, IContentTypeService contentTypeService, IMapper mapper,
             IControllerHelper controllerServices)
        {
            _contentService = contentService;
            _contentTypeService = contentTypeService;
            _mapper = mapper;
            _controllerService = controllerServices;
        }

        [HttpGet]
        [Route("test/{p}")]
        public string Test(int p)
        {
            int k = p + 1;
            return k.ToString();
        }



        /// <summary>
        /// Add new player
        /// </summary>
        /// <param name="newPlayer"></param>
        /// <returns>Player model, which was created</returns>
        [HttpPost]
        [Route("AddNewPlayerToReal/")]
        
        public PlayerViewModel AddNewPlayerToReal(PlayerViewModel newPlayer)
        {
            int nodeID = 2080;

            //get node info to add in
            var playersNodeName = Umbraco.Content(nodeID).Name;
            var playersNodeGuid = Umbraco.Content(nodeID).Key;
            GuidUdi currentPageUdi = new GuidUdi(playersNodeName, playersNodeGuid);

            //create content node
            var data = _contentService.CreateContent(playersNodeName, currentPageUdi, UmbracoAliasConfiguration.Player.Alias, 0);

            //define properties
            data.Name = newPlayer.Name;
            data.SetValue(UmbracoAliasConfiguration.Player.PlayerName, newPlayer.Name);
            data.SetValue(UmbracoAliasConfiguration.Player.PlayerAge, newPlayer.Age);


            _contentService.SaveAndPublish(data);

            var playersToSort = Umbraco.Content(nodeID).Children().ToList();
            var playersIDToSort = playersToSort.OrderByDescending(x => x.CreateDate).Select(y => y.Id).ToList();
            _contentService.Sort(playersIDToSort);

            return newPlayer;
        }

        /// <summary>
        /// Get all teams from DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Teams/")]    
        public IEnumerable<TeamViewModel> GetAllTeams()
        {
            // find all root elements of Team document type
            var teamsRootId = _controllerService.GetAllRootsId(UmbracoAliasConfiguration.Team.Alias);

            if (teamsRootId==null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            List<IPublishedContent> teamListContent = new List<IPublishedContent>();

            foreach (var teamRootItem in teamsRootId)
            {
                var teamContent = Umbraco.Content(teamRootItem);
                teamListContent.AddRange(_controllerService.GetChildrensByAlias(teamContent, UmbracoAliasConfiguration.Team.Alias));
            }

            //map data from IPublishedContent to the 
            List<TeamViewModel> allTeams = new List<TeamViewModel>();
            foreach (var team in teamListContent)
            {
                allTeams.Add(new TeamViewModel()
                {
                    Id=team.Id,
                    Name = team.Value(UmbracoAliasConfiguration.Team.TeamName).ToString(),
                    StadiumName = team.Value(UmbracoAliasConfiguration.Team.TeamStadium).ToString(),
                    Players = GetTeamPlayers(team.Id).ToList()

                });
            }
            return allTeams;
        }

        /// <summary>
        /// get team names for menu
        /// </summary>
        /// <returns>list of team names</returns>
        [HttpGet]
        [Route("TeamNames/")]     
        public IEnumerable<TeamNameViewModel> GetAllTeamNames()
        {
            IEnumerable<TeamViewModel> fullTeamList = GetAllTeams();
            var teamlist = _mapper.Map<IEnumerable<TeamNameViewModel>>(fullTeamList);
            return teamlist;
        }


        /// <summary>
        /// Get all players from the specific team
        /// </summary>
        /// <param name="nodeId"></param>
        /// <exception cref="HttpResponseException">Not Found</exception>
        /// <returns>List of team players</returns>
        [HttpGet]
        [Route("GetTeamPlayers/{nodeId}")]
       
        public IEnumerable<PlayerViewModel> GetTeamPlayers(int nodeId)
        {
            //int nodeID = 2068; //Barcelona content

            if (!(_controllerService.IsNodeIdCorrect(nodeId)))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var teamPlayersContentById = Umbraco.Content(nodeId);
            if (!(_controllerService.IsContentNotNull(teamPlayersContentById)))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if (teamPlayersContentById.Descendants().Where(x => x.IsDocumentType(UmbracoAliasConfiguration.Player.Alias)).FirstOrDefault()==null) 
            {
                return new List<PlayerViewModel>();
            }

            var teamPlayersContent = teamPlayersContentById.Descendants().Where(x => x.IsDocumentType(UmbracoAliasConfiguration.Player.Alias));


            if (teamPlayersContent == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            List<PlayerViewModel> teamPlayersList = new List<PlayerViewModel>();
            foreach (var player in teamPlayersContent)
            {
                teamPlayersList.Add(new PlayerViewModel()
                {
                    Id=player.Id,
                    Name = player.Value(UmbracoAliasConfiguration.Player.PlayerName).ToString(),
                    Age = Int32.Parse(player.Value(UmbracoAliasConfiguration.Player.PlayerAge).ToString())
                });
            }
            return teamPlayersList;
        }

        /// <summary>
        /// Get team all info by Id
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns>team view model</returns>
        [HttpGet]
        [Route("GetTeamById/{nodeId}")]      
        public TeamViewModel GetTeamById(int nodeId)
        {
            //int nodeID=2072; //ManCity content
            //int nodeID = 2068; //Barcelona content
            if (!(_controllerService.IsNodeIdCorrect(nodeId)))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var teamContent = Umbraco.Content(nodeId);
            if (!(_controllerService.IsContentNotNull(teamContent)))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            TeamViewModel teamModel = new TeamViewModel()
            {
                Id = nodeId,
                Name = teamContent.Value(UmbracoAliasConfiguration.Team.TeamName).ToString(),
                StadiumName = teamContent.Value(UmbracoAliasConfiguration.Team.TeamStadium).ToString(),
                Players = GetTeamPlayers(nodeId).ToList()

            };
            return teamModel;
        }
    }
}
