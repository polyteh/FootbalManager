using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UmbracoWeb.Models
{
    public class TeamViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StadiumName { get; set; }
        public List<PlayerViewModel> Players { get; set; }
    }
}