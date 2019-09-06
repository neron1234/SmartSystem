using MMK.SmartSystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class StringToTranslate
    {
        /// <summary>
        /// 文字翻译
        /// </summary>
        /// <param name="str">待翻译文字</param>
        /// <returns>翻译后的文本</returns>
        public static string Translate(this string str)
        {
            var dict = SmartSystemCommonConsts.UserConfiguration?.Localization?.Values?.SmartSystem;
            if (dict == null)
            {
                return str;
            }
            if (dict.ContainsKey(str))
            {
                return dict[str];
            }
            return str;
        }
    }
}
