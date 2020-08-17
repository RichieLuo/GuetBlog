using System;
using System.Collections.Generic;
using System.Text;

namespace Guet.ViewModels.Common
{
    public class BaseResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; } = 0;

        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; } = false;

        /// <summary>
        /// 状态文本
        /// </summary>
        public string Msg { get; set; } = "操作失败";
        /// <summary>
        /// 集合数据总量
        /// </summary>
        public int Count { get; set; } = 0;
        /// <summary>
        /// 是否为集合
        /// </summary>
        public bool IsArray { get; set; } = false;
    }
}
