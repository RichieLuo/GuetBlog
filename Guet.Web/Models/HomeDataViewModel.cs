using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guet.ViewModels.ApplicationOrganization;
using Guet.ViewModels.Blogs;
using Guet.ViewModels.Site;

namespace Guet.Web.Models
{
    /// <summary>
    /// 首页数据
    /// </summary>
    public class HomeDataViewModel
    {
        public SiteSettingVM SiteInfo { get; set; }
        public ApplicationUserVM User { get; set; }
        public List<BannerVM> Banners { get; set; }
        public List<BannerVM> ADs { get; set; }
        public List<ArticleVM> TopTenArticles { get; set; }
        public List<FriendLinkVM> FriendLinks { get; set; }
        /// <summary>
        /// 推荐文章
        /// </summary>
        public List<ArticleVM> ReArticles { get; set; }

        public HomeDataViewModel()
        {
            Banners = new List<BannerVM>();
            ADs = new List<BannerVM>();
            TopTenArticles = new List<ArticleVM>();
            ReArticles = new List<ArticleVM>();
            User = new ApplicationUserVM();
            SiteInfo = new SiteSettingVM();
            FriendLinks = new List<FriendLinkVM>();
        }
    }
}
