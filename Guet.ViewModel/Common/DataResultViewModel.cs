using System;
using System.Collections.Generic;
using System.Text;

namespace Guet.ViewModels.Common
{
    /// <summary>
    /// 结果返回模型
    /// </summary>
    public class DataResultViewModel<T> : BaseResult
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }  
        public DataResultViewModel(){}
    }
}
