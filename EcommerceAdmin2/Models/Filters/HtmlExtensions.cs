using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Filters
{
    public static class HtmlExtensions
    {
        private static readonly HtmlContentBuilder _emptyBuilder = new HtmlContentBuilder();

        public static IHtmlContent BuildBreadcrumbNavigation(this IHtmlHelper helper)
        {
            if (helper.ViewContext.RouteData.Values["controller"].ToString() == "Home" ||
                helper.ViewContext.RouteData.Values["controller"].ToString() == "Account")
            {
                return _emptyBuilder;
            }

            string controllerName = helper.ViewContext.RouteData.Values["controller"].ToString();
            string actionName = helper.ViewContext.RouteData.Values["action"].ToString();

            var breadcrumb = new HtmlContentBuilder()
                                .AppendHtml("<ol class='breadcrumb'><li>")
                                .AppendHtml(helper.ActionLink("Home", "Index", "Home"))
                                .AppendHtml("</li><li>")
                                .AppendHtml(helper.ActionLink(controllerName.Titleize(),
                                                          "Index", controllerName))
                                .AppendHtml("</li>");


            if (helper.ViewContext.RouteData.Values["action"].ToString() != "Index")
            {
                breadcrumb.AppendHtml("<li>")
                          .AppendHtml(helper.ActionLink(actionName.Titleize(), actionName, controllerName))
                          .AppendHtml("</li>");
            }

            return breadcrumb.AppendHtml("</ol>");
        }
    }
    public static class StringExtensions
    {
        public static string Titleize(this string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text).ToSentenceCase();
        }

        public static string ToSentenceCase(this string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        }
    }
}
