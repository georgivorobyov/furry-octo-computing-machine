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
    <binding template=""TileWideText01"">
        <text id=""1"">{5} {4}</text>
        <text id=""2"">{0}</text>
        <text id=""3""></text>
        <text id=""4"">{1}, {2}, {3}</text>
        <text id=""5""></text>
    </binding>
    <binding template=""TileSquareText01"">
        <text id=""1"">{4} {5}</text>
        <text id=""2"">{1}</text>
        <text id=""3"">{2}</text>
        <text id=""4"">{3}</text>
    </binding>
</visual>
</tile>";

            var stop = string.Format("{0} ({1})", scheduleVirtualTable.Stop.Name, scheduleVirtualTable.Stop.Number);
            var tile = string.Format(tileXml,
                stop,
                scheduleVirtualTable.RightTimes.FirstOrDefault(),
                scheduleVirtualTable.RightTimes.Skip(1).FirstOrDefault(),
                scheduleVirtualTable.RightTimes.Skip(2).FirstOrDefault(),
                scheduleVirtualTable.Title,
                scheduleVirtualTable.VehicleType);

            return new HttpResponseMessage() { Content = new StringContent(tile, Encoding.UTF8, "application/xml") };
        }

        public HttpResponseMessage GetNews(int number)
        {
            var newsController = new NewsController();
            var news = newsController.GetNewsList().Skip(number).FirstOrDefault();
                        
            var tileXml = @"<?xml version=""1.0"" encoding=""utf-8"" ?><tile>
                       <visual>
                        <binding template=""TileWidePeekImage03"">
                          <image id=""1"" src=""ms-appx:///Assets/WideLogo.scale-100.png"" alt=""Моят транспорт""/>
                          <text id=""1"">{0}</text>
                        </binding>
                       </visual>
                     </tile>";

            var tile = string.Format(tileXml, news.Title);

            return new HttpResponseMessage() { Content = new StringContent(tile, Encoding.UTF8, "application/xml") };
        }
    }
}
