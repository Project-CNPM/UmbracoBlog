using Examine;
using Examine.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models.PublishedContent;
using Lucene.Net.Analysis;
using Umbraco.Web;
using CleanBlog.Core.Extensions;
using static Umbraco.Core.Constants;

namespace CleanBlog.Core.Services
{
    public class SearchService : ISearchService
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;

        public SearchService(IUmbracoContextAccessor umbracoContextAccessor)
        {
            _umbracoContextAccessor = umbracoContextAccessor;
        }
        public IEnumerable<IPublishedContent> GetPageOfContentSearchResults(string searchTerm, string category, int pageNumber, out long totalItemCount, string[] docTypeAliases, int pageSize = 10)
        {
            var pageOfResults = GetPageOfSearchResults(searchTerm, category, pageNumber,
                 out totalItemCount, docTypeAliases, "content");

            var items = new List<IPublishedContent>();
            if (pageOfResults != null )
            {
                foreach (var item in pageOfResults)
                {
                    var page = _umbracoContextAccessor.UmbracoContext.Content.GetById(int.Parse(item.Id));
                    if (page != null)
                    {
                        items.Add(page);
                    }
                }
            }

            return items;
        }

        public IEnumerable<ISearchResult> GetPageOfSearchResults(string searchTerm, string category, int pageNumber, out long totalItemCount, string[] docTypeAliases, string searchType, int pageSize = 10)
        {
            int skip = pageNumber > 1 ? (pageNumber - 1) * pageSize : 0;
            string[] terms = !string.IsNullOrEmpty(searchTerm) && searchTerm.Contains(" ")
                    ? searchTerm.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    : !string.IsNullOrWhiteSpace(searchTerm) ? new string[] { searchTerm } : null;
            if (ExamineManager.Instance.TryGetIndex(UmbracoIndexes.ExternalIndexName, out var index))
            {
                if(terms != null && terms.Any())
                {
                    //optimize word for search ex in the night garden => night garden
                    terms = terms.Where(x => !StopAnalyzer.ENGLISH_STOP_WORDS_SET.Contains(x.ToLower()) &&
                       x.Length > 2).ToArray();
                }           
                var searcher = index.GetSearcher();
                var criteria = searcher.CreateQuery(searchType);
                var query = criteria.GroupedNot(new string[] { "umbracoNaviHide" },
                    new string[] { "1" });

                if (terms != null && terms.Any())
                {
                    query.And(q => q
                    .GroupedOr(new[] { "nodeName" }, terms.Boost(10))
                    .Or()
                    .GroupedOr(new[] { "title" }, terms.Boost(8))
                    .Or()
                    .GroupedOr(new[] { "subtitle" }, terms.Boost(6))
                    .Or()
                    .GroupedOr(new[] { "mainContent" }, terms.Boost(4))
                    .Or()
                    .GroupedOr(new[] { "metaDescription" }, terms.Boost(2))
                    .Or()
                    .GroupedOr(new[] { "nodeName", "title", "subtitle", "mainContent", "metaDescription",
                        "keywords" }, terms.Fuzzy()), BooleanOperation.Or);
                }

                if (docTypeAliases != null && docTypeAliases.Any())
                {
                    query.And(q => q.GroupedOr(new[] { "__NodeTypeAlias" }, docTypeAliases));
                }
                if (!string.IsNullOrWhiteSpace(category))
                {
                    query.And().Field("searchableCategories", category);
                }

                var allResults = query.Execute();

                totalItemCount = allResults.TotalItemCount;

                var pageOfResults = allResults.Skip(skip).Take(pageSize);

                return pageOfResults;
            }

            totalItemCount = 0;
            return Enumerable.Empty<ISearchResult>();
        }
    }
}
