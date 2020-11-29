using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shakespearean_PokeWiki.Models.PokeApi
{
    //NB: the full model has not been included, unneeded properties have been omitted
    public class PokemonResponseModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public NamedAPIResource species { get; set; }
    }
}