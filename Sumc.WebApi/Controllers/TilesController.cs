using Sumc.Models.Response;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Sumc.WebApi.Controllers
{
    public class TilesController : SchedulesController
    {
        // GET api/tiles?tileId=
        public HttpResponseMessage Get(string tileId)
        {
            var scheduleVirtualTable = this.GetVirtualTableByUrlParameter(tileId);
            return GetSecondaryTileXml(scheduleVirtualTable);
        }

        private HttpResponseMessage GetSecondaryTileXml(ScheduleVirtualTable scheduleVirtualTable)
        {

            var tileXml = @"<?xml version=""1.0"" encoding=""utf-8"" ?><tile>
                       <visual>
                         <binding template=""TileWideBlockAndText01"">
                           <text id=""1"">{0}</text>
                           <text id=""2"">{1}</text>
                           <text id=""3"">{2}</text>
                           <text id=""4"">{3}</text>
                           <text id=""5"">{4}</text>
                           <text id=""6"">{5}</text>
                         </binding>   
                         <binding template=""TileSquareBlock"">
                            <text id=""1"">{4}</text>
                            <text id=""2"">{6}</text>
                         </binding> 
                       </visual>
                     </tile>";

            var stop = string.Format("{0} ({1})", scheduleVirtualTable.Stop.Name, scheduleVirtualTable.Stop.Number);
            var times = string.Join(", ", scheduleVirtualTable.RightTimes);
            var tile = string.Format(tileXml,
                stop,
                scheduleVirtualTable.RightTimes.FirstOrDefault(),
                scheduleVirtualTable.RightTimes.Skip(1).FirstOrDefault(),
                scheduleVirtualTable.RightTimes.Skip(2).FirstOrDefault(),
                scheduleVirtualTable.Title,
                scheduleVirtualTable.VehicleType,
                times);

            return new HttpResponseMessage() { Content = new StringContent(tile, Encoding.UTF8, "application/xml") };
        }

        public HttpResponseMessage Get()
        {
            var newsController = new NewsController();
           // var news = newsController.GetNews();
                        
            var tileXml = @"<?xml version=""1.0"" encoding=""utf-8"" ?><tile>
                       <visual>
                         <binding template=""TileWideImageAndText02"">
                          <image id=""1"" src=""image1.png"" alt=""Моят транспорт""/>
                          <text id=""1"">Text Field 1</text>
                          <text id=""2"">Text Field 2</text>
                        </binding>
                         <binding template=""TileSquareBlock"">
                            <text id=""1"">{4}</text>
                            <text id=""2"">{6}</text>
                         </binding> 
                       </visual>
                     </tile>";

            //var firstNews = news
            var tile = string.Format(tileXml);

            return new HttpResponseMessage() { Content = new StringContent(tile, Encoding.UTF8, "application/xml") };
        }
    }
}
