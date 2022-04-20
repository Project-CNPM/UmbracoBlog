using CleanBlog.Core.Helpers;
using CleanBlog.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace CleanBlog.Core.Services
{
    public class ArticleService : IArticleService
    {
        public IPublishedContent GetArticleListPage(IPublishedContent siteRoot)
        {
            return siteRoot.Descendants().Where(x => x.ContentType.Alias == "articleList").FirstOrDefault();

        }

        public IEnumerable<IPublishedContent> GetLatestArticles(IPublishedContent siteRoot)
        {
            var articleList = GetArticleListPage(siteRoot);
            var articles = articleList.Descendants().Where(x => x.ContentType.Alias == "article" && x.IsVisible()).OrderByDescending(y => y.Value<DateTime>("articleDate"));
            return articles;
        }
        public ArticleResultSet GetLatestArticles(IPublishedContent currentContentItem, HttpRequestBase request)
        {
            var siteRoot = currentContentItem.Root();
            var articleList = GetArticleListPage(siteRoot);
            var articles = articleList.Descendants().Where(x => x.ContentType.Alias == "article" && x.IsVisible()).OrderByDescending(y => y.Value<DateTime>("articleDate"));

            var isArticleListPage = articleList.Id == currentContentItem.Id;
            var fallbackPageSize = isArticleListPage ? 10 : 3;

            var pageNumber = QueryStringHelper.GetIntFromQueryString(request, "page", 1);
            var pageSize = QueryStringHelper.GetIntFromQueryString(request, "size", fallbackPageSize);


            var totalItemCount = articles.Count();

            var pageOfArticles = articles.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var pageCount = totalItemCount > 0 ? Math.Ceiling((double)totalItemCount / pageSize) : 1;

            var resultSet = new ArticleResultSet() 
            { PageCount = pageCount,
              PageNumber = pageNumber, 
              PageSize = pageSize,
              Results= pageOfArticles,
              IsArticleListPage = isArticleListPage,
              Url = articleList.Url
            };
            return resultSet;

        }

       
    }
}
