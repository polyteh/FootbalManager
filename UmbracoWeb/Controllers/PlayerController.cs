using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;
using UmbracoWeb.Models;

namespace UmbracoWeb.Controllers
{
    public class PlayerController : Umbraco.Web.Mvc.RenderMvcController
    {
        // GET: Player
        public ActionResult Player(ContentModel model)
        {
            var property = model.Content.GetProperty("playerAge").GetValue();
            var playerModel = new Player(model.Content);
            return View(playerModel);
        }
    }
}