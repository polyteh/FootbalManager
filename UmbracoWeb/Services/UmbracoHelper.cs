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
    public abstract class UmbracoHelper<T> : UmbracoWebService, IUmbracoHelper<T>
        where T : class
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

        public T GetNodeModelById(int nodeId)
        {

            if (!(_controllerService.IsNodeIdCorrect(nodeId)))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var nodeContent = Umbraco.Content(nodeId);
            if (!(_controllerService.IsContentNotNull(nodeContent)))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            T contentModel = MapUmbracoContentToModel(nodeContent);
            return contentModel;
        }

        public IEnumerable<T> GetDescendantsContentByAncestorId(int ancestorId, string descendandsAlias)
        {

            if (!(_controllerService.IsNodeIdCorrect(ancestorId)))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var contentById = Umbraco.Content(ancestorId);
            if (!(_controllerService.IsContentNotNull(contentById)))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if (contentById.Descendants().Where(x => x.IsDocumentType(descendandsAlias)).FirstOrDefault() == null)
            {
                return new List<T>();
            }

            var descendatnsContent = contentById.Descendants().Where(x => x.IsDocumentType(descendandsAlias));


            if (descendatnsContent == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            List<T> descendantsList = new List<T>();
            foreach (var content in descendatnsContent)
            {

                descendantsList.Add(MapUmbracoContentToModel(content));

            }
            return descendantsList;
        }

        public IEnumerable<T> GetAllDescendantsByAlias(string descendandsAlias)
        {
            // find Ids of root elements for Team document type

            var descendansRoots = _controllerService.GetAllRootsId(descendandsAlias);

            if (descendansRoots == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            List<IPublishedContent> descendantsListContent = new List<IPublishedContent>();
            foreach (var descendant in descendansRoots)
            {
                var playersContentById = Umbraco.Content(descendant);
                descendantsListContent.AddRange(_controllerService.GetChildrensByAlias(playersContentById, descendandsAlias));
            }

            if (descendantsListContent == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            List<T> descendansList = new List<T>();
            foreach (var descendant in descendantsListContent)
            {
                descendansList.Add(MapUmbracoContentToModel(descendant));
            }

            return descendansList;
        }
        public abstract T MapUmbracoContentToModel(IPublishedContent content);
    }
}