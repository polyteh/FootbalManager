using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Models;
using Umbraco.Web;

namespace UmbracoWeb.Models
{
    public class Player: PublishedContentBase, IPublishedContent
    {
        IPublishedContent _content;
        public Player(IPublishedContent content) 
        {
            _content=content;
        }

        public override IPublishedContentType ContentType
        { get { return _content.ContentType; } }

        public override Guid Key => throw new NotImplementedException();

        public override int Id => throw new NotImplementedException();

        public override int SortOrder => throw new NotImplementedException();

        public override int Level => throw new NotImplementedException();

        public override string Path => throw new NotImplementedException();

        public override int? TemplateId => throw new NotImplementedException();

        public override int CreatorId => throw new NotImplementedException();

        public override string CreatorName => throw new NotImplementedException();

        public override DateTime CreateDate => throw new NotImplementedException();

        public override int WriterId => throw new NotImplementedException();

        public override string WriterName => throw new NotImplementedException();

        public override DateTime UpdateDate => throw new NotImplementedException();

        public override IReadOnlyDictionary<string, PublishedCultureInfo> Cultures => throw new NotImplementedException();

        public override PublishedItemType ItemType => throw new NotImplementedException();

        public override IPublishedContent Parent 
        {
            get { return _content.Parent; }
        }

        public override IEnumerable<IPublishedContent> ChildrenForAllCultures => throw new NotImplementedException();

        public override IEnumerable<IPublishedProperty> Properties => throw new NotImplementedException();

        public override IPublishedProperty GetProperty(string alias)
        {
            var property = _content.GetProperty(alias);
            return property;
        }

        public override bool IsDraft(string culture = null)
        {
            throw new NotImplementedException();
        }

        public override bool IsPublished(string culture = null)
        {
            throw new NotImplementedException();
        }

        public string StadiumName 
        {
            get 
            {
                var stadiumName = _content.Value<string>("stadiumName", fallback: Fallback.ToAncestors);
                return stadiumName;
            }
        }
    }
}