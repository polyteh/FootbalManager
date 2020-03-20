using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace UmbracoWeb.Controllers
{
    public class TagController : SurfaceController
    {
        private readonly IContentService _contentService;
        private readonly ITagQuery _tagService;
        public TagController(IContentService contentService, ITagQuery tagService)
        {
            _contentService = contentService;
            _tagService = tagService;
        }

        [HttpPost]
        public ActionResult GetContentByTags(string[] tags)
        {
            if (tags!=null)
            {

                List<string> tagItems = new List<string>();
                foreach (var item in tags)
                {

                    tagItems.AddRange(_tagService.GetContentByTag(item).Select(x => x.Name).ToList());
                }
                

                 return Json(tagItems, JsonRequestBehavior.AllowGet); ;
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}