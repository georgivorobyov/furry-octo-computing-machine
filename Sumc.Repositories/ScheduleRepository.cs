using System;
using System.Net;

namespace Sumc.Repositories
{
    public class ScheduleRepository : BaseRequestRepository
    {
        public HttpWebResponse GetByUrlParameters(string urlParameters, Cookie sessionToken, Cookie authToken)
        {
            var url = string.Format("{0}schedules/vehicle{1}", BaseSofiaTrafficUrl, urlParameters);
            return HttpRequester.Get(url, sessionToken, authToken);
        }

        public HttpWebResponse GetVirtualTableByUrlParameters(string urlParameters, Cookie sessionToken, Cookie authToken)
        {
            var url = string.Format("{0}schedules/vehicle-vt{1}", BaseSofiaTrafficUrl, urlParameters);
            return HttpRequester.Get(url, sessionToken, authToken);
        }

        public HttpWebResponse GetTransportDirections(Sumc.Models.Response.TransportType transportType, string transportNumber, Cookie sessionToken, Cookie authToken)
        {
            var url = string.Format("{0}schedules?tt={1}&ln={2}&s=Търсене", BaseSofiaTrafficUrl, (int)transportType, transportNumber);
            return HttpRequester.Get(url, sessionToken, authToken);
        }

        public HttpWebResponse GetByUrlParameters(string urlParameters, DateTime from, Cookie sessionToken, Cookie authToken)
        {
            var url = string.Format("{0}schedules/vehicle{1}&h={2}", BaseSofiaTrafficUrl, urlParameters, from.Hour);
            return HttpRequester.Get(url, sessionToken, authToken);
        }
    }
}
