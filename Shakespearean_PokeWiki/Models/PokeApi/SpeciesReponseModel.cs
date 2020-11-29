using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Shakespearean_PokeWiki.Models.PokeApi
{
    //NB: the full model has not been included, unneeded properties have been omitted
    public class SpeciesReponseModel
    {
        [JsonProperty(PropertyName = "flavor_text_entries")]
        public IEnumerable<FlavorTextEntryDetail> flavorTextEntries { get; set; }

    }

    public class FlavorTextEntryDetail
    {
        [JsonProperty(PropertyName = "flavor_text")]
        public string flavorText { get; set; }
        public NamedAPIResource language { get; set; }
        public NamedAPIResource version { get; set; }
    }
}