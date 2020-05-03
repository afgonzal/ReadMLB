using System;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace ReadMLB2020
{
    internal class ScheduleHelper
    {
        private const string ScoreEx = "^[WL], \\d+-\\d+$";
        public static string ExtractRival(HtmlNode row)
        {
            return row.ChildNodes[1].InnerHtml.TrimEnd();
        }

        public static Score ExtractScore(HtmlNode row)
        {
            var result = row.ChildNodes[2].InnerHtml;
            var w = Regex.Match(result, "^[WL]").Value == "W" ? true : false;
            var teamScore = Convert.ToByte(Regex.Match(result, "\\d+-").Value.Replace('-',' '));
            var rivalScore = Convert.ToByte(Regex.Match(result, "\\d+$").Value);

            return new Score { RivalScore =  rivalScore, TeamScore = teamScore, W = w};
        }

        public static DateTime ExtractDate(HtmlNode row)
        {
            return DateTime.ParseExact(row.ChildNodes[0].ChildNodes[0].InnerHtml, "MM/dd/yyyy hh:mm tt",
                null);
        }

        public static HtmlNode FindScheduleTable(HtmlDocument html, byte teamId)
        {
            var node = html.DocumentNode.SelectSingleNode($"//b/a[@name='t{teamId}']").ParentNode;

            do
            {
                node = node.NextSibling;
            } while (node.Name != "table" || node.FirstChild?.FirstChild == null || node.FirstChild.FirstChild.InnerHtml != "Date/Time");

            return node;

        }

        public static bool IsAtHome(ref string rival)
        {
            if (rival.StartsWith("at "))
            {
                rival = rival.Substring(3);
                return false;
            }

            return true;
        }
    }
}