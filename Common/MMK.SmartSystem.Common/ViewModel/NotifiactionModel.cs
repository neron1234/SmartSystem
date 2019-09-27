using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.ViewModel
{
    public class NotifiactionModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 通知标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 通知内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 通知类型
        /// </summary>
        public EnumPromptType NotifiactionType { get; set; }
    }

    #region EnumPromptType
    /// <summary>
    /// 提示类型
    /// </summary>
    public enum EnumPromptType
    {
        /// <summary>
        /// 消息
        /// </summary>
        Info,
        /// <summary>
        /// 警告
        /// </summary>
        Warn,
        /// <summary>
        /// 失败
        /// </summary>
        Error,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
    }
    #endregion
}
