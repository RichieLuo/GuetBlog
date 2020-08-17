using System;

namespace Guet.Web.Models
{
    public class ErrorViewModel
    {
        /// <summary>
        /// ״̬��
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// ����״̬
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string Msg { get; set; }
        public string RequestId { get; set; } 
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}