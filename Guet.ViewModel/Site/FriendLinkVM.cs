using System;
using System.Collections.Generic;
using System.Text;
using Guet.Entities.Site;

namespace Guet.ViewModels.Site
{
    /// <summary>
    /// 友情链接
    /// </summary>
    public class FriendLinkVM : EntityBaseVM
    {
        /// <summary>
        /// 链接名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 链接封面
        /// </summary>
        public string Cover { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        public FriendLinkVM() { }
        public FriendLinkVM(FriendLink friendLink)
        {
            Id = friendLink.Id;
            Name = friendLink.Name;
            Url = friendLink.Url;
            Cover = friendLink.Cover;
            IsEnable = friendLink.IsEnable;
        }

    }
}
