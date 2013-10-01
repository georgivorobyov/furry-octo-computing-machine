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
                new MainStop { Name = "ПЛ. ОРЛОВ МОСТ", TransportNumbers = new string[] { "4", "5","8","11","76", "84","204","213","280","306","604" } },
                new MainStop { Name = "БУЛ. ГОЦЕ ДЕЛЧЕВ", TransportNumbers = new string[] { "64" }},
                new MainStop { Name = "ГАРА ПОДУЯНЕ", TransportNumbers = new string[] { "11", "72", "75", "404" } },
               // new MainStop { Name = "ДЪРЖАВНА ПЕЧАТНИЦА", TransportNumbers = new string[] { "64" } },
                new MainStop { Name = "Ж.К. КРАСНО СЕЛО", TransportNumbers = new string[] { "64", "260", "83", "102"} },
                new MainStop { Name = "УМБАЛ СВ. АННА", TransportNumbers = new string[] { "5","8","84","306" } },
                new MainStop { Name = "КВ. ОРЛАНДОВЦИ", TransportNumbers = new string[] { "100" } },
                new MainStop { Name = "ЛЕТИЩЕ СОФИЯ ТЕРМИНАЛ 2", TransportNumbers = new string[] { "84" } },
                new MainStop { Name = "К.ВЕЛИЧКОВ", TransportNumbers = new string[] { "3", "11", "19", "22" } },
               // new MainStop { Name = "МЛАДОСТ 1" },
                new MainStop { Name = "ЦАРИГРАДСКО ШОСЕ", TransportNumbers = new string[] { "1", "3", "5", "6" } },
                new MainStop { Name = "СЛИВНИЦА", TransportNumbers = new string[] { "81", "309" } },
                new MainStop { Name = "ПЛ. ЛЪВОВ МОСТ", TransportNumbers = new string[] { "11", "78", "85", "86", "213", "285", "305", "404", "413" } },
                new MainStop { Name = "ПЛ. МАКЕДОНИЯ", TransportNumbers = new string[] { "4", "5" } },
                new MainStop { Name = "ПЛ. СТОЧНА ГАРА", TransportNumbers = new string[] { "11", "78","85","86" } },
                new MainStop { Name = "ПЛ. ЦЕНТРАЛНА ГАРА", TransportNumbers = new string[] { "78","85","213","285","305","404","413" } },
                new MainStop { Name = "РУМЪНСКО ПОСОЛСТВО", TransportNumbers = new string[] { "120","305","413" } },
                new MainStop { Name = "СБАЛ ПО ОНКОЛОГИЯ", TransportNumbers = new string[] { "69", "280", "294" } },
                new MainStop { Name = "СЕМИНАРИЯТА", TransportNumbers = new string[] { "67" } },
                new MainStop { Name = "СУ КЛИМЕНТ ОХРИДСКИ", TransportNumbers = new string[] { "9", "280","306" } },
                new MainStop { Name = "УЛ. ГЕН. ГУРКО", TransportNumbers = new string[] { "1","2","5","8" } },
                new MainStop { Name = "УЛ. ЙЕРУСАЛИМ", TransportNumbers = new string[] { "5","306","384" } },
                new MainStop { Name = "ХОТЕЛ ПЛИСКА", TransportNumbers = new string[] { "72" } },
                new MainStop { Name = "ЦЕНТЪР ПО ХИГИЕНА", TransportNumbers = new string[] { "2", "8", "9", "64", "74", "604" } },
            };
        }
    }
}