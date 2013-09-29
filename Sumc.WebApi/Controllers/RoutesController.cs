using HtmlAgilityPack;
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
    public class RoutesController : BaseApiController
    {
        private readonly RouteRepository routeRepository;

        public RoutesController()
        {
            this.routeRepository = new RouteRepository();
        }

        [HttpGet]
        public object GetRoutes(string from, string to)
        {
            var response = this.routeRepository.GetRoutes(from, to, this.SessionKey, this.GlobalAuthKey);

            var text = this.GetTextFromResponse(response);
            var document = this.GetDocumentReader(text);
            //No returned cookies for this page
            if (!response.ResponseUri.AbsolutePath.Contains("search"))
            {
                //direct match
                return new RouteResult();
            }

            var routeSearch = new RouteSearch();
            routeSearch.FromOrigin = document.DocumentNode.SelectSingleNode("//input[@name='orig_from']").Attributes.First(a => a.Name == "value").Value;
            routeSearch.ToOrigin = document.DocumentNode.SelectSingleNode("//input[@name='orig_to']").Attributes.First(a => a.Name == "value").Value;
            routeSearch.FromJson = document.DocumentNode.SelectSingleNode("//input[@name='json_from']").Attributes.First(a => a.Name == "value").Value;
            routeSearch.ToJson = document.DocumentNode.SelectSingleNode("//input[@name='json_to']").Attributes.First(a => a.Name == "value").Value;
            routeSearch.FromResults = this.ToRoutesStopsInformation(document.DocumentNode.SelectNodes("//input[@name='from_radio']"));
            routeSearch.ToResults = this.ToRoutesStopsInformation(document.DocumentNode.SelectNodes("//input[@name='to_radio']"));
            return routeSearch;
        }

        private IEnumerable<RouteStopInformation> ToRoutesStopsInformation(HtmlNodeCollection htmlNodeCollection)
        {
            foreach (var node in htmlNodeCollection)
            {
                var value = node.Attributes.First(a => a.Name == "value").Value;
                var stopInformation = new Queue<string>( node.NextSibling.InnerText.Split(','));
                var number = stopInformation.Dequeue();
                var name = string.Join(",", stopInformation);
                yield return new RouteStopInformation { Name = name, Number = number, Value = value };
            }
        }

        public RouteResult GetResultsExtented(string from, string to, string fromOrigin, string toOrigin, string fromJson, string toJson, int chosenTo, int chosenFrom)
        {
            // Rresults does not work.
            //var response = this.routeRepository.GetRoutes(from, to, fromJson, toJson, chosenTo, chosenFrom, fromOrigin, toOrigin, this.SessionKey, this.GlobalAuthKey);
            //this.SessionKey = this.GetCookie(response);

            //var text = this.GetTextFromResponse(response);
            //var document = this.GetDocumentReader(text);

            return new RouteResult();
        }
    }
}
