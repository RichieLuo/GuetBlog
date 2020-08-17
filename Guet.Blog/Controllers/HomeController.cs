using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Guet.DataAccess;
using Guet.DataAccess.SqlServer.Ultilities;
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
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEntityRepository<SiteSetting> _siteSettingRepository;
        private readonly IEntityRepository<Article> _articleRepository;
        private readonly IEntityRepository<Category> _categoryRepository;
        private readonly IEntityRepository<Label> _labelRepository;
        private readonly IEntityRepository<ArticleAndLabel> _articleAndLabelRepository;

        private readonly IEntityRepository<Banner> _bannerRepository;
        private readonly IEntityRepository<FriendLink> _friendLinkRepository;
        private readonly IEntityRepository<About> _aboutRepository;
        private readonly IEntityRepository<Message> _messageRepository;

        private readonly IEntityRepository<Comment> _commentRepository;
        private readonly IEntityRepository<CommentItem> _commentItemRepository;
        public HomeController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IEntityRepository<SiteSetting> siteSettingRepository,
            IEntityRepository<Article> articleRepository,
            IEntityRepository<Category> categoryRepository,
            IEntityRepository<Label> labelRepository,
            IEntityRepository<ArticleAndLabel> articleAndLabelRepository,
            IEntityRepository<FriendLink> friendLinkRepository,
            IEntityRepository<About> aboutRepository,
            IEntityRepository<Message> messageRepository,
            IEntityRepository<Banner> bannerRepository,
            IEntityRepository<Comment> commentRepository,
            IEntityRepository<CommentItem> commentItemRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _siteSettingRepository = siteSettingRepository;
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _labelRepository = labelRepository;
            _articleAndLabelRepository = articleAndLabelRepository;
            _friendLinkRepository = friendLinkRepository;
            _aboutRepository = aboutRepository;
            _messageRepository = messageRepository;
            _bannerRepository = bannerRepository;
            _commentRepository = commentRepository;
            _commentItemRepository = commentItemRepository;
        }

        /// <summary>
        /// 博客首页
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var userAdmin = await _userManager.GetUsersInRoleAsync(ApplicationRoleNameEnum.Admin.ToString());
            if (userAdmin.Count <= 0)
            {
                return RedirectToAction("Install", "Account");
            }
            var articles = await _articleRepository.GetAllAsyn();
            var data = new HomeDataViewModel();
            data.SiteInfo = await GetSiteInfo();
            foreach (var item in articles.Where(x => x.IsRecommend == true))
            {
                var aVM = new ArticleVM(item);
                data.ReArticles.Add(aVM);
            }
            foreach (var item in articles.OrderByDescending(x => x.VisitCount).Take(5).Skip(0))
            {
                var aVM = new ArticleVM(item);
                data.TopTenArticles.Add(aVM);
            }
            var query = await _bannerRepository.GetAllAsyn();
            foreach (var item in query.Where(x => x.IsBanner == true && x.IsShow == true))
            {
                data.Banners.Add(new BannerVM(item));
            }
            foreach (var item in query.Where(x => x.IsBanner == false && x.IsShow == true))
            {
                data.ADs.Add(new BannerVM(item));
            }
            var links = await _friendLinkRepository.FindByAsyn(x => x.IsEnable == true);
            foreach (var link in links)
            {
                data.FriendLinks.Add(new FriendLinkVM(link));
            }
            var users = _userManager.Users;
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, ApplicationRoleNameEnum.Admin.ToString()))
                {
                    data.User = new ApplicationUserVM(user);
                    break;
                }
            }

            return View(data);
        }

        [HttpPost]
        public async Task<SiteSettingVM> GetSiteInfo()
        {
            var siteInfo = await _siteSettingRepository.GetAllAsyn();
            return new SiteSettingVM(siteInfo.FirstOrDefault());

        }

        /// <summary>
        /// 站内搜索
        /// </summary> 
        /// <returns></returns>
        public async Task<IActionResult> Search(string key)
        {
            //var data = await Search(new ParamsInputViewModel() { Key = key });
            //return View(data.Data);
            return View(new List<ArticleVM>());

        }

        /// <summary>
        /// 站内搜索
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<ArticleVM>> Search(ParamsInputViewModel param)
        {
            var list = new List<ArticleVM>();
            var searchKey = param.Key;
            var query = await _articleRepository.FindByAsyn(x => x.Title.Contains(searchKey) || x.Abstract.Contains(searchKey) || x.Content.Contains(searchKey));
            if (query.Any())
            {
                int orderNumber = param.Limit * (param.Index - 1) + 1;
                var user = await GetUserVMById(query.FirstOrDefault()?.UserId.ToString());
                foreach (var item in query.ToList())
                {
                    var aVM = new ArticleVM(item);
                    aVM.CTime = aVM.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    if (user != null)
                    {
                        aVM.User = user;
                    }
                    aVM.OrderNumber = (orderNumber++).ToString();

                    var cate = await _categoryRepository.GetSingleAsyn(item.CategoryId);
                    if (cate != null)
                    {
                        aVM.Category = new CategoryVM(cate);
                    }

                    list.Add(aVM);
                }
            }
            return list;
        }


        ///<summary>
        /// 文章统计
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetArticleCount(string key)
        {
            IQueryable<Article> query = null;
            if (string.IsNullOrEmpty(key))
            {
                query = await _articleRepository.GetAllAsyn();
            }
            else
            {
                query = await _articleRepository.FindByAsyn(x => x.Title.Contains(key) || x.Abstract.Contains(key) || x.Content.Contains(key));
            }
            return Json(new { Count = query.Count() });
        }

        /// <summary>
        /// 博客文章列表 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Articles()
        {
            var query = await _articleRepository.GetAllAsyn();
            var articlesVM = new List<ArticleVM>();
            int orderNumber = 0;
            foreach (var item in query.ToList())
            {
                var articleVM = new ArticleVM(item);
                var category = await _categoryRepository.GetSingleAsyn(item.CategoryId);
                if (category != null)
                {
                    articleVM.Category = new CategoryVM(category);
                }
                articleVM.OrderNumber = (orderNumber++).ToString();
                articlesVM.Add(articleVM);
            }
            return View(articlesVM);
        }
        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<ArticleVM>> GetArticles(ParamsInputViewModel param)
        {
            var list = new List<ArticleVM>();
            var searchKey = param.Key;
            var query = await _articleRepository.GetAllAsyn();
            var articles = query.Skip(param.Limit * (param.Index - 1)).Take(param.Limit);
            if (articles.Any())
            {
                int orderNumber = param.Limit * (param.Index - 1) + 1;
                var user = await GetUserVMById(articles.FirstOrDefault()?.UserId.ToString());
                user = user == null ? new ApplicationUserVM() : user;
                foreach (var item in articles.ToList())
                {
                    var aVM = new ArticleVM(item);
                    aVM.CTime = aVM.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    aVM.User = user;
                    aVM.OrderNumber = (orderNumber++).ToString();

                    var cate = await _categoryRepository.GetSingleAsyn(item.CategoryId);
                    if (cate != null)
                    {
                        aVM.Category = new CategoryVM(cate);
                    }

                    list.Add(aVM);
                }
            }
            return list;
        }

        /// <summary>
        /// 博客文章明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ArticleDetail(Guid id)
        {
            var r = new DataResultViewModel<ArticleVM>();

            var article = await _articleRepository.GetSingleAsyn(id);
            if (article != null)
            {
                article.VisitCount += 1;
                await _articleRepository.AddOrEditAndSaveAsyn(article);

                var articleVM = new ArticleVM(article);
                var cate = await _categoryRepository.GetSingleAsyn(article.CategoryId);
                if (cate != null)
                {
                    articleVM.Category = new CategoryVM(cate);
                }
                var labels = await _articleAndLabelRepository.FindByAsyn(x => x.ArticleId == article.Id);
                foreach (var item in labels)
                {
                    var label = await _labelRepository.GetSingleAsyn(item.LabelId);
                    articleVM.Labels.Add(new LabelVM(label));
                }

                r.Code = 200;
                r.Status = true;
                r.Msg = "找到一篇文章";
                r.Data = articleVM;
                ViewBag.CommentsData = await GetCommentListByArticleId(article.Id);
                var reArticles = await _articleRepository.FindByAsyn(x => x.IsRecommend == true);
                reArticles = reArticles.Skip(0).Take(4);
                var reArticlesVM = new List<ArticleVM>();
                foreach (var item in reArticles)
                {
                    reArticlesVM.Add(new ArticleVM(item));
                }
                ViewBag.RecommendArticles = reArticlesVM;
                return View(r);
            }
            return RedirectToAction("Error", "Home", new ErrorViewModel { Code = 0, Status = false, Msg = "文章不存在，可能已被博主删除！" });
        }

        /// <summary>
        ///  博客文章分类
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IActionResult> Category(ParamsInputViewModel param)
        {
            bool check = Guid.TryParse(param.Id, out Guid checkId);
            if (!string.IsNullOrEmpty(param.Id) && check)
            {
                var r = new DataResultViewModel<CategoryVM>();
                ViewBag.IsList = false;
                CategoryVM categoryVM = null;
                var category = await _categoryRepository.GetSingleAsyn(checkId);
                if (category != null)
                {
                    categoryVM = new CategoryVM(category);
                    var articles = await _articleRepository.FindByAsyn(x => x.CategoryId == category.Id);
                    foreach (var article in articles.ToList())
                    {
                        categoryVM.Articles.Add(new ArticleVM(article));
                    }
                    r.Code = 200;
                    r.Status = true;
                    r.Msg = "操作成功";
                    r.Data = categoryVM;

                }
                ViewBag.ResultData = r;
            }
            else
            {
                ViewBag.IsList = true;
                ViewBag.ResultListData = PagingCategories(param);
            }
            return View();
        }

        /// <summary>
        /// 用于分页的请求
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataResultViewModel<List<CategoryVM>> PagingCategories(ParamsInputViewModel param)
        {
            var r = new DataResultViewModel<List<CategoryVM>>();
            var categories = _categoryRepository.GetAll();
            var paging = categories.Skip(param.Limit * (param.Index - 1)).Take(param.Limit);
            if (paging.Any())
            {
                var categoriesVM = new List<CategoryVM>();
                int orderNumber = param.Limit * (param.Index - 1) + 1;
                foreach (var categorie in paging.ToList())
                {
                    var categoryVM = new CategoryVM(categorie);
                    categoryVM.OrderNumber = (orderNumber++).ToString();
                    categoriesVM.Add(categoryVM);
                }
                r.Code = 200;
                r.Status = true;
                r.Msg = "操作成功";
                r.Count = categories.Count();
                r.IsArray = true;
                r.Data = categoriesVM;
            }
            return r;
        }
        /// <summary>
        ///  博客文章标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Label(ParamsInputViewModel param)
        {
            bool check = Guid.TryParse(param.Id, out Guid checkId);
            if (!string.IsNullOrEmpty(param.Id) && check)
            {
                //单标签查询
                var label = await _labelRepository.GetSingleAsyn(checkId);

            }
            else
            {
                //全部标签查询

            }
            var labels = await _labelRepository.GetAllAsyn();

            return View(labels);
        }
        /// <summary>
        /// 文章归档
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public IActionResult ArticleArchiving(DateTime date)
        {
            return View();
        }


        /// <summary>
        /// 友情链接
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> FriendLinks(ParamsInputViewModel param)
        {
            var query = await _friendLinkRepository.GetAllAsyn();
            var friendlinks = query.Skip(param.Limit * (param.Index - 1)).Take(param.Limit);
            var r = new DataResultViewModel<List<FriendLinkVM>>();
            r.Data = new List<FriendLinkVM>();
            if (friendlinks.Any())
            {
                r.Code = 200;
                r.Status = true;
                r.Msg = "请求成功";
                int orderNumber = param.Limit * (param.Index - 1) + 1;
                foreach (var item in friendlinks.ToList())
                {
                    var flvm = new FriendLinkVM(item);
                    flvm.OrderNumber = (orderNumber++).ToString();
                    r.Data.Add(flvm);
                }
            }
            return View(r);
        }

        /// <summary>
        /// 友情链接申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> ApplyFriendLink(FriendLinkVM model)
        {
            var r = new DataResultViewModel<FriendLinkVM>();
            var friendLink = new FriendLink()
            {
                Name = model.Name,
                Url = model.Url,
                Cover = model.Cover
            };

            var ok = await _friendLinkRepository.AddOrEditAndSaveAsyn(friendLink);
            if (ok)
            {
                r.Code = 200;
                r.Status = true;
                r.Msg = "您的申请已经提交成功！";
            }
            return Json(r);
        }

        /// <summary>
        /// 关于我
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> About()
        {
            var about = await _aboutRepository.GetAllAsyn();
            var aboutVM = new AboutVM(about.FirstOrDefault());
            return View(aboutVM);
        }


        /// <summary>
        /// 留言
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> LeavingMessage(ParamsInputViewModel param)
        {
            var query = await _messageRepository.GetAllAsyn();
            var messages = query.Skip(param.Limit * (param.Index - 1)).Take(param.Limit);
            var r = new DataResultViewModel<List<MessageVM>>();
            if (messages.Any())
            {
                r.Code = 200;
                r.Status = true;
                r.Msg = "请求成功";
                int orderNumber = param.Limit * (param.Index - 1) + 1;
                foreach (var item in messages.ToList())
                {
                    var mVM = new MessageVM(item);
                    mVM.OrderNumber = (orderNumber++).ToString();
                    r.Data.Add(mVM);
                }
            }
            return View(r);
        }

        /// <summary>
        /// 错误页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Error(ErrorViewModel err)
        {
            var error = new ErrorViewModel
            {
                Code = err.Code,
                Status = err.Status,
                Msg = err.Msg,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            string action = string.Empty;
            switch (err.Code)
            {
                case 401: action = "401"; break;
                case 403: action = "403"; break;
                case 404: action = "404"; break;
                case 500: action = "500"; break;
                default: action = "Error"; break;
            }
            return View(action, error);
        }

        /// <summary>
        /// 文章评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> Comment(CommentVM model)
        {
            var r = new DataResultViewModel<CommentVM>();
            bool isAdmin = false;
            var comment = new Comment()
            {
                ArticleId = model.ArticleId,
                Content = model.Content,
                UserId = model.UserId,
                SiteUrl = model.SiteUrl,
                NickName = string.IsNullOrEmpty(model.NickName) ? "匿名用户" : model.NickName,
                EMail = model.EMail,
                Avatar = "/images/defaultAvatar.jpg"
            };
            if (!string.IsNullOrEmpty(User.Identity?.Name))
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                comment.UserId = user.Id;
                comment.NickName = user.NickName;
                comment.Avatar = user.Avatar;
                comment.EMail = user.Email;
                isAdmin = await CheckIsAdmin(user);
            }
            var ok = await _commentRepository.AddOrEditAndSaveAsyn(comment);
            if (ok)
            {
                var cVM = new CommentVM(comment);
                cVM.IsAdmin = isAdmin;
                cVM.CanDelete = isAdmin;
                cVM.CTime = FormateDate(cVM.CreateTime);
                r.Code = 200;
                r.Status = true;
                r.Msg = "评论成功！";
                r.Data = cVM;
            }
            return Json(r);
        }

        public async Task<IActionResult> CommentItem(CommentItemVM model)
        {
            var r = new DataResultViewModel<CommentItemVM>();
            bool isAdmin = false;
            var commentItem = new CommentItem()
            {
                ParentId = model.ParentId,
                Content = model.Content,
                UserId = model.UserId,
                SiteUrl = model.SiteUrl,
                NickName = string.IsNullOrEmpty(model.NickName) ? "匿名用户" : model.NickName,
                EMail = model.EMail,
                Avatar = "/images/defaultAvatar.jpg"
            };
            if (!string.IsNullOrEmpty(User.Identity?.Name))
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                commentItem.UserId = user.Id;
                commentItem.NickName = user.NickName;
                commentItem.Avatar = user.Avatar;
                commentItem.EMail = user.Email;
                isAdmin = await CheckIsAdmin(user);
            }
            var ok = await _commentItemRepository.AddOrEditAndSaveAsyn(commentItem);
            if (ok)
            {
                var cVM = new CommentItemVM(commentItem);
                cVM.CanDelete = isAdmin;
                cVM.IsAdmin = isAdmin;
                cVM.CTime = FormateDate(cVM.CreateTime);
                r.Code = 200;
                r.Status = true;
                r.Msg = "回复成功！";
                r.Data = cVM;
            }
            return Json(r);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteComment(DeleteCommentViewModel m)
        {
            DeleteStatusModel ok = null;
            var r = new ResultViewModel()
            {
                Code = 200,
                Status = true,
                Msg = "评论删除成功"
            };
            if (m.IsParent)
            {
                ok = await _commentRepository.DeleteAndSaveAsyn(m.Id);
                if (ok.DeleteSatus)
                {
                    var childrens = await _commentItemRepository.FindByAsyn(x => x.ParentId == m.Id);
                    foreach (var item in childrens)
                    {
                        await _commentItemRepository.DeleteAndSaveAsyn(item.Id);
                    }
                    return Json(r);
                }
            }
            else
            {
                ok = await _commentItemRepository.DeleteAndSaveAsyn(m.Id);
                if (ok.DeleteSatus)
                {
                    return Json(r);
                }
            }
            return Json(new ResultViewModel() { Msg = "删除失败！" });
        }

        /// <summary>
        /// 获取文章评论列表
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<List<CommentVM>> GetCommentListByArticleId(Guid articleId)
        {
            var comments = new List<CommentVM>();

            if (!Guid.Empty.Equals(articleId))
            {
                var query = await _commentRepository.FindByAsyn(x => x.ArticleId == articleId);
                bool canDelete = false;
                bool checkRole = false;
                var isLogin = string.IsNullOrEmpty(User.Identity?.Name) ? false : true;
                if (isLogin)
                {
                    canDelete = await CheckIsAdmin(await _userManager.FindByNameAsync(User.Identity?.Name));
                }
                foreach (var comment in query)
                {
                    var commentVM = new CommentVM(comment);
                    commentVM.CTime = FormateDate(commentVM.CreateTime);
                    commentVM.CanDelete = canDelete;
                    if (!string.IsNullOrEmpty(comment.UserId))
                    {
                        var user = await _userManager.FindByIdAsync(comment.UserId);
                        commentVM.Avatar = user.Avatar;
                        checkRole = await CheckIsAdmin(user);
                        commentVM.IsAdmin = checkRole;
                    }
                    var items = await _commentItemRepository.FindByAsyn(x => x.ParentId == comment.Id);
                    foreach (var item in items.OrderBy(x => x.CreateTime))
                    {
                        var itemVM = new CommentItemVM(item);
                        itemVM.CanDelete = canDelete;
                        itemVM.CTime = FormateDate(itemVM.CreateTime);
                        if (!string.IsNullOrEmpty(item.UserId))
                        {
                            var user = await _userManager.FindByIdAsync(item.UserId);
                            itemVM.Avatar = user.Avatar;
                            checkRole = await CheckIsAdmin(user);
                            itemVM.IsAdmin = checkRole;

                        }
                        commentVM.Children.Add(itemVM);
                    }
                    comments.Add(commentVM);
                }
            }
            return comments;
        }

        private async Task<ApplicationUserVM> GetUserVMById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userVM = new ApplicationUserVM(user);
                return userVM;
            }
            return null;
        }

        private string FormateDate(DateTime dateTime)
        {
            string date = string.Empty;
            string week = string.Empty;
            switch ((int)dateTime.DayOfWeek)
            {
                case 0:
                    week = "星期日";
                    break;
                case 1:
                    week = "星期一";
                    break;
                case 2:
                    week = "星期二";
                    break;
                case 3:
                    week = "星期三";
                    break;
                case 4:
                    week = "星期四";
                    break;
                case 5:
                    week = "星期五";
                    break;
                default:
                    week = "星期六";
                    break;
            }
            date = $"{dateTime.Year}年{AddZero(dateTime.Month)}月{AddZero(dateTime.Day)}日 {AddZero(dateTime.Hour)}点{AddZero(dateTime.Minute)}分{AddZero(dateTime.Second)}秒 {week}";

            return date;
        }

        private string AddZero(int num)
        {
            return num < 10 ? "0" + num.ToString() : num.ToString();
        }

        private async Task<bool> CheckIsAdmin(ApplicationUser user)
        {
            if (user == null) return false;
            return await _userManager.IsInRoleAsync(user, ApplicationRoleNameEnum.Admin.ToString());
        }
    }
}
