using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.PublishedContent;

namespace UmbracoWeb.Interfaces
{
    public interface IUmbracoHelper<T,K>
        where T:class
        where K:class
    {
        IEnumerable<K> GetNodeDescendansByDocumentTypeAlias(int nodeId, string alias);
        K MapUmbracoContentToModel(IPublishedContent content);
    }
}
