using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace zavrsni.Helpers
{
    public static class LabelExtensions
    {
        public static string RemoveHTMLTags(this string text)
        {
            string text2 = Regex.Replace(text, "&scaron;", "š");
            return Regex.Replace(text2, "<.*?>", string.Empty);
        }

        public static string Truncate(this string text)
        {
            if (text.Length > 200)
            {
                return text.Substring(0, 200) + "...";
            }
            else
            {
                return text;
            }

        } 
    }
}