using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace UmbracoWeb.Filters
{
    public class ActionCORSFilter: ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Request.Headers.Referrer == null)
            {
                return;
            }
            var requestHeaderReferrer = actionExecutedContext.Request.Headers.Referrer.OriginalString;
            string responseHeader = requestHeaderReferrer.Substring(0, requestHeaderReferrer.Length - 1);
            actionExecutedContext.Response.Content.Headers.Add("Access-Control-Allow-Origin", "http://localhost:3000");
        }
    }
}