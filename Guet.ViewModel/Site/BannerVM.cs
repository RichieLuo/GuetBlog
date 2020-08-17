using System;
using System.Collections.Generic;
using System.Text;
using Guet.Entities.Site;

namespace Guet.ViewModels.Site
{
    public class BannerVM : EntityBaseVM
    {
        public bool IsShow { get; set; }
        public bool Target { get; set; }
        public string Url { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsBanner { get; set; }
        public BannerVM() { }
        public BannerVM(Banner b)
        {
            if (b != null)
            {
                Id = b.Id;
                IsShow = b.IsShow;
                Target = b.Target;
                Url = b.Url;
                Href = b.Href;
                Title = b.Title;
                Description = b.Description;
                CreateTime = b.CreateTime;
                UpdateTime = b.UpdateTime;
                IsBanner = b.IsBanner;
            }
        }

    }
}
