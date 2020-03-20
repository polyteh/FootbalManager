using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;

namespace UmbracoWeb.Interfaces
{
    public interface IControllerServices
    {
        bool IsNodeIdCorrect(int nodeId);
        bool IsContentNotNull(IPublishedContent content);
        IEnumerable<IPublishedContent> GetChildrensByAlias(IPublishedContent parentContent, string childrenAlias);
        int GetParentContentId(string childrenAlias);
    }
}
