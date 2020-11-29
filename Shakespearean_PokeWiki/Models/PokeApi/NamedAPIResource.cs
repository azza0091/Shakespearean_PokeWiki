using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shakespearean_PokeWiki.Models.PokeApi
{
    public class NamedAPIResource
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public NamedAPIResource() { }
        public NamedAPIResource(string Name, string Url)
        {
            this.Name = Name;
            this.Url = Url;
        }
    }
}