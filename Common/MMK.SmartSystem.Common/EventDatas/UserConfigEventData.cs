using Abp.Events.Bus;
using MMK.SmartSystem.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.EventDatas
{

    public class BaseApiEventData<T> : EventData
    {
        public ErrorTagretEnum Tagret { get; set; }

        public int HashCode { get; set; }
        public Action<T> SuccessAction { set; get; }

        public Action ErrorAction { set; get; }
    }

    public class BaseErrorEventData : EventData
    {
        public ErrorTagretEnum Tagret { get; set; }

        public int HashCode { get; set; }

        public Action SuccessAction { set; get; }

        public Action ErrorAction { set; get; }

    }

    public class UserInfoEventData : BaseErrorEventData
    {
        public int UserId { get; set; }
    }
    public class UserLoginEventData : BaseErrorEventData
    {
        public string UserName { get; set; }

        public string Pwd { get; set; }
    }

    public class UserLanguageEventData : BaseErrorEventData
    {
        public string Culture { get; set; }

    }
}
