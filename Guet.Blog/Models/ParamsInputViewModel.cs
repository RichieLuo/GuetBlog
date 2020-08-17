using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guet.Web.Models
{
    /// <summary>
    /// 输入参数
    /// </summary>
    public class ParamsInputViewModel
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public int Index { get; set; } = 1;
        public int Limit { get; set; } = 10;

        public ParamsInputViewModel() { }
        public ParamsInputViewModel(string id, string key, int index = 1, int limit = 10)
        {
            Id = id;
            Key = key;
            Index = index;
            Limit = limit;
        }
    }
}
