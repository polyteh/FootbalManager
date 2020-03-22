using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UmbracoWeb.Models
{
    public class LeagueViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TeamViewModel> Teams { get; set; }
        public LeagueViewModel()
        {
            Teams = new List<TeamViewModel>();
        }
    }
}