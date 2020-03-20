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
        private readonly IControllerServices _controllerService;
        public TeamApiController(IContentService contentService, IContentTypeService contentTypeService, IMapper mapper,
             IControllerServices controllerServices)
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
        /// Get all players from DB
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns>List of players</returns>
        [HttpGet]
        [Route("Players/")]
       
        public IEnumerable<PlayerViewModel> GetAllPlayers()
        {
            //int nodeID = 2077; //Players content

            // find root element of Team document type
            int playersRootId = _controllerService.GetParentContentId(UmbracoAliasConfiguration.Player.Alias);

            var playersContentById = Umbraco.Content(playersRootId);
            if (!(_controllerService.IsContentNotNull(playersContentById)))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }


            IEnumerable<IPublishedContent> playersListContent = _controllerService.GetChildrensByAlias(playersContentById, UmbracoAliasConfiguration.Player.Alias);

            if (playersListContent == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            List<PlayerViewModel> allPlayers = new List<PlayerViewModel>();
            foreach (var player in playersListContent)
            {
                allPlayers.Add(new PlayerViewModel()
                {
                    Id=player.Id,
                    Name = player.Value(UmbracoAliasConfiguration.Player.PlayerName).ToString(),
                    Age = Int32.Parse(player.Value(UmbracoAliasConfiguration.Player.PlayerAge).ToString())
                });
                //Debug.WriteLine("New player");
                //Debug.WriteLine(item.Value("PlayerName"));
            }

            return allPlayers;
        }

        /// <summary>
        /// Get player by Id
        /// </summary>
        /// <exception cref="HttpResponseException">Not Found</exception>
        /// <param name="nodeId">Player ID</param>
        /// <returns>Player Model</returns>
        [HttpGet]
        [Route("GetPlayerById/{nodeId}")]
       
        public PlayerViewModel GetPlayerById(int nodeId)
        {
            //int nodeId = 2084; //Varan
            if ((!_controllerService.IsNodeIdCorrect(nodeId)))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var playerContent = Umbraco.Content(nodeId);
            var isP = playerContent.IsPublished();
            // var parentInfo = playerContent.Parent;
            if (!(_controllerService.IsContentNotNull(playerContent)))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var player = new PlayerViewModel()
            {
                Id=playerContent.Id,
                Name = playerContent.Value(UmbracoAliasConfiguration.Player.PlayerName).ToString(),
                Age = Int32.Parse(playerContent.Value(UmbracoAliasConfiguration.Player.PlayerAge).ToString())
            };

            return player;
        }

        /// <summary>
        /// Add new player
        /// </summary>
        /// <param name="newPlayer"></param>
        /// <returns>Player model, which was created</returns>
        [HttpPost]
        [Route("AddNewPlayer/")]
       
        public PlayerViewModel AddNewPlayer(PlayerViewModel newPlayer)
        {
            int nodeID = 2077;

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

            return newPlayer;
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
            // find root element of Team document type
            int teamsRootId = _controllerService.GetParentContentId(UmbracoAliasConfiguration.Team.Alias);

            //get all Teams child and get data from them List<TeamViewModel>
            var teamsContent = Umbraco.Content(teamsRootId);
            IEnumerable<IPublishedContent> teamsListContent = _controllerService.GetChildrensByAlias(teamsContent, UmbracoAliasConfiguration.Team.Alias);

            //map data from IPublishedContent to the 
            List<TeamViewModel> allTeams = new List<TeamViewModel>();
            foreach (var team in teamsListContent)
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

            if (!teamPlayersContentById.HasValue(UmbracoAliasConfiguration.Team.Players))
            {
                return new List<PlayerViewModel>();
            }

            var teamPlayersContent = teamPlayersContentById.Value<IEnumerable<IPublishedContent>>(UmbracoAliasConfiguration.Team.Players).ToList();

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
        /// <returns></returns>
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
