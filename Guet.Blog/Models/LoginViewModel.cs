using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Guet.Web.Models
{
    /// <summary>
    /// 普通用户登录注册模型
    /// </summary>
    public class LoginRegisterViewModel
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string EMail { get; set; }
        /// <summary>
        ///验证码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 返回路径
        /// </summary>
        public string ReturnUrl { get; set; }
        public  string Remark { get; set; }
        public LoginRegisterViewModel() { }
        /// <summary>
        ///有参构造函数
        /// </summary>
        /// <param name="nickname">昵称</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="confirmPassword">确认密码</param>
        /// <param name="email">邮箱地址</param>
        /// <param name="code">验证码</param>
        /// <param name="returnUrl">登录返回的路径</param>
        public LoginRegisterViewModel(string nickname, string username, string password, string confirmPassword, string email, string code = "", string returnUrl = "")
        {
            this.NickName = nickname;
            this.UserName = username;
            this.Password = password;
            this.ConfirmPassword = confirmPassword;
            this.EMail = email;
            this.Code = code;
            this.ReturnUrl = returnUrl;
        }

    }
}
