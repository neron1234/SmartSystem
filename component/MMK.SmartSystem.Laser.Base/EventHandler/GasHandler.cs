using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.Common.SerivceProxy;
using System;

namespace MMK.SmartSystem.Laser.Base.EventHandler
{
    public class GasHandler : IEventHandler<GasEventData>, ITransientDependency
    {
        public void HandleEvent(GasEventData eventData)
        {
            GasClientServiceProxy gasClientService = new GasClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            string errorMessage = string.Empty;
            try
            {
                var rs = gasClientService.GetAllAsync(0, 50).Result;
                errorMessage = rs.Error?.Details;
                if (rs.Success)
                {
                    Messenger.Default.Send(rs.Result);
                }
                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = errorMessage,
                    ErrorAction = eventData.ErrorAction,
                    HashCode = eventData.HashCode
                });
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = ex.Message,
                    ErrorAction = eventData.ErrorAction,
                    HashCode = eventData.HashCode
                });
            }
        }
    }
}
