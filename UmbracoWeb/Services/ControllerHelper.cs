using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Umbraco.Web.WebApi;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Composing;
using UmbracoWeb.Configuration;
using UmbracoWeb.Filters;
using UmbracoWeb.Interfaces;
using UmbracoWeb.Models;

namespace UmbracoWeb.Services
{
    public class ControllerHelper : UmbracoWebService, IControllerHelper
    {
        /// <summary>
        /// check, if nodeId is above zero
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public bool IsNodeIdCorrect(int nodeId)
        {
            return nodeId > 0 ? true : false;
        }

        /// <summary>
        /// check, if content with specified node exists
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool IsContentNotNull(IPublishedContent content)
        {
            return content != null ? true : false;
        }

        /// <summary>
        /// select children from parent by alias
        /// </summary>
        /// <param name="parentContent"></param>
        /// <param name="childrenAlias"></param>
        /// <returns></returns>
        public IEnumerable<IPublishedContent> GetChildrensByAlias(IPublishedContent parentContent, string childrenAlias)
        {
            IEnumerable<IPublishedContent> contentList = parentContent.Children;
            if (contentList.Any() == true)
            {
                return contentList.Where(x => x.IsDocumentType(childrenAlias));
            }
            return null;
        }
        /// <summary>
        /// return the nearest root Id for the alias
        /// </summary>
        /// <param name="childrenAlias"></param>
        /// <returns></returns>
        public int GetRootId(string childrenAlias)
        {
            var listOfRoots = Umbraco.ContentAtRoot();

            int rootId = 0;
            if (listOfRoots != null)
            {
                foreach (var root in listOfRoots)
                {
                    foreach (var item in root.Descendants().Where(x => x.IsDocumentType(childrenAlias)))
                    {
                        rootId = item.Parent.Id;
                        // i.e.: we asssume, that we have only one root element with Team document type, called Teams, exists
                        break;
                    }
                }
            }
            return rootId;
        }


        /// <summary>
        /// return all roots Id for specific alias
        /// </summary>
        /// <param name="childrenAlias"></param>
        /// <returns></returns>
        public IEnumerable<int> GetAllRootsId(string childrenAlias)
        {
            var listOfRoots = Umbraco.ContentAtRoot();

            List<int> rootsId = new List<int>();
            if (listOfRoots != null)
            {
                foreach (var root in listOfRoots)
                {
                    foreach (var item in root.Descendants().Where(x => x.IsDocumentType(childrenAlias)))
                    {
                        rootsId.Add(item.Parent.Id);
                    }
                }
            }
            return rootsId.Distinct();
        }
    }
}