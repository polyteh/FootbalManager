using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;

namespace UmbracoWeb.Interfaces
{
    public interface IControllerHelper
    {
        bool IsNodeIdCorrect(int nodeId);
        bool IsContentNotNull(IPublishedContent content);
        IEnumerable<IPublishedContent> GetChildrensByAlias(IPublishedContent parentContent, string childrenAlias);
        int GetRootId(string childrenAlias);
        IEnumerable<int> GetAllRootsId(string childrenAlias);
    }
}
