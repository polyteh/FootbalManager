using AutoMapper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
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
        private readonly IControllerServices _controllerService;

        public PlayerApiController(IContentService contentService, IContentTypeService contentTypeService, IMapper mapper,
            IControllerServices controllerServices)
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
                    Id = player.Id,
                    Name = player.Value(UmbracoAliasConfiguration.Player.PlayerName).ToString(),
                    Age = Int32.Parse(player.Value(UmbracoAliasConfiguration.Player.PlayerAge).ToString())
                });
                //Debug.WriteLine("New player");
                //Debug.WriteLine(item.Value("PlayerName"));
            }

            return allPlayers;
        }

    }
}