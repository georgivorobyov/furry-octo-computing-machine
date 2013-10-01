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
        public IEnumerable<MainStop> GetMainStops()
        {
            return new MainStop[]
            {
                new MainStop { Name = "ПЛ.ОРЛОВ МОСТ", TransportNumbers = new string[] { "4", "5","8","11","76", "84","204","213","280","306","604" } },
                new MainStop { Name = "БУЛ.ГОЦЕ ДЕЛЧЕВ", TransportNumbers = new string[] { "9", "2", "204" }},
                new MainStop { Name = "ГАРА ПОДУЯНЕ", TransportNumbers = new string[] { "11", "72", "75", "404" } },
                new MainStop { Name = "НДК", TransportNumbers = new string[] { "1", "7" } },
                new MainStop { Name = "Ж.К. КРАСНО СЕЛО", TransportNumbers = new string[] { "5", "4", "64", "260", "83", "102"} },
                new MainStop { Name = "УМБАЛ СВ.АННА", TransportNumbers = new string[] { "1","3", "5","6","76","84","204","213","305","306","604" } },
                new MainStop { Name = "КВ. ОРЛАНДОВЦИ", TransportNumbers = new string[] { "100", "22", "23","24" } },
                new MainStop { Name = "ЛЕТИЩЕ СОФИЯ ТЕРМИНАЛ 2", TransportNumbers = new string[] { "84", "384" } },
                new MainStop { Name = "К.ВЕЛИЧКОВ", TransportNumbers = new string[] { "3", "11", "19", "22", "83" } },
                new MainStop { Name = "МЛАДОСТ 1", TransportNumbers = new string[] { "5", "111", "4", "113" } },
                new MainStop { Name = "ЦАРИГРАДСКО ШОСЕ", TransportNumbers = new string[] { "1", "3", "5", "6", "114" } },
                new MainStop { Name = "СЛИВНИЦА", TransportNumbers = new string[] { "1","3","7","6","4","81", "309" } },
                new MainStop { Name = "ПЛ.ЛЪВОВ МОСТ", TransportNumbers = new string[] { "11", "78", "85", "86", "213", "285", "305", "404", "413" } },
                new MainStop { Name = "ПЛ. МАКЕДОНИЯ", TransportNumbers = new string[] { "4", "5" } },
                new MainStop { Name = "ПЛ. СТОЧНА ГАРА", TransportNumbers = new string[] { "6", "7","85","86" } },
                new MainStop { Name = "ПЛ.ЦЕНТРАЛНА ГАРА", TransportNumbers = new string[] { "1","3","7","6","4","213","285","305","404","413" } },
                new MainStop { Name = "РУМЪНСКО ПОСОЛСТВО", TransportNumbers = new string[] { "120","305","413" } },
                new MainStop { Name = "СБАЛ ПО ОНКОЛОГИЯ", TransportNumbers = new string[] { "67", "69", "70", "88", "413", "280", "294" } },
                new MainStop { Name = "СЕМИНАРИЯТА", TransportNumbers = new string[] { "18" } },
                new MainStop { Name = "КЛИМЕНТ ОХРИДСКИ", TransportNumbers = new string[] { "94", "84", "280","306" } },
                new MainStop { Name = "УЛ. ГЕН. ГУРКО", TransportNumbers = new string[] { "9","94","5","84", "8", "2", "1" } },
                new MainStop { Name = "УЛ.ЙЕРУСАЛИМ", TransportNumbers = new string[] { "4", "5", "88","113", "306","384" } },
                new MainStop { Name = "ХОТЕЛ ПЛИСКА", TransportNumbers = new string[] { "280","76","204","213","305","306","604","84","4","11","5","8" } },
                new MainStop { Name = "ЦЕНТЪР ПО ХИГИЕНА", TransportNumbers = new string[] { "2", "8", "9", "64", "74", "604" } },
            };
        }
    }
}