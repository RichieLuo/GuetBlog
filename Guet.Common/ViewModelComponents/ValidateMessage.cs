using System;
using System.Collections.Generic;
using System.Text;

namespace Guet.Common.ViewModelComponents
{
    public class ValidateMessage
    {
        public bool IsOk { get; set; }
        public List<ValidateMessageItem> ValidateMessageItems { get; set; }

        public ValidateMessage()
        {
            this.IsOk = true;
            this.ValidateMessageItems = new List<ValidateMessageItem>();
        }
    }
}
