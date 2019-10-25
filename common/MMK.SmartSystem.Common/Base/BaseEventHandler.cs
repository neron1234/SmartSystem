using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Events.Bus.Handlers;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common
{
    public abstract class BaseEventHandler<T, U> : IEventHandler<T>, ITransientDependency where T : BaseApiEventData<U> where U : class
    {
        public abstract RequestResult<U> WebRequest(T eventData);

        protected string apiHost = SmartSystemCommonConsts.ApiHost;
        protected HttpClient httpClient = new HttpClient();

        public void HandleEvent(T eventData)
        {
            try
            {
                var result = WebRequest(eventData);
                if (result == null)
                {
                    return;
                }
                string errorMessage = result.Error?.Details;
                if (result.Success)
                {
                    eventData.SuccessAction?.Invoke(result.Result);
                    return;
                }
                eventData.ErrorAction?.Invoke();
                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = errorMessage,
                });
            }
            catch (ApiException apiExcaption)
            {
                eventData.ErrorAction?.Invoke();
                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = apiExcaption.Message,

                });
            }
            catch (Exception ex)
            {
                eventData.ErrorAction?.Invoke();

                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = ex.Message + ex.InnerException?.Message,


                });
            }
        }
    }
}
