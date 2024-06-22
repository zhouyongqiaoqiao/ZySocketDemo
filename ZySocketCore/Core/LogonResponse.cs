using System;
using System.Collections.Generic;
using System.Text;
using ZySocketCore.Core.Enum;

namespace ZySocketCore.Core
{
    /// <summary>
    /// 登录响应
    /// </summary>
    public class LogonResponse
    {
        private LogonResult logonResult = LogonResult.Failed;
        private string failureCause = string.Empty;

        public LogonResponse() { }
        public LogonResponse(LogonResult result, string _failureCause)
        {
            this.LogonResult = result;
            this.FailureCause = _failureCause;
        }

        public LogonResult LogonResult { get => logonResult; set => logonResult = value; }
        //
        // 摘要:
        //     当LogonResult为Failed时，指明失败的原因。
        public string FailureCause { get => failureCause; set => failureCause = value; }
    }
}
