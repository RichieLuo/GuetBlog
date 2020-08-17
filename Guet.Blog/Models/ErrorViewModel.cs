using System;

namespace Guet.Web.Models
{
    public class ErrorViewModel
    {
        /// <summary>
        /// ×´Ì¬Âë
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// ÇëÇó×´Ì¬
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// ´íÎóÄÚÈİ
        /// </summary>
        public string Msg { get; set; }
        public string RequestId { get; set; } 
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}