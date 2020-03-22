using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using Umbraco.Core.Services;
using UmbracoWeb.Interfaces;
using Umbraco.Web;
using Umbraco.Core.Models.PublishedContent;

namespace UmbracoWeb.Services
{
    public abstract class UmbracoHelper<T, K> : UmbracoWebService, IUmbracoHelper<T, K>
        where T : class
        where K : class
    {
        protected readonly IContentService _contentService;
        protected readonly IContentTypeService _contentTypeService;
        protected readonly IMapper _mapper;
        protected readonly IControllerHelper _controllerService;
        public UmbracoHelper(IContentService contentService, IContentTypeService contentTypeService, IMapper mapper,
             IControllerHelper controllerServices)
        {
            _contentService = contentService;
            _contentTypeService = contentTypeService;
            _mapper = mapper;
            _controllerService = controllerServices;
        }
        public IEnumerable<K> GetNodeDescendansByDocumentTypeAlias(int nodeId, string alias)
        {
            //int nodeID = 2068; //Barcelona content

            if (!(_controllerService.IsNodeIdCorrect(nodeId)))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var contentById = Umbraco.Content(nodeId);
            if (!(_controllerService.IsContentNotNull(contentById)))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if (contentById.Descendants().Where(x => x.IsDocumentType(alias)).FirstOrDefault() == null)
            {
                return new List<K>();
            }

            var descendatnsContent = contentById.Descendants().Where(x => x.IsDocumentType(alias));


            if (descendatnsContent == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            List<K> descendantsList = new List<K>();
            foreach (var content in descendatnsContent)
            {

                descendantsList.Add(MapUmbracoContentToModel(content));

            }
            return descendantsList;
        }
        public abstract K MapUmbracoContentToModel(IPublishedContent content);

    }
}