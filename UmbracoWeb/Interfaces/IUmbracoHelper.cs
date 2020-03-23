using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.PublishedContent;

namespace UmbracoWeb.Interfaces
{
    public interface IUmbracoHelper<T>
        where T:class
    {
        T GetNodeModelById(int nodeId);
        /// <summary>
        /// get descendants content by ancestor Id and descendans alias
        /// </summary>
        /// <param name="ancestorId">ancestor Id</param>
        /// <param name="descendandsAlias">descendans alias</param>
        /// <returns>return list of descendans models</returns>
        IEnumerable<T> GetDescendantsContentByAncestorId(int ancestorId, string descendandsAlias);
        IEnumerable<T> GetAllDescendantsByAlias(string descendandsAlias);
    }
}
