using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

namespace zavrsni.Helpers
{
    public static class LabelExtensions
    {
        public static string ParseHTML(this string text)
        {
            string text2 = Regex.Replace(text, "&scaron;", "š");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(text2);
            if (!text2.Contains("<iframe") && !text2.Contains("<img"))
            {
                string s = doc.DocumentNode.SelectSingleNode("//p").InnerText;
                return s;
            }

            if (text2.Contains("<img")) return text2;
            if (text2.Contains("<iframe")) return text2;
            return text2;
        }

        public static string Truncate(this string text)
        {
            if (text.Length > 200 && !text.Contains("<img"))
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