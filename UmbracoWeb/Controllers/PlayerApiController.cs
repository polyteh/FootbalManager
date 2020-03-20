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

        public PlayerApiController(IContentService contentService, IContentTypeService contentTypeService, IMapper mapper,
            IControllerHelper controllerServices)
        {
            _contentService = contentService;
            _contentTypeService = contentTypeService;
            _mapper = mapper;
            _controllerService = controllerServices;
        }


        [HttpGet]
        [Route("Players/")]
        public IEnumerable<PlayerViewModel> GetAllPlayers()
        {

            // find Ids of root elements for Team document type

            var playersRoots = _controllerService.GetAllRootsId(UmbracoAliasConfiguration.Player.Alias);

            if (playersRoots==null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            List<IPublishedContent> playerListContent = new List<IPublishedContent>();
            foreach (var playersRootId in playersRoots)
            {
                var playersContentById = Umbraco.Content(playersRootId);
                playerListContent.AddRange(_controllerService.GetChildrensByAlias(playersContentById, UmbracoAliasConfiguration.Player.Alias));
            }         

            if (playerListContent == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            List<PlayerViewModel> allPlayers = new List<PlayerViewModel>();
            foreach (var player in playerListContent)
            {
                allPlayers.Add(new PlayerViewModel()
                {
                    Id = player.Id,
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

            if (!(_controllerService.IsContentNotNull(playerContent)))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var player = new PlayerViewModel()
            {
                Id = playerContent.Id,
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

    }
}