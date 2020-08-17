using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guet.ViewModels.Blogs;
using Guet.ViewModels.Site;

namespace Guet.Web.Models
{
    /// <summary>
    /// 右侧 标签、分类、文章归档
    /// </summary>
    public class LabelsVCViewModels
    {
        /// <summary>
        /// 标签列表
        /// </summary>
        public List<LabelVM> Labels { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public List<CategoryVM> Categories { get; set; }
        /// <summary>
        /// 归档统计
        /// </summary>
        public List<ArticleArchivingVM> ArticleArchivings { get; set; }
        public BannerVM Banner { get; set; }
        public SiteSettingVM Sinfo { get; set; }
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public LabelsVCViewModels()
        {
            Sinfo = new SiteSettingVM();
            Banner = new BannerVM();
            this.Labels = new List<LabelVM>();
            this.Categories = new List<CategoryVM>();
            this.ArticleArchivings = new List<ArticleArchivingVM>();
        }
    }
}
