using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shakespearean_PokeWiki.Models.Pokemon
{
    public class TranslatedPokemonResponseModel
    {
        public string name { get; set; }
        public string description { get; set; }

        public TranslatedPokemonResponseModel()
        {

        }
        public TranslatedPokemonResponseModel(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
    }
}