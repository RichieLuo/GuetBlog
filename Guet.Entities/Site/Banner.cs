using System;
using System.Collections.Generic;
using System.Text;

namespace Guet.Entities.Site
{
    public class Banner : EntityBase
    {
        public bool IsShow { get; set; }
        public bool Target { get; set; }
        public string Url { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } 
        public bool IsBanner { get; set; }
    }
}
