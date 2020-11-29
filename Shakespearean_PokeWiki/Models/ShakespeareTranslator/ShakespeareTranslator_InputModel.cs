using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shakespearean_PokeWiki.Models.ShakespeareTranslator
{
    public class ShakespeareTranslator_InputModel
    {
        public string text { get; set; }

        public ShakespeareTranslator_InputModel()
        {

        }
        public ShakespeareTranslator_InputModel(string text)
        {
            this.text = text;
        }
    }
}