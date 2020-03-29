using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace VotingSystem.Integration.Tests.Infrastructure
{
    public static class AntiForgeryUtils
    {
        public static string ExtractFormToken(string html, string fieldName)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var verificationToken = document.DocumentNode
                .SelectSingleNode("//input[@name='test_csrf_field']")
                .GetAttributeValue<string>("value", "");

            return verificationToken;
        }

        public static string ExtractCookieToken(HttpResponseHeaders headers)
        {
            var headerValue = headers.GetValues("Set-Cookie").First();
            var cookieToken = headerValue.Split(';').First().Split('=').Last();
            return cookieToken;
        }
    }
}
