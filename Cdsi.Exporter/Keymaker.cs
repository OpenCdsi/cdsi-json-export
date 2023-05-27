using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenCdsi.Export
{

    public static class Keymaker
    {
        public static object Create(this string text)
        {
            IEnumerable<string> words;

            if (Regex.Match(text, "/|-|_| ").Success)
            {
                words = Regex.Replace(text, "/|-|_", " ").Split(" ").Where(word => word != "").Select(word => word.ToLower());
            }
            else if (Regex.Match(text, "[a-z][A-Z]").Success)
            {
                words = Regex.Split(text, @"(?<!^)(?=[A-Z])").Select(word => word.ToLower());
            }
            else
            {
                words = new List<string> { text.ToLower() };
            }
            var key = string.Join("-", words);

            return Regex.Match(key, @"[a-zA-Z]").Success
                ? key
                : int.Parse(key.Replace("-", ""));
        }
    }
}
