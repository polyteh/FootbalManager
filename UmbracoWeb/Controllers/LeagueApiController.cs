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
        //[HttpGet]
        //[Route("Leagues/")]
        //public IEnumerable<LeagueViewModel> GetAllLeagues()
        //{

        //    // find Ids of root elements for League document type

        //    var leaguesRoots = _controllerService.GetAllRootsId(UmbracoAliasConfiguration.League.Alias);

        //    if (leaguesRoots == null)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.NotFound);
        //    }

        //    List<IPublishedContent> leagueListContent = new List<IPublishedContent>();
        //    foreach (var leagueRootId in leaguesRoots)
        //    {
        //        var leaguesContentById = Umbraco.Content(leagueRootId);
        //        leagueListContent.AddRange(_controllerService.GetChildrensByAlias(leaguesContentById, UmbracoAliasConfiguration.League.Alias));
        //    }

        //    if (leagueListContent == null)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.NotFound);
        //    }

        //    List<LeagueViewModel> allLeagues = new List<LeagueViewModel>();
        //    foreach (var league in leagueListContent)
        //    {
        //        var k = _umbracoLeagueHelper.GetNodeDescendanModelsByDocumentTypeAlias(league.Id, UmbracoAliasConfiguration.Team.Alias);
        //        allLeagues.Add(new LeagueViewModel()
        //        {
        //            Id = league.Id,
        //            Name = league.Value(UmbracoAliasConfiguration.League.LeagueName).ToString(),
        //            Teams = k as List<TeamViewModel>
        //        });;
        //    }

        //    return allLeagues;
        //}


        [HttpGet]
        [Route("GetLeagueById/{nodeId}")]
        public LeagueViewModel GetLeagueById(int nodeId)
        {
            //int nodeID=2110; //Primara content
            LeagueViewModel leagueModel = _umbracoLeagueHelper.GetNodeModelById(nodeId);
            return leagueModel;
        }


        [HttpGet]
        [Route("Stub/")]
        public string GetStubString()
        {
            return "stub";
        }

    }
}