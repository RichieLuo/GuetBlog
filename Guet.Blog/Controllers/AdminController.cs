using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Guet.DataAccess;
using Guet.Entities.ApplicationOrganization;
using Guet.Entities.Blogs;
using Guet.Entities.Site;
using Guet.ViewModels.ApplicationOrganization;
using Guet.ViewModels.Blogs;
using Guet.ViewModels.Common;
using Guet.ViewModels.Site;
using Guet.Web.Models;

namespace Guet.Web.Controllers
{
    /// <summary>
    /// 后台管理
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly IEntityRepository<Article> _articleRepository;
        private readonly IEntityRepository<Category> _categoryRepository;
        private readonly IEntityRepository<Label> _labelRepository;
        private readonly IEntityRepository<ArticleAndLabel> _articleAndLabelRepository;
        private readonly IEntityRepository<SiteSetting> _siteSettingRepository;
        private readonly IEntityRepository<Banner> _bRepository;
        private readonly IEntityRepository<Message> _mRepository;
        private readonly IEntityRepository<FriendLink> _fRepository;
        private readonly IEntityRepository<About> _aRepository;
        public AdminController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IEntityRepository<Article> articleRepository,
            IEntityRepository<Category> categoryRepository,
            IEntityRepository<Label> labelRepository,
            IEntityRepository<SiteSetting> siteSettingRepository,
            IEntityRepository<Banner> bRepository,
            IEntityRepository<Message> mRepository,
            IEntityRepository<FriendLink> fRepository,
            IEntityRepository<ArticleAndLabel> articleAndLabelRepository,
            IEntityRepository<About> aRepository
            ) : base(userManager, signInManager, roleManager)
        {
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _labelRepository = labelRepository;
            _articleAndLabelRepository = articleAndLabelRepository;
            _siteSettingRepository = siteSettingRepository;
            _bRepository = bRepository;
            _mRepository = mRepository;
            _fRepository = fRepository;
            _aRepository = aRepository;
        }


        /// <summary>
        /// 后台管理首页（仪表盘|欢迎页）
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 站点设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> SiteSettings()
        {
            var setting = await _siteSettingRepository.GetAllAsyn();
            var sVM = new SiteSettingVM(setting.FirstOrDefault());
            return PartialView(sVM);
        }


        /// <summary>
        /// 修改站点信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateSiteSetting(SiteSettingVM model)
        {
            var r = new DataResultViewModel<SiteSettingVM>();
            var siteSetting = await _siteSettingRepository.GetSingleAsyn(model.Id);
            if (siteSetting != null)
            {
                siteSetting.Title = model.Title;
                siteSetting.Description = model.Description;
                siteSetting.Keywords = model.Keywords;
                siteSetting.Copyright = model.Copyright;
                siteSetting.ICP = model.ICP;
                siteSetting.Logo = model.Logo;
                siteSetting.UpdateTime = DateTime.Now;
                siteSetting.QrCode = model.QrCode;
                siteSetting.UseImgLogo = model.UseImgLogo;
                siteSetting.Statistics = model.Statistics;
                var status = await _siteSettingRepository.AddOrEditAndSaveAsyn(siteSetting);
                if (status)
                {
                    r.Code = 200;
                    r.Status = true;
                    r.Msg = "修改成功";
                }
            }
            return Json(r);
        }


        public async Task<IActionResult> About()
        {
            var about = await _aRepository.GetAllAsyn();
            return PartialView(new AboutVM(about.FirstOrDefault()));
        }

        /// <summary>
        /// 更新关于我
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateAbout(AboutVM m)
        {
            var r = new ResultViewModel();
            var about = await _aRepository.GetSingleAsyn(m.Id);
            if (about != null)
            {
                about.Content = m.Content;
                about.UpdateTime = DateTime.Now;
                var ok = await _aRepository.AddOrEditAndSaveAsyn(about);
                if (ok)
                {
                    r.Code = 200;
                    r.Status = true;
                    r.Msg = "修改保存成功！";
                }
            }
            return Json(r);
        }


        public IActionResult FriendLink()
        {
            return PartialView();
        }

        /// <summary>
        /// 友情链接列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetFriendLinks(ParamsInputViewModel param)
        {
            var r = new DataResultViewModel<List<FriendLinkVM>>();
            r.Data = new List<FriendLinkVM>();
            var query = await _fRepository.GetAllAsyn();
            var links = query.Skip(param.Limit * (param.Index - 1)).Take(param.Limit);
            if (links.Any())
            {
                int orderNumber = param.Limit * (param.Index - 1) + 1;
                foreach (var link in links)
                {
                    var vm = new FriendLinkVM(link);
                    vm.OrderNumber = (orderNumber++).ToString();
                    r.Data.Add(vm);
                }
                r.Code = 200;
                r.Status = true;
                r.Msg = "查询成功！";
            }

            return Json(r);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddFriendLink(FriendLinkVM model)
        {
            var r = new DataResultViewModel<FriendLinkVM>();
            var friendLink = new FriendLink()
            {
                Name = model.Name,
                Url = model.Url,
                Cover = model.Cover,
                IsEnable = model.IsEnable
            };

            var ok = await _fRepository.AddOrEditAndSaveAsyn(friendLink);
            if (ok)
            {
                r.Code = 200;
                r.Status = true;
                r.Msg = "添加成功";
            }
            return Json(r);
        }

        /// <summary>
        /// 更新 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateFriendLink(FriendLinkVM m)
        {
            var r = new ResultViewModel();
            var data = await _fRepository.GetSingleAsyn(m.Id);
            if (data != null)
            {
                data.Name = m.Name;
                data.Url = m.Url;
                data.Cover = m.Cover;
                var ok = await _fRepository.AddOrEditAndSaveAsyn(data);
                if (ok)
                {
                    r.Code = 200;
                    r.Status = true;
                    r.Msg = "修改保存成功！";
                }
            }
            return Json(r);
        }


        /// <summary>
        /// 链接数量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> GetFriendLinksCount()
        {
            var query = await _fRepository.GetAllAsyn();
            return query.Count();
        }

        /// <summary>
        /// 禁用链接
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DisEnableFriendLink(DisEnableFriendLinkInput m)
        {
            var r = new ResultViewModel();
            var link = await _fRepository.GetSingleAsyn(m.Id);
            if (link != null)
            {
                link.IsEnable = m.Enable;
                var ok = await _fRepository.AddOrEditAndSaveAsyn(link);
                if (ok)
                {
                    r.Code = 200;
                    r.Status = true;
                    r.Msg = "操作成功";
                }
            }
            return Json(r);
        }

        /// <summary>
        /// 删除链接
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteFriendLink(Guid id)
        {
            var r = new ResultViewModel();
            var link = await _fRepository.GetSingleAsyn(id);
            if (link != null)
            {
                var ok = await _fRepository.DeleteAndSaveAsyn(id);
                if (ok.DeleteSatus)
                {
                    r.Code = 200;
                    r.Status = true;
                    r.Msg = "操作成功";
                }
            }
            return Json(r);
        }


        #region 轮播图


        public IActionResult Banner()
        {
            return PartialView();
        }
        /// <summary>
        /// 轮播图
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetBanners(ParamsInputViewModel param)
        {
            var r = new DataResultViewModel<List<BannerVM>>();
            r.Data = new List<BannerVM>();
            var query = await _bRepository.GetAllAsyn();
            var datas = query.Skip(param.Limit * (param.Index - 1)).Take(param.Limit);
            if (datas.Any())
            {
                int orderNumber = param.Limit * (param.Index - 1) + 1;
                foreach (var data in datas)
                {
                    var vm = new BannerVM(data);
                    vm.OrderNumber = (orderNumber++).ToString();
                    r.Data.Add(vm);
                }
                r.Code = 200;
                r.Status = true;
                r.Msg = "查询成功！";
            }

            return Json(r);
        }

        /// <summary>
        /// 单条
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetBannerById(Guid id)
        {
            var link = await _bRepository.GetSingleAsyn(id);
            var vm = new BannerVM(link);
            return Json(vm);
        }


        /// <summary>
        /// 更新 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateBanner(BannerVM m)
        {
            var r = new ResultViewModel();
            var data = await _bRepository.GetSingleAsyn(m.Id);
            if (data != null)
            {
                data.Title = m.Title;
                data.Description = m.Description;
                data.Url = m.Url;
                data.Href = m.Href;
                data.Target = m.Target;
                data.IsShow = m.IsShow;
                data.IsBanner = data.IsBanner;

                var ok = await _bRepository.AddOrEditAndSaveAsyn(data);
                if (ok)
                {
                    r.Code = 200;
                    r.Status = true;
                    r.Msg = "修改保存成功！";
                }
            }
            return Json(r);
        }

        /// <summary>
        /// 数量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> GetBannersCount()
        {
            var query = await _bRepository.GetAllAsyn();
            return query.Count();
        }

        /// <summary>
        /// 禁用 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DisEnableBanner(DisEnableFriendLinkInput m)
        {
            var r = new ResultViewModel();
            var data = await _bRepository.GetSingleAsyn(m.Id);
            if (data != null)
            {
                data.IsShow = m.Enable;
                var ok = await _bRepository.AddOrEditAndSaveAsyn(data);
                if (ok)
                {
                    r.Code = 200;
                    r.Status = true;
                    r.Msg = "操作成功";
                }
            }
            return Json(r);
        }

        /// <summary>
        /// 删除 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteBanner(Guid id)
        {
            var r = new ResultViewModel();
            var data = await _bRepository.GetSingleAsyn(id);
            if (data != null)
            {
                var ok = await _bRepository.DeleteAndSaveAsyn(id);
                if (ok.DeleteSatus)
                {
                    r.Code = 200;
                    r.Status = true;
                    r.Msg = "操作成功";
                }
            }
            return Json(r);
        }

        #endregion


        #region 文章
        /// <summary>
        /// 文章页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult Articles()
        {
            return PartialView();
        }

        /// <summary>
        /// 文章列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetArticles(ParamsInputViewModel param)
        {
            var r = new DataResultViewModel<List<ArticleVM>>();
            r.Data = new List<ArticleVM>();
            var query = await _articleRepository.GetAllAsyn();
            var articles = query.Skip(param.Limit * (param.Index - 1)).Take(param.Limit);
            if (articles.Any())
            {
                int orderNumber = param.Limit * (param.Index - 1) + 1;
                foreach (var article in articles.OrderByDescending(x => x.CreateTime))
                {
                    var aVM = new ArticleVM(article);
                    aVM.OrderNumber = (orderNumber++).ToString();
                    var labels = await _articleAndLabelRepository.FindByAsyn(x => x.ArticleId == article.Id);
                    foreach (var item in labels)
                    {
                        var label = await _labelRepository.GetSingleAsyn(item.LabelId);
                        if (label != null)
                        {
                            aVM.Labels.Add(new LabelVM(label));
                        }
                    }
                    var category = await _categoryRepository.GetSingleAsyn(article.CategoryId);
                    if (category != null)
                    {
                        aVM.Category = new CategoryVM(category);
                    }
                    r.Data.Add(aVM);
                }
                r.Code = 200;
                r.Status = true;
                r.Msg = "请求成功";
            }
            return Json(r);
        }


        /// <summary>
        /// 添加文章页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AddArticle(Guid? id)
        {
            var cates = await _categoryRepository.GetAllAsyn();
            var labels = await _labelRepository.GetAllAsyn();
            var catesVM = new List<CategoryVM>();
            var labelsVM = new List<LabelVM>();

            foreach (var item in cates)
            {
                catesVM.Add(new CategoryVM(item));
            }
            foreach (var item in labels)
            {
                labelsVM.Add(new LabelVM(item));
            }
            ViewBag.Categories = catesVM;
            ViewBag.Labels = labelsVM;

            if (id != null && id.HasValue)
            {
                var article = await _articleRepository.GetSingleAsyn(id.Value);
                if (article != null)
                {
                    var articleVM = new ArticleVM(article); //文章
                    var category = await _categoryRepository.GetSingleAsyn(article.CategoryId);

                    var categoryVM = new CategoryVM(category);//分类
                    var lavelsVM = new List<LabelVM>();//标签
                    var hasLabels = await _articleAndLabelRepository.FindByAsyn(x => x.ArticleId == article.Id);
                    foreach (var item in hasLabels)
                    {
                        var label = await _labelRepository.GetSingleAsyn(item.LabelId);
                        lavelsVM.Add(new LabelVM(label));
                    }

                    return PartialView(new AddArticleVM()
                    {
                        IsNew = false,
                        Article = articleVM,
                        LabelsVM = lavelsVM,
                        CategoryVM = categoryVM
                    });
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            return PartialView(new AddArticleVM() { IsNew = true });
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> AddArticle(AddArticleVM model)
        {
            try
            { 
                var user = await GetUser(User.Identity.Name);
                Article article = null;
                var r = new DataResultViewModel<ArticleVM>();
                if (model.IsNew)
                {
                    article = new Article()
                    {
                        Title = model.Article.Title,
                        Abstract = model.Article.Abstract,
                        CategoryId = model.CategoryId,
                        Content = model.Article.Content,
                        Cover = string.IsNullOrEmpty(model.Article.Cover) ? "/images/main/article_image_002.jpg" : model.Article.Cover,
                        UpdateTime = DateTime.Now,
                        IsOwn = model.Article.IsOwn,
                        IsNotOwnDesctiption = model.Article.IsNotOwnDesctiption,
                        UserId = Guid.Parse(user.Id),
                        IsRecommend = model.Article.IsRecommend
                    };
                    r.Msg = "文章添加成功！";
                }
                else
                {
                    article = await _articleRepository.GetSingleAsyn(model.Article.Id);
                    article.Title = model.Article.Title;
                    article.Abstract = model.Article.Abstract;
                    article.CategoryId = model.CategoryId;
                    article.Content = model.Article.Content;
                    article.Cover = string.IsNullOrEmpty(model.Article.Cover) ? "/images/main/article_image_002.jpg" : model.Article.Cover;
                    article.UpdateTime = DateTime.Now;
                    article.CreateTime = article.CreateTime;
                    article.IsRecommend = model.Article.IsRecommend;
                    article.IsOwn = model.Article.IsOwn;
                    article.IsNotOwnDesctiption = model.Article.IsNotOwnDesctiption;
                    r.Msg = "文章更新成功！";
                }
                var ok = await _articleRepository.AddOrEditAndSaveAsyn(article);
                if (ok)
                {
                    var deleteLabels = await _articleAndLabelRepository.FindByAsyn(x => x.ArticleId == model.Article.Id);
                    foreach (var item in deleteLabels)
                    {
                        await _articleAndLabelRepository.DeleteAndSaveAsyn(item.Id);
                    }
                    //添加标签
                    if (model.Labels?.Count > 0)
                    {
                        model.Labels = model.Labels.Distinct().ToList();
                        foreach (var labelId in model.Labels)
                        {
                            await _articleAndLabelRepository.AddOrEditAndSaveAsyn(new ArticleAndLabel()
                            {
                                ArticleId = article.Id,
                                LabelId = labelId
                            });
                        }
                    }

                    r.Code = ok ? 200 : default;
                    r.Status = ok;
                    r.Msg = ok ? r.Msg : "操作失败！";
                }
                return Json(r);
            }
            catch (Exception ex)
            {
                return Json(new BaseResult() { Code = 500, Status = false, Msg = "操作失败，请重试！" });
            }
        }


        #region 标签管理

        public IActionResult Label()
        {
            return View();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetLabels(ParamsInputViewModel param)
        {
            var r = new DataResultViewModel<List<LabelVM>>();
            r.Data = new List<LabelVM>();
            var query = await _labelRepository.GetAllAsyn();
            var data = query.Skip(param.Limit * (param.Index - 1)).Take(param.Limit);
            if (data.Any())
            {
                int orderNumber = param.Limit * (param.Index - 1) + 1;
                foreach (var label in data.OrderByDescending(x => x.CreateTime))
                {
                    var aVM = new LabelVM(label);
                    aVM.OrderNumber = (orderNumber++).ToString();
                    r.Data.Add(aVM);
                }
                r.Code = 200;
                r.Status = true;
                r.Msg = "请求成功";
            }
            return Json(r);
        }

        [HttpPost]
        public async Task<int> GetLabelsCount()
        {
            var query = await _labelRepository.GetAllAsyn();
            return query.Count();
        }

        /// <summary>
        /// 添加或编辑标签
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> AddOrEditLabel(LabelVM model)
        {
            var r = new DataResultViewModel<LabelVM>();
            //var has = await _labelRepository.FindByAsyn(x => x.Name == model.Name);
            var has = _labelRepository.GetSingleBy(x => x.Name == model.Name);
            if (has == null)
            {
                if (Guid.Empty == model.Id)
                {
                    var label = new Label()
                    {
                        Name = model.Name
                    };
                    var ok = await _labelRepository.AddOrEditAndSaveAsyn(label);
                    if (ok)
                    {
                        r.Code = 200;
                        r.Status = true;
                        r.Msg = "标签添加成功";
                        r.Data = new LabelVM(label);
                    }
                }
                else
                {
                    var label = await _labelRepository.GetSingleAsyn(model.Id);
                    label.Name = model.Name;
                    var ok = await _labelRepository.AddOrEditAndSaveAsyn(label);
                    if (ok)
                    {
                        r.Code = 200;
                        r.Status = true;
                        r.Msg = "标签编辑成功";
                        r.Data = new LabelVM(label);
                    }
                }
            }
            else
            {
                r.Msg = "标签已经存在";
            }
            return Json(r);
        }

        /// <summary>
        /// 获取单个标签
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetLabelById(Guid id)
        {
            var r = new DataResultViewModel<LabelVM>();
            var label = await _labelRepository.GetSingleAsyn(id);
            if (label != null)
            {
                r.Code = 200;
                r.Status = true;
                r.Data = new LabelVM(label);
            }
            else
            {
                r.Msg = "查询失败";
            }
            return Json(r);
        }

        /// <summary>
        /// 添加或编辑标签
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteLabel(Guid id)
        {
            var r = new ResultViewModel();
            var label = await _labelRepository.GetSingleAsyn(id);
            if (label != null)
            {
                var ok = await _labelRepository.DeleteAndSaveAsyn(label.Id);
                if (ok.DeleteSatus)
                {
                    var q=await _articleAndLabelRepository.FindByAsyn(x => x.LabelId == label.Id);
                    foreach (var item in q)
                    {
                        await _articleAndLabelRepository.DeleteAndSaveAsyn(item.Id);
                    }
                    r.Code = 200;
                    r.Status = true;
                    r.Msg = "删除成功！";
                }
                else
                {
                    r.Msg = "删除失败，请重试！";
                }
            }
            else
            {
                r.Msg = "该标签不存在或者已经被删除！";
            }
            return Json(r);
        }

        #endregion


        #region 分类管理

        public IActionResult Category()
        {
            return View();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetCategories(ParamsInputViewModel param)
        {
            var r = new DataResultViewModel<List<CategoryVM>>();
            r.Data = new List<CategoryVM>();
            var query = await _categoryRepository.GetAllAsyn();
            var data = query.Skip(param.Limit * (param.Index - 1)).Take(param.Limit);
            if (data.Any())
            {
                int orderNumber = param.Limit * (param.Index - 1) + 1;
                foreach (var cate in data.OrderByDescending(x => x.CreateTime))
                {
                    var aVM = new CategoryVM(cate);
                    aVM.OrderNumber = (orderNumber++).ToString();
                    r.Data.Add(aVM);
                }
                r.Code = 200;
                r.Status = true;
                r.Msg = "请求成功";
            }
            return Json(r);
        }

        [HttpPost]
        public async Task<int> GetCategoriesCount()
        {
            var query = await _categoryRepository.GetAllAsyn();
            return query.Count();
        }
        /// <summary>
        /// 添加或编辑分类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> AddOrEditCategory(CategoryVM model)
        {
            var r = new DataResultViewModel<CategoryVM>();
            try
            {
                //var has = await _labelRepository.FindByAsyn(x => x.Name == model.Name);
                var has = _categoryRepository.GetSingleBy(x => x.Name == model.Name);
                if (has == null)
                {
                    if (Guid.Empty == model.Id)
                    {
                        var cate = new Category()
                        {
                            Name = model.Name
                        };
                        var ok = await _categoryRepository.AddOrEditAndSaveAsyn(cate);
                        if (ok)
                        {
                            r.Code = 200;
                            r.Status = true;
                            r.Msg = "添加成功";
                            r.Data = new CategoryVM(cate);
                        }
                    }
                    else
                    {
                        var cate = await _categoryRepository.GetSingleAsyn(model.Id);
                        cate.Name = model.Name;
                        var ok = await _categoryRepository.AddOrEditAndSaveAsyn(cate);
                        if (ok)
                        {
                            r.Code = 200;
                            r.Status = true;
                            r.Msg = "编辑成功";
                            r.Data = new CategoryVM(cate);
                        }
                    }
                }
                else
                {
                    r.Msg = "分类名称已经存在";
                }
                return Json(r);

            }
            catch (Exception ex)
            {
                r.Msg = ex.Message;
                return Json(r);

            }
        }

        /// <summary>
        /// 获取单个分类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetCategoryById(Guid id)
        {
            var r = new DataResultViewModel<CategoryVM>();
            var cate = await _categoryRepository.GetSingleAsyn(id);
            if (cate != null)
            {
                r.Code = 200;
                r.Status = true;
                r.Data = new CategoryVM(cate);
            }
            else
            {
                r.Msg = "查询失败";
            }
            return Json(r);
        }

        /// <summary>
        /// 添加或编辑标签
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteCategory(Guid id)
        {
            var r = new ResultViewModel();
            var cate = await _categoryRepository.GetSingleAsyn(id);
            if (cate != null)
            {
                var hasArticle = _articleRepository.GetSingleBy(x => x.CategoryId == cate.Id);
                if (hasArticle == null)
                {
                    var ok = await _categoryRepository.DeleteAndSaveAsyn(cate.Id);
                    if (ok.DeleteSatus)
                    {
                        r.Code = 200;
                        r.Status = true;
                        r.Msg = "删除成功！";
                    }
                    else
                    {
                        r.Msg = "删除失败，刷新后重试！";
                    }
                }
                else
                {
                    r.Msg = "删除失败，该分类下有文章无法删除！";
                }
            }
            else
            {
                r.Msg = "该分类不存在或者已经被删除！";
            }
            return Json(r);
        }

        #endregion

        /// <summary>
        /// 设置推荐
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DisRecommend(DisEnableFriendLinkInput m)
        {
            var r = new ResultViewModel();
            var article = await _articleRepository.GetSingleAsyn(m.Id);
            if (article != null)
            {
                article.IsRecommend = m.Enable;
                var ok = await _articleRepository.AddOrEditAndSaveAsyn(article);
                if (ok)
                {
                    r.Code = 200;
                    r.Status = true;
                    r.Msg = "操作成功";
                }
            }
            return Json(r);
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteArticle(Guid id)
        {
            var r = new ResultViewModel();
            var article = await _articleRepository.GetSingleAsyn(id);
            if (article != null)
            {
                var ok = await _articleRepository.DeleteAndSaveAsyn(id);
                if (ok.DeleteSatus)
                {
                    r.Code = 200;
                    r.Status = true;
                    r.Msg = "操作成功";
                }
            }
            return Json(r);
        }


        #endregion

        private async Task<ApplicationUser> GetUser(string name) => await _userManager.FindByNameAsync(name);


        #region 用户信息页面
        /// <summary>
        /// 资料修改页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);

            var userVM = new ApplicationUserVM(user);

            return View(userVM);
        }

        /// <summary>
        /// 资料修改页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        #endregion



        #region 用户管理页面

        public IActionResult Users()
        {
            return View();
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetUsers(ParamsInputViewModel param)
        {
            var r = new DataResultViewModel<List<ApplicationUserVM>>();
            r.Data = new List<ApplicationUserVM>();
            var query = _userManager.Users;
            var pagin = query.Skip(param.Limit * (param.Index - 1)).Take(param.Limit);
            if (pagin.Any())
            {
                int orderNumber = param.Limit * (param.Index - 1) + 1;
                foreach (var user in pagin)
                {
                    var aVM = new ApplicationUserVM(user);
                    aVM.OrderNumber = (orderNumber++).ToString();
                    var roleNames = await _userManager.GetRolesAsync(user);
                    foreach (var roleName in roleNames)
                    {
                        if (roleName == ApplicationRoleNameEnum.Admin.ToString())
                        {
                            aVM.IsAdmin = true;
                        }
                        var role = await _roleManager.FindByNameAsync(roleName);
                        aVM.Roles.Add(new ApplicationRoleVM(role));
                    }
                    r.Data.Add(aVM);
                }
                r.Code = 200;
                r.Status = true;
                r.Msg = "加载成功...";
            }
            return Json(r);
        }


        public int GetUsersCount()
        {
            var users = _userManager.Users.Count();
            return users;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ResetPassword(UserResetPasswordVM model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var ok = await _userManager.ResetPasswordAsync(user, token, model.ConfirmNewPassword);
                    if (ok.Succeeded)
                    {
                        return Json(new ResultViewModel(code: 200, status: true, msg: "密码重置成功，新密码：" + model.ConfirmNewPassword));
                    }
                }
                return Json(new ResultViewModel(msg: "密码修改失败"));
            }
            catch (Exception)
            {
                return Json(new ResultViewModel(msg: "密码修改失败"));
            }
        }

        /// <summary>
        /// 解除或禁用用户
        /// </summary>
        /// <returns></returns> 
        [HttpPost]
        public async Task<JsonResult> DisEnableUser(DisEnableUserViewModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user != null)
                {
                    var ok = await _userManager.SetLockoutEnabledAsync(user, model.Enable);
                    if (ok.Succeeded)
                    {
                        return Json(new ResultViewModel(code: 200, status: true, msg: model.Enable ? "用户已经被锁定成功" : "用户已经被解除锁定"));
                    }
                }
                return Json(new ResultViewModel(msg: "操作失败"));
            }
            catch (Exception)
            {
                return Json(new ResultViewModel(msg: "操作失败"));
            }
        }

        /// <summary>
        /// 解除或禁用用户
        /// </summary>
        /// <returns></returns> 
        [HttpPost]
        public async Task<JsonResult> DeleteUser(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user != null)
                {
                    var ok = await _userManager.DeleteAsync(user);
                    if (ok.Succeeded)
                    {
                        return Json(new ResultViewModel(code: 200, status: true, msg: "用户【" + user.UserName + "】已被删除！"));
                    }
                }
                return Json(new ResultViewModel(msg: "操作失败！"));
            }
            catch (Exception)
            {
                return Json(new ResultViewModel(msg: "操作失败"));
            }
        }

        #endregion
    }
}
