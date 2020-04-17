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
    [RoutePrefix("api/footballmanager/league")]
    public class LeagueApiController : UmbracoApiController
    {
        private readonly IContentService _contentService;
        private readonly IContentTypeService _contentTypeService;
        private readonly IMapper _mapper;
        private readonly IControllerHelper _controllerService;
        private readonly IUmbracoHelper<LeagueViewModel> _umbracoLeagueHelper;
        public LeagueApiController(IContentService contentService, IContentTypeService contentTypeService, IMapper mapper,
            IControllerHelper controllerServices, IUmbracoHelper<LeagueViewModel> umbracoHelper)
        {
            _contentService = contentService;
            _contentTypeService = contentTypeService;
            _mapper = mapper;
            _controllerService = controllerServices;
            _umbracoLeagueHelper = umbracoHelper;
        }

        [HttpGet]
        [Route("GetLeagueById/{nodeId}")]
        public LeagueViewModel GetLeagueById(int nodeId)
        {
            //int nodeID=2110; //Primara content
            LeagueViewModel leagueModel = _umbracoLeagueHelper.GetNodeModelById(nodeId);
            return leagueModel;
        }

        [HttpGet]
        [Route("Leagues/")]
        public IEnumerable<LeagueViewModel> GetAllLeagues()
        {

            var allLeagues = _umbracoLeagueHelper.GetAllDescendantsByAlias(UmbracoAliasConfiguration.League.Alias);
            return allLeagues;
        }

        /// <summary>
        /// get league names for menu
        /// </summary>
        /// <returns>list of league names</returns>
        [HttpGet]
        [Route("LeagueNames/")]
        public IEnumerable<LeagueNameViewModel> GetAllLeagueNames()
        {
            IEnumerable<LeagueViewModel> fullLeagueList = GetAllLeagues();
            var leagueList = _mapper.Map<IEnumerable<LeagueNameViewModel>>(fullLeagueList);
            return leagueList;
        }

    }
}