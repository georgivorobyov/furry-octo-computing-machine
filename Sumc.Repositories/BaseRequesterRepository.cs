using System.IO;
using System.Net;

namespace Sumc.Repositories
{
    public class BaseRequesterRepository<T> : BaseRepository<T> where T: class
    {
        public const string BaseSofiaTrafficUrl = "http://m.sofiatraffic.bg/";
    }
}
