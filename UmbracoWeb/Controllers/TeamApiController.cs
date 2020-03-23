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
        private readonly IUmbracoHelper<TeamViewModel> _umbracoTeamHelper;

        public TeamApiController(IContentService contentService, IContentTypeService contentTypeService, IMapper mapper,
             IControllerHelper controllerServices, IUmbracoHelper<TeamViewModel> umbracoTeamHelper)
        {
            _contentService = contentService;
            _contentTypeService = contentTypeService;
            _mapper = mapper;
            _controllerService = controllerServices;
            _umbracoTeamHelper = umbracoTeamHelper;
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

            var allTeams = _umbracoTeamHelper.GetAllDescendantsByAlias(UmbracoAliasConfiguration.Team.Alias);
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
            try
            {
                var teamModel = _umbracoTeamHelper.GetNodeModelById(nodeId);
                return teamModel.Players;
            }
            catch (Exception)
            {

                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

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
            try
            {
                var teamModel = _umbracoTeamHelper.GetNodeModelById(nodeId);
                return teamModel;
            }
            catch (Exception)
            {

                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

        }
    }
}
