using AutoMapper;
using System;
using System.Collections.Generic;
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
    [RoutePrefix("api/footballmanager/player")]
    public class PlayerApiController : UmbracoApiController
    {
        private readonly IContentService _contentService;
        private readonly IContentTypeService _contentTypeService;
        private readonly IMapper _mapper;
        private readonly IControllerHelper _controllerService;
        private readonly IUmbracoHelper<PlayerViewModel> _umbracoPlayerHelper;

        public PlayerApiController(IContentService contentService, IContentTypeService contentTypeService, IMapper mapper,
            IControllerHelper controllerServices, IUmbracoHelper<PlayerViewModel> umbracoPlayerHelper)
        {
            _contentService = contentService;
            _contentTypeService = contentTypeService;
            _mapper = mapper;
            _controllerService = controllerServices;
            _umbracoPlayerHelper = umbracoPlayerHelper;
        }


        [HttpGet]
        [Route("Players/")]
        public IEnumerable<PlayerViewModel> GetAllPlayers()
        {

            // find Ids of root elements for Team document type

            var allPlayers = _umbracoPlayerHelper.GetAllDescendantsByAlias(UmbracoAliasConfiguration.Player.Alias);

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
            var player = _umbracoPlayerHelper.GetNodeModelById(nodeId);

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

    }
}