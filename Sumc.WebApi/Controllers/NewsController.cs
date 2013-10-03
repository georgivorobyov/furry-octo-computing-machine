using Sumc.Models.Response;
using Sumc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sumc.WebApi.Controllers
{
    public class NewsController : BaseApiController
    {
        private readonly NewsRepository newsRepository;

        public NewsController()
        {
            this.newsRepository = new NewsRepository();
        }

        public HttpResponseMessage GetNews()
        {
            return this.PerformOperation<IEnumerable<NewsGlanceInformation>>(() =>
            {
                var response = this.newsRepository.GetNews(this.SessionKey, this.GlobalAuthKey);
                var text = this.GetTextFromResponse(response);
                var document = this.GetDocumentReader(text);

                var news = new List<NewsGlanceInformation>();
                var newsLinks = document.DocumentNode.SelectNodes("//page_box/a]");
                var newsDates = document.DocumentNode.SelectNodes("//page_box/span[@class='date']");
                for (int i = 0; i < newsLinks.Count; i++)
                {
                    var glanceNews = new NewsGlanceInformation();
                    glanceNews.Id = newsLinks[i].Attributes.First(a => a.Name == "href").Value;
                    glanceNews.Title = newsLinks[i].InnerHtml.Trim('\t', '\n');
                    glanceNews.Date = DateTime.Parse(newsLinks[i].InnerHtml.Trim('\t', '\n'));
                    news.Add(glanceNews);
                }

                return news;
            });
        }

        public HttpResponseMessage GetNews(string id)
        {
            return this.PerformOperation<News>(() =>
            {
                var response = this.newsRepository.GetNews(this.SessionKey, this.GlobalAuthKey);
                var text = this.GetTextFromResponse(response);
                var document = this.GetDocumentReader(text);

                var news = new News();
                news.Title = document.DocumentNode.SelectSingleNode("//*[@class='page_title']").InnerText.Trim('\n', '\t');
                news.Content = document.DocumentNode.SelectSingleNode("*[@class='page_cnt']").InnerText;
                news.Date = DateTime.Parse(document.DocumentNode.SelectSingleNode("*[@class='date']").InnerText.Trim('\n', '\t'));
                return news;
            });
        }
    }
}
