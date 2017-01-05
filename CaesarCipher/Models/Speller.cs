using NHunspell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CaesarCipher.Models
{
    public static class Speller
    {
        public static int Analyze(string text)
        {
            int results = -1;
            string pathToAff = HttpContext.Current.Server.MapPath("~/Content/en_US.aff");
            string pathToDic = HttpContext.Current.Server.MapPath("~/Content/en_US.dic");

            using (var hunspell = new Hunspell(pathToAff, pathToDic))
            {
                MatchCollection matches = Regex.Matches(text, "[a-zA-Z]+", RegexOptions.IgnoreCase);
                string[] words = matches.Cast<Match>().Select(m => m.Value).ToArray();
                results = words.Where(word => hunspell.Spell(word)).Count();
            }

            return results;
        }
    }
}