using System;

namespace Sumc.Models.Response
{
    public abstract class BaseNews
    {
        public string Title { get; set; }

        public DateTime Date { get; set; }
    }
}
