using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guet.Web.Models
{
    /// <summary>
    /// 站点安装信息模型
    /// </summary>
    public class InstallModel
    {
        public string UserName { get; set; }
        public string NickName { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }

        public string EMail { get; set; }
        public string Remark { get; set; }
    }
}
