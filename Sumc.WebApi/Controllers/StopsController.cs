using Sumc.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sumc.WebApi.Controllers
{
    public class StopsController : ApiController
    {
        public IEnumerable<Stop> GetMainStops()
        {
            return new Stop[]
            {
                new Stop { Name = "АВТОСТАНЦИЯ ХЛАДИЛНИКА" },
                new Stop { Name = "БУЛ. ГОЦЕ ДЕЛЧЕВ" },
                new Stop { Name = "ГАРА ПОДУЯНЕ" },
                new Stop { Name = "ДЪРЖАВНА ПЕЧАТНИЦА" },
                new Stop { Name = "Ж.К. КРАСНО СЕЛО" },
                new Stop { Name = "КВ. ОРЛАНДОВЦИ" },
                new Stop { Name = "ЛЕТИЩЕ СОФИЯ ТЕРМИНАЛ 2" },
                new Stop { Name = "МЕТРОСТАНЦИЯ К.ВЕЛИЧКОВ" },
                new Stop { Name = "МЕТРОСТАНЦИЯ МЛАДОСТ 1" },
                new Stop { Name = "МЕТРОСТАНЦИЯ СЛИВНИЦА" },
                new Stop { Name = "МЕТРОСТАНЦИЯ ЦАРИГРАДСКО ШОСЕ" },
                new Stop { Name = "ПЛ. ЛЪВОВ МОСТ" },
                new Stop { Name = "ПЛ. МАКЕДОНИЯ" },
                new Stop { Name = "ПЛ. ОРЛОВ МОСТ" },
                new Stop { Name = "ПЛ. СТОЧНА ГАРА" },
                new Stop { Name = "ПЛ. ЦЕНТРАЛНА ГАРА" },
                new Stop { Name = "РУМЪНСКО ПОСОЛСТВО" },
                new Stop { Name = "СБАЛ ПО ОНКОЛОГИЯ" },
                new Stop { Name = "СЕМИНАРИЯТА" },
                new Stop { Name = "СУ КЛИМЕНТ ОХРИДСКИ" },
                new Stop { Name = "УЛ. ГЕН. ГУРКО" },
                new Stop { Name = "УЛ. ЙЕРУСАЛИМ" },
                new Stop { Name = "УМБАЛ СВ. АННА" },
                new Stop { Name = "ХОТЕЛ ПЛИСКА" },
                new Stop { Name = "ЦЕНТЪР ПО ХИГИЕНА" },
            };
        }
    }
}