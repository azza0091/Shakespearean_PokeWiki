using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shakespearean_PokeWiki.Models.ShakespeareTranslator
{
    public class ShakespeareTranslator_ResponseModel
    {
        public ShakespeareTranslator_ResponseModelContent contents { get; set; }
    }

    public class ShakespeareTranslator_ResponseModelContent
    {
        public string text { get; set; }
        public string translated { get; set; }
        public string translation { get; set; }
    }
}