using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Guet.Entities.ApplicationOrganization;
using Guet.ViewModels.ApplicationOrganization;
using Guet.ViewModels.Common;
using Guet.Web.Models;

namespace Guet.Web.Controllers
{
    /// <summary>
    /// 登录注册
    /// </summary>
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
             RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// 博客用户的个人中心
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 管理员登录页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        /// <summary>
        /// 管理员登录处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Login([FromForm]LoginRegisterViewModel model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                return Json(new ResultViewModel(msg: "请检查账号密码是否已经输入！"));
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return Json(new ResultViewModel(msg: "登录失败，请检查您的账号是否输入正确！"));
            }
            var isAdmin = await _userManager.IsInRoleAsync(user, ApplicationRoleNameEnum.Admin.ToString());
            if (!isAdmin)
            {
                return Json(new ResultViewModel(msg: "登录失败，您的权限不足！"));
            }
            var loginStatus = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);
            if (loginStatus.Succeeded)
            {
                return Json(new ResultViewModel(200, true, msg: "登录成功,请正在跳转...", returnUrl: string.IsNullOrWhiteSpace(model.ReturnUrl) ? "/Admin/Index" : model.ReturnUrl));
            }
            return Json(new ResultViewModel(msg: "登录失败,请反馈给博主！"));
        }


        #region 普通用户登录注册(使用弹窗的方式)

        /// <summary>
        ///  普通用户登录请求处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UserLogin([FromForm]LoginRegisterViewModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    return Json(new ResultViewModel(msg: "登录失败，请检查您的账号是否输入正确！"));
                }
                if (await _userManager.IsLockedOutAsync(user))
                {
                    return Json(new ResultViewModel(msg: "账号已经被锁定！"));
                }
                var loginStatus = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);
                if (loginStatus.Succeeded)
                {
                    var isAdmin = await _userManager.IsInRoleAsync(user, ApplicationRoleNameEnum.Admin.ToString());
                    var userVM = new ApplicationUserVM(user);
                    userVM.IsAdmin = isAdmin;
                    return Json(new DataResultViewModel<ApplicationUserVM>()
                    {
                        Code = 200,
                        Status = true,
                        Msg = "登录成功",
                        Data = userVM
                    });
                }
                return Json(new ResultViewModel(msg: "登录失败,请检查你的账号或密码是否正确！"));
            }
            catch (Exception ex)
            {
                return Json(new ResultViewModel(msg: $"登录失败：{ex.Message}"));

            }
        }

        /// <summary>
        /// 普通用户注册请求处理 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UserRegister([FromForm]LoginRegisterViewModel model)
        {
            try
            {
                var hasUser = await _userManager.FindByNameAsync(model.UserName);
                if (hasUser != null)
                {
                    return Json(new ResultViewModel(msg: "注册失败，该用户已经存在，推荐使用邮箱作为用户名！"));
                }

                var roleName = ApplicationRoleNameEnum.User.ToString();
                ApplicationUser user = new ApplicationUser()
                {
                    Avatar = "/images/defaultAvatar.jpg",
                    UserName = model.UserName,
                    NickName = model.NickName,
                    Email = model.EMail,
                    Remark =string.IsNullOrEmpty( model.Remark)?"该用户是新用户还没有填写个人简介":model.Remark,
                    LockoutEnabled = false
                };
                var addUser = await _userManager.CreateAsync(user);
                //await _userManager.SetLockoutEnabledAsync(user, false);
                if (addUser.Succeeded)
                {
                    var addPassword = await _userManager.AddPasswordAsync(user, model.Password);
                    if (addPassword.Succeeded)
                    {
                        var roleExists = await _roleManager.RoleExistsAsync(roleName);
                        if (!roleExists)
                        {
                            var userRole = new ApplicationRole()
                            {
                                Name = roleName,
                                DisplayName = "普通用户",
                                Description = ApplicationRoleTypeEnum.适用于普通注册用户.ToString(),
                                ApplicationRoleType = ApplicationRoleTypeEnum.适用于普通注册用户
                            };
                            await _roleManager.CreateAsync(userRole);
                        }
                        if (!await _userManager.IsInRoleAsync(user, roleName))
                        {
                            await _userManager.AddToRoleAsync(user, roleName);
                            return Json(new ResultViewModel(200, true, "注册成功，请牢记您的账号密码！"));
                        }
                    }
                }

                return Json(new ResultViewModel(msg: "注册失败，您可以向管理员反馈情况！"));
            }
            catch (Exception ex)
            {
                return Json(new ResultViewModel(msg: $"操作失败：{ex.Message}"));
            }
        }


        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<JsonResult> GetInfoByUserId(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var userVM = new ApplicationUserVM(user);
                return Json(userVM);
            }
            return Json(new ApplicationUserVM());
        }


        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<JsonResult> UpdateInfo(ApplicationUserVM m)
        {
            var r = new DataResultViewModel<ApplicationUserVM>();
            var user = await _userManager.FindByIdAsync(m.Id.ToString());
            if (user != null)
            {
                if (!string.IsNullOrEmpty(m.Password) && !string.IsNullOrEmpty(m.ConfirmPassword))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var b = await _userManager.ResetPasswordAsync(user, token, m.ConfirmPassword);
                    if (b.Succeeded)
                    {
                        r.Msg = "密码修改成功，但资料修改失败！";
                    }
                    else
                    {
                        r.Msg = "密码修改失败";
                        return Json(r);
                    }
                }
                else
                {
                    r.Msg = "两次密码输入不一致！"; 
                }

                user.NickName = m.NickName;
                user.Avatar = m.Avatar;
                user.Email = m.EMail;
                user.Remark = m.Remark;

                var ok = await _userManager.UpdateAsync(user);
                if (ok.Succeeded)
                {
                    r.Code = 200;
                    r.Status = true;
                    r.Msg = "资料修改成功！";
                    r.Data = new ApplicationUserVM(user);
                }
                else
                {
                    r.Msg = "资料修改失败！";
                }
                return Json(r);
            }
            return Json(r);
        }

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<JsonResult> Logout(bool isAdmin)
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Json(new ResultViewModel(200, true, "注销成功！", returnUrl: isAdmin ? "/Account/Admin" : "/Home/Index"));
            }
            catch (Exception)
            {
                return Json(new ResultViewModel(msg: "注销失败，请刷新页面！"));
            }
        }

        /// <summary>
        /// 修改个人资料
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<JsonResult> ChangeProfile(ApplicationUserVM model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                if (user != null)
                {
                    user.Remark = model.Remark;
                    user.NickName = model.NickName;
                    user.Email = model.EMail;
                    user.Avatar = model.Avatar;
                    var ok = await _userManager.UpdateAsync(user);
                    if (ok.Succeeded)
                    {
                        return Json(new ResultViewModel(code: 200, status: true, msg: "资料修改成功"));
                    }
                }
                return Json(new ResultViewModel(msg: "资料修改失败"));
            }
            catch (Exception)
            {
                return Json(new ResultViewModel(msg: "资料修改失败"));
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<JsonResult> ChangePassword(UserResetPasswordVM model)
        {
            try
            {
                if (model.NewPassword != model.ConfirmNewPassword)
                {
                    return Json(new ResultViewModel(msg: "新密码和确认密码不相同"));
                }
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                if (user != null)
                {
                    var ok = await _userManager.ChangePasswordAsync(user, model.Password, model.ConfirmNewPassword);
                    if (ok.Succeeded)
                    {
                        return Json(new ResultViewModel(code: 200, status: true, msg: "密码修改成功"));
                    }
                    else
                    {
                        return Json(new ResultViewModel(msg: "旧密码不正确"));
                    }
                }
                return Json(new ResultViewModel(msg: "密码修改失败"));
            }
            catch (Exception)
            {
                return Json(new ResultViewModel(msg: "密码修改失败"));
            }
        }



        #endregion


        #region 站点安装

        /// <summary>
        /// 安装页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Install()
        {
            var user = await _userManager.GetUsersInRoleAsync(ApplicationRoleNameEnum.Admin.ToString());
            if (user.Count > 0)
            {
                ViewBag.HasInstall = true;
            }
            else
            {
                ViewBag.HasInstall = false;
            }
            return View();
        }


        /// <summary>
        /// 站点安装
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Install([FromForm]InstallModel model)
        {
            try
            {
                var roleName = ApplicationRoleNameEnum.Admin.ToString();
                var users = await _userManager.GetUsersInRoleAsync(roleName);
                if (users.Count > 0)
                {
                    return Json(new ResultViewModel(msg: "网站已经安装，请勿重复操作！"));
                }
                if (model.ConfirmPassword != model.Password)
                {
                    return Json(new ResultViewModel(msg: "两次输入密码不正确！"));
                }
                var hasUser = await _userManager.FindByNameAsync(model.UserName);
                if (hasUser != null)
                {
                    return Json(new ResultViewModel(msg: "安装失败,管理员账号已经存在！"));
                }

                ApplicationUser user = new ApplicationUser()
                {
                    Remark = model.Remark,
                    UserName = model.UserName,
                    NickName = model.NickName,
                    Email = model.EMail,
                    Avatar = "/images/defaultAvatar.jpg",
                    LockoutEnabled = false
                };
                var addUser = await _userManager.CreateAsync(user);
                if (addUser.Succeeded)
                {
                    var addPassword = await _userManager.AddPasswordAsync(user, model.Password);
                    if (addPassword.Succeeded)
                    {
                        var roleExists = await _roleManager.RoleExistsAsync(roleName);
                        if (!roleExists)
                        {
                            var userRole = new ApplicationRole()
                            {
                                Name = roleName,
                                DisplayName = "博主·超级管理员",
                                Description = "博主·超级管理员",
                                ApplicationRoleType = ApplicationRoleTypeEnum.适用于系统管理人员
                            };
                            await _roleManager.CreateAsync(userRole);
                        }
                        if (!await _userManager.IsInRoleAsync(user, roleName))
                        {
                            await _userManager.AddToRoleAsync(user, roleName);
                            return Json(new ResultViewModel(code: 200, status: true, msg: "安装成功！", returnUrl: "/Account/Login"));
                        }
                    }
                }

                return Json(new ResultViewModel(msg: "安装失败"));
            }
            catch (Exception ex)
            {
                return Json(new ResultViewModel(msg: $"安装失败：{ex.Message}"));
            }
        }

        #endregion


        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
