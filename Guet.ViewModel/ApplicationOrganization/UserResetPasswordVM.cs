using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Guet.ViewModels.ApplicationOrganization
{
    /// <summary>
    /// 用户重置密码视图
    /// </summary>
    public class UserResetPasswordVM
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 旧密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// 再次确认新密码
        /// </summary>
        public string ConfirmNewPassword { get; set; }

    }
}
