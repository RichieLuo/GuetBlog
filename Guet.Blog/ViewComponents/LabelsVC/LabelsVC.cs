using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guet.DataAccess;
using Guet.Entities.Blogs;
using Guet.Entities.Site;
using Guet.ViewModels.Blogs;
using Guet.ViewModels.Site;
using Guet.Web.Models;

namespace Guet.Web.ViewComponents.LabelsVC
{
    /// <summary>
    /// 首页右侧标签
    /// </summary>
    public class LabelsVC : ViewComponent
    {
        private readonly IEntityRepository<Category> _categoryRepository;
        private readonly IEntityRepository<Label> _labelRepository;
        private readonly IEntityRepository<SiteSetting> _siteSettingRepository;
        private readonly IEntityRepository<Banner> _bRepository;
        public LabelsVC(
            IEntityRepository<Category> categoryRepository,
            IEntityRepository<Label> labelRepository,
                 IEntityRepository<SiteSetting> siteSettingRepository,
            IEntityRepository<Banner> bRepository)
        {
            _categoryRepository = categoryRepository;
            _labelRepository = labelRepository;
            _siteSettingRepository = siteSettingRepository;
            _bRepository = bRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var catesVM = new List<CategoryVM>();
            var labelsVM = new List<LabelVM>();

            var cates = await _categoryRepository.GetAllAsyn();
            var labels = await _labelRepository.GetAllAsyn();
            var banner = await _bRepository.FindByAsyn(x => x.IsBanner == false && x.IsShow == true);
            var siteInfo = await _siteSettingRepository.GetAllAsyn();

            foreach (var item in cates)
            {
                catesVM.Add(new CategoryVM(item));
            }
            foreach (var item in labels)
            {
                labelsVM.Add(new LabelVM(item));
            }
            return View(new LabelsVCViewModels()
            {
                Sinfo = new SiteSettingVM(siteInfo.FirstOrDefault()),
                Banner = new BannerVM(banner.FirstOrDefault()),
                Categories = catesVM,
                Labels = labelsVM
            });
        }
    }
}