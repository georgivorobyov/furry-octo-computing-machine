using HtmlAgilityPack;
using Sumc.Models.Response;
using Sumc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Sumc.WebApi.Controllers
{
    public class SchedulesController : BaseApiController
    {
        private readonly ScheduleRepository scheduleRepository;

        public SchedulesController()
        {
            this.scheduleRepository = new ScheduleRepository();
        }

        //ScheduleDirections
        public HttpResponseMessage GetTransportDirections(Sumc.Models.Response.TransportType transportType, string transportNumber)
        {
            return this.PerformOperation(() =>
            {
                var response = this.scheduleRepository.GetTransportDirections(transportType, transportNumber, this.SessionKey, this.GlobalAuthKey);
                var text = this.GetTextFromResponse(response);
                var document = this.GetDocumentReader(text);
                var error = document.DocumentNode.SelectSingleNode("//*[@class='error']");
                var scheduleDirections = new ScheduleDirections();
                if (error != null && !string.IsNullOrWhiteSpace(error.InnerText))
                {
                    scheduleDirections.Error = error.InnerText;
                    return scheduleDirections;
                }

                var directions = document.DocumentNode.SelectNodes("//*[@class='info']");
                scheduleDirections.Directions = this.GetDirections(directions);
                scheduleDirections.TransportType = transportType;
                scheduleDirections.Lid = document.DocumentNode.SelectSingleNode("//input[@name='lid']").Attributes.First(a => a.Name == "value").Value;
                scheduleDirections.Rid = document.DocumentNode.SelectSingleNode("//input[@name='rid']").Attributes.First(a => a.Name == "value").Value;
                scheduleDirections.TransportNumber = transportNumber;
                return scheduleDirections;
            });
        }

        //Schedule
        [ActionName("Transport")]
        public HttpResponseMessage GetTransportSchedule(Sumc.Models.Response.TransportType transportType, string stopValue, string lid, string rid)
        {
            var url = string.Format("?stop={0}&vt={1}&lid={2}&rid={3}", stopValue, (int)transportType, lid, rid);
            var schedule = this.GetByUrlParameters(url);
            return schedule;
        }

        private IEnumerable<ScheduleDirection> GetDirections(HtmlNodeCollection directions)
        {
            foreach (var direction in directions)
            {
                var table = direction.NextSibling.NextSibling;
                var optionElements = table.SelectNodes("self::node()//option");
                var options = this.ToOptions(optionElements);
                var scheduleDirection = new ScheduleDirection();
                scheduleDirection.Direction = direction.InnerText;
                scheduleDirection.Stops = options;
                yield return scheduleDirection;
            }
        }

        private IEnumerable<RouteStopInformation> ToOptions(HtmlNodeCollection optionElements)
        {
            foreach (var option in optionElements)
            {
                var stopDetails = option.NextSibling.InnerText.Split(new char[] { '(', ')' });
                yield return new RouteStopInformation
                {
                    Name = stopDetails[0].Trim('\n', '\t'),
                    Number = stopDetails[1],
                    Value = option.Attributes.First(a => a.Name == "value").Value
                };
            }
        }

        //ScheduleVirtualTable
        [HttpGet]
        [ActionName("VirtualTable")]
        public HttpResponseMessage GetVirtualTable(string urlParameters)
        {
            return this.PerformOperation(() =>
            {
                return GetVirtualTableByUrlParameter(urlParameters);
            });
        }

        protected ScheduleVirtualTable GetVirtualTableByUrlParameter(string urlParameters)
        {
            urlParameters = CleanParameters(urlParameters);
            urlParameters = urlParameters.Replace("stop=", "s=");
            var response = this.scheduleRepository.GetVirtualTableByUrlParameters(urlParameters, null,null);

            //No Cookies

            var text = this.GetTextFromResponse(response);
            var document = this.GetDocumentReader(text);
            var schedule = new ScheduleVirtualTable();
            schedule.ArrivalsTimes = "Not Supported";

            var error = document.DocumentNode.SelectSingleNode("//*[@class='error']");
            if (error != null && !string.IsNullOrWhiteSpace(error.InnerText))
            {
                schedule.Error = error.InnerText;
                return schedule;
            }

            var vehicleInfo = document.DocumentNode.SelectSingleNode("//*[@class='page_title']/b").InnerText.Split(' ');
            var pageContent = document.DocumentNode.SelectSingleNode("//*[@class='page_box']");
            var pageDetails = document.DocumentNode.SelectSingleNode("//*[@class='txt_box']");

            schedule.RightTimes = pageContent.ChildNodes[13].InnerText.Split(',');
            schedule.Information = pageContent.SelectSingleNode("//*[@class='info']").InnerText;
            schedule.Title = vehicleInfo[1];
            schedule.VehicleType = vehicleInfo[0];
            schedule.Route = HttpUtility.HtmlDecode(pageDetails.ChildNodes[6].InnerText).Trim('\t', '\n', ' ', '"');
            schedule.Stop = this.GetStop(pageDetails.ChildNodes[2]);
            this.Vehicles(pageContent, schedule);
            return schedule;
        }

        private string CleanParameters(string urlParameters)
        {
            var parameterIndex = urlParameters.IndexOf('?');
            if (parameterIndex > 0)
            {
                urlParameters = urlParameters.Substring(parameterIndex);
            }

            urlParameters = this.UrlDecode(urlParameters);
            return urlParameters;
        }

        private void Vehicles(HtmlNode pageContent, ScheduleVirtualTable virtualTable)
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
                var vehicleDetails = vehicle.InnerText.Split(new char[] { '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                vehicleVirtualTable.ArrivalsTimes = vehicleDetails[1].Split(',');
                vehicleVirtualTable.Route = vehicleDetails[2];
                vehicleVirtualTable.Title = vehicleDetails[0];
                yield return vehicleVirtualTable;
            }
        }

        // IEnumerable<string>
        [HttpGet]
        [ActionName("UrlNextArriveTimes")]
        public HttpResponseMessage GetNextArriveTimes(string urlParameters, DateTime from)
        {
            return this.PerformOperation(() =>
            {
                urlParameters = this.CleanParameters(urlParameters);
                urlParameters = urlParameters.Replace("s=", "stop=");
                var date = from.AddHours(3);
                if (date.Hour < 2)
                {
                    date = new DateTime(2013, 05, 05, 23, 0, 0);
                }

                var response = this.scheduleRepository.GetByUrlParameters(urlParameters, date, this.SessionKey, this.GlobalAuthKey);

                var text = this.GetTextFromResponse(response);
                var document = this.GetDocumentReader(text);
                var pageContent = document.DocumentNode.SelectSingleNode("//*[@class='page_cnt']");
                return this.GetArrivalsTimes(pageContent);
            });
        }

        //IEnumerable<string> 
        [HttpGet]
        [ActionName("TransportNextArriveTimes")]
        public HttpResponseMessage GetNextArriveTimes(Sumc.Models.Response.TransportType transportType, string stopValue, string lid, string rid, DateTime from)
        {
            var url = string.Format("?stop={0}&vt={1}&lid={2}&rid={3}", stopValue, (int)transportType, lid, rid);
            return this.GetNextArriveTimes(url, from);
        }

        //Schedule
        [HttpGet]
        [ActionName("Url")]
        public HttpResponseMessage GetByUrlParameters(string urlParameters)
        {
            return this.PerformOperation(() =>
            {
                urlParameters = this.CleanParameters(urlParameters);
                urlParameters = urlParameters.Replace("s=", "stop=");
                var response = this.scheduleRepository.GetByUrlParameters(urlParameters, this.SessionKey, this.GlobalAuthKey);
                //No Cookies
                var text = this.GetTextFromResponse(response);
                var document = this.GetDocumentReader(text);
                var schedule = new Schedule();

                var errors = document.DocumentNode.SelectNodes("//*[@class='error']");
                if (errors != null)
                {
                    foreach (var error in errors)
                    {
                        if (!string.IsNullOrWhiteSpace(error.InnerText))
                        {
                            schedule.Error = error.InnerText;
                            schedule.IsFound = false;
                            return schedule;
                        }
                    }
                }

                schedule.IsFound = true;

                var vehicleInfo = document.DocumentNode.SelectSingleNode("//*[@class='page_title'][2]/b").InnerText.Split(' ');
                var pageContent = document.DocumentNode.SelectSingleNode("//*[@class='page_cnt']");

                schedule.Title = vehicleInfo[1];
                var transportTypeString = urlParameters.Substring(urlParameters.IndexOf("vt=") + 3, 1);
                schedule.VehicleType = (Sumc.Models.Response.TransportType)int.Parse(transportTypeString);// vehicleInfo[0];
                schedule.Route = pageContent.ChildNodes[7].InnerText.Trim('\n', '\t');
                schedule.RightTimeUrl = this.UrlEncode(pageContent.ChildNodes.First(x => x.Name == "a").Attributes.First(a => a.Name == "href").Value);
                schedule.ArrivalsTimes = this.GetArrivalsTimes(pageContent);
                schedule.Stops = this.GetStops(pageContent, urlParameters);
                schedule.Stop = this.GetStop(pageContent.ChildNodes[2]);
                return schedule;
            });
        }

        private Stop GetStop(HtmlNode pageContent)
        {
            var stopDetails = pageContent.InnerText.Split(new char[] { '(', ')' });
            return new Stop { Name = stopDetails[0].Trim('\n', '\t'), Number = stopDetails[1].Trim('\n', '\t') };
        }

        private IEnumerable<ExtendedStop> GetStops(HtmlNode pageContent, string currentStop)
        {
            var stops = pageContent.SelectNodes("self::node()//li");
            foreach (var stop in stops)
            {
                var stopDetails = stop.InnerText.Split(new char[] { '(', ')' });
                var urlElement = stop.SelectSingleNode("a");
                string url = null;
                if (urlElement != null)
                {
                    url = urlElement.Attributes.First(x => x.Name == "href").Value;
                }

                if (url == null)
                {
                    url = currentStop;
                }

                yield return new ExtendedStop { Name = stopDetails[0].Trim('\n', '\t'), Number = stopDetails[1].Trim('\n', '\t'), ScheduleUrl = url };
            }
        }

        private IEnumerable<string> GetArrivalsTimes(HtmlNode pageContent)
        {
            var arrivalTimes = pageContent.SelectNodes("self::node()//td")
                .Where(td => td.SelectSingleNode("b") == null)
                .Select(td => this.CleanValue(td.InnerText));

            var arrivalTimesList = new List<DateTime>();
            foreach (var time in arrivalTimes)
            {
                DateTime arrivalTime;
                if (DateTime.TryParse(time, out arrivalTime))
                {
                    arrivalTimesList.Add(arrivalTime);
                }
            }

            arrivalTimesList.Sort();
            foreach (var arrivalTime in arrivalTimesList)
            {
                yield return arrivalTime.ToString("HH:mm");
            }
        }

        private string CleanValue(string value)
        {
            return value.Trim('\n', '\t').Replace("&nbsp", "").Replace(";", "");
        }
    }
}