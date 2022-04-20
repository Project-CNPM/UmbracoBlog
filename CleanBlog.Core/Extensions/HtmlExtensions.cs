using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace CleanBlog.Core.Extensions
{
    public static class HtmlExtensions
    {
        public static IHtmlString GetGridHtml(this HtmlHelper html, IPublishedContent contentItem, string propertyAlias, string framework, bool fluidImages)
        {
            if (fluidImages)
            {
                return html.Raw(html.GetGridHtml(contentItem, propertyAlias, framework).ToString().Replace("<img ", "<img class=\"img-fluid\" "));
            }
            else
            {
                return html.GetGridHtml(contentItem, propertyAlias, framework);
            }
        }
    }
}
