using HtmlAgilityPack;
using Sumc.Models.Response;
using Sumc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Sumc.WebApi.Controllers
{
    public class VirtualTablesController : BaseApiController
    {
        private readonly VirtualTableRepository vtRepository;

        public VirtualTablesController()
        {
            this.vtRepository = new VirtualTableRepository();
        }

        //VirtualTable
        public HttpResponseMessage Get(string query)
        {
            return this.PerformOperation(() =>
            {
                if (!this.sessionRepository.IsActiveAuthCookie())
                {
                    throw new ArgumentOutOfRangeException("Cookie");
                }

                var response = this.vtRepository.Get(query, this.SessionKey, this.GlobalAuthKey);

                var cookie = this.GetCookie(response);
                this.SessionKey = cookie;

                var text = this.GetTextFromResponse(response);
                var document = this.GetDocumentReader(text);
                var virtualTable = new VirtualTable();

                var error = document.DocumentNode.SelectSingleNode("//*[@class='error']");
                if (!string.IsNullOrWhiteSpace(error.InnerText))
                {
                    virtualTable.IsFound = false;
                    return virtualTable;
                }
                else
                {
                    virtualTable.IsFound = true;
                }

                var pageContent = document.DocumentNode.SelectSingleNode("//*[@class='page_cnt']");

                var stops = pageContent.ChildNodes.Where(s => s.Name == "b").Skip(1).Select(x => x.InnerText);
                virtualTable.FoundStops = this.ToStops(stops);
                this.Vehicles(pageContent, virtualTable);
                virtualTable.Stop = this.ToStop(pageContent.ChildNodes.First(s => s.Name == "b").InnerText);
                virtualTable.Information = pageContent.SelectSingleNode("//*[@class='info']").InnerText;
                return virtualTable;
            });
        }

        private IEnumerable<Stop> ToStops(IEnumerable<string> stops)
        {
            foreach (var stop in stops)
            {
                var cleanStop = stop.Replace("спирка", string.Empty);
                yield return this.ToStop(cleanStop);
            }
        }

        private Stop ToStop(string stop)
        {
            var stopDetails = stop.Split(new char[] { '(', ')' });
            var name = stopDetails[0].Substring(stopDetails[0].IndexOf('.') + 1);
            name = name.Replace("&nbsp;", " ");
            name = name.Replace("&nbsp", " ");
            name = name.Replace("\t", "");
            name = name.Trim();
            return new Stop { Name = name, Number = stopDetails[1] };
        }

        private void Vehicles(HtmlNode pageContent, VirtualTable virtualTable)
        {
            for (int i = 1; i <= 3; i++)
            {
                var xPath = string.Format("//*[@class='arr_title_{0}']", i);
                var foundVehiclesTypes = pageContent.SelectNodes(xPath);
                if (foundVehiclesTypes != null)
                {
                    foreach (var vehicleType in foundVehiclesTypes)
                    {
                        //var vehicleTypeName = vehicleType.SelectSingleNode("//b");
                        var vehiclesXPath = "//*[@class='arr_info_" + i + "']";
                        var vehiclesForType = pageContent.SelectNodes(vehiclesXPath);
                        var vehicles = this.GetVehiclesForType(vehiclesForType);

                        switch (i)
                        {
                            case 1:
                                virtualTable.Buses = vehicles;
                                break;
                            case 2:
                                virtualTable.Trolleys = vehicles;
                                break;
                            case 3:
                                virtualTable.Trams = vehicles;
                                break;
                        }
                    }
                }
            }
        }

        private IEnumerable<VehicleVirtualTable> GetVehiclesForType(HtmlNodeCollection vehiclesForType)
        {
            foreach (var vehicle in vehiclesForType)
            {
                var vehicleVirtualTable = new VehicleVirtualTable();
                vehicleVirtualTable.ScheduleUrl = this.UrlEncode(vehicle.SelectSingleNode("a").Attributes.First(a => a.Name == "href").Value);
                var vehicleDetails = vehicle.InnerText.Split(new char[] { '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries); //[0] = Number(name), [1] = ArrivalsTimes, [2] = Route
                vehicleVirtualTable.ArrivalsTimes = vehicleDetails[1].Split(',');
                vehicleVirtualTable.Route = vehicleDetails[2];
                vehicleVirtualTable.Title = this.CleanNumberTitle(vehicleDetails[0]);
                yield return vehicleVirtualTable;
            }
        }

        private string CleanNumberTitle(string title)
        {
            var builder = new StringBuilder(3);
            foreach (var character in title)
            {
                if (char.IsDigit(character))
                {
                    builder.Append(character);
                }
                else
                {
                    break;
                }
            }

            return builder.ToString();
        }
    }
}