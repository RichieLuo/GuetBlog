using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guet.ViewModels.Common;

namespace Guet.Web.Models
{
    /// <summary>
    /// 处理结果返回模型
    /// </summary>
    public class ResultViewModel: BaseResult
    { 
        /// <summary>
        /// 操作成功跳转地址
        /// </summary> 
        public string ReturnUrl { get; set; }

        public ResultViewModel() { }
     
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="status">状态</param>
        /// <param name="msg">状态消息文本</param>
        /// <param name="returnUrl">操作成功跳转地址</param>
        public ResultViewModel(int code = 0, bool status = false, string msg = "操作失败", string returnUrl = "")
        {
            this.Code = code;
            this.Status = status;
            this.Msg = msg;
            this.ReturnUrl = returnUrl;
        }
    }
}
