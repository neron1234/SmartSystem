using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.Common.SerivceProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.EventHandler
{
    public class UpdateCuttingDataHandler : IEventHandler<UpdateCuttingDataEventData>, ITransientDependency
    {
        public void HandleEvent(UpdateCuttingDataEventData eventData)
        {
            CuttingDataClientServiceProxy cuttingDataClientServiceProxy = new CuttingDataClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            string errorMessage = string.Empty;
            try
            {
                var rs = cuttingDataClientServiceProxy.UpdateAsync(eventData.UpdateCuttingData).Result;
                errorMessage = rs.Error?.Details;
                if (rs.Success)
                {
                    //Messenger.Default.Send(rs.Result);
                    Messenger.Default.Send(new MainSystemNoticeModel
                    {
                        Tagret = eventData.Tagret,
                        Success = true,
                        SuccessAction = eventData.SuccessAction,
                        HashCode = eventData.HashCode
                    });
                }
                else
                {
                    Messenger.Default.Send(new MainSystemNoticeModel
                    {
                        Tagret = eventData.Tagret,
                        Error = errorMessage,
                        ErrorAction = eventData.ErrorAction,
                        HashCode = eventData.HashCode
                    });
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = ex.ToString(),
                    ErrorAction = eventData.ErrorAction,
                    HashCode = eventData.HashCode
                });
            }
        }
    }

    public class CuttingDataByGroupIdHandler : IEventHandler<CuttingDataByGroupIdEventData>, ITransientDependency
    {
        public void HandleEvent(CuttingDataByGroupIdEventData eventData)
        {
            CuttingDataClientServiceProxy cuttingDataClientServiceProxy = new CuttingDataClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            string errorMessage = string.Empty;
            try
            {
                var rs = cuttingDataClientServiceProxy.GetAllAsync(eventData.machiningDataGroupId, 0, 50).Result;
                errorMessage = rs.Error?.Details;
                if (rs.Success)
                {
                    eventData?.SuccessAction?.Invoke(rs.Result.Items.ToList());
                    return;
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
                    Error = ex.ToString(),
                    ErrorAction = eventData.ErrorAction,
                    HashCode = eventData.HashCode
                });
            }
        }
    }
}
