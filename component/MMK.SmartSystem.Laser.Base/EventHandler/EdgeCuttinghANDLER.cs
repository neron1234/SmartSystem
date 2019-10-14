﻿using Abp.Dependency;
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
    public class UpdateEdgeCuttingHandler : IEventHandler<UpdateEdgeCuttingEventData>, ITransientDependency
    {
        public void HandleEvent(UpdateEdgeCuttingEventData eventData)
        {
            EdgeCuttingClientServiceProxy edgeCuttingClientServiceProxy = new EdgeCuttingClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            string errorMessage = string.Empty;
            try
            {
                var rs = edgeCuttingClientServiceProxy.UpdateAsync(eventData.UpdateEdgeCuttingData).Result;
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

    public class EdgeCuttingHandler : IEventHandler<EdgeCuttingByGroupIdEventData>, ITransientDependency
    {
        public void HandleEvent(EdgeCuttingByGroupIdEventData eventData)
        {
            EdgeCuttingClientServiceProxy edgeCuttingClientServiceProxy = new EdgeCuttingClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            string errorMessage = string.Empty;
            try
            {
                var rs = edgeCuttingClientServiceProxy.GetAllAsync(eventData.machiningDataGroupId,0,50).Result;
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
                    Error = ex.ToString(),
                    ErrorAction = eventData.ErrorAction,
                    HashCode = eventData.HashCode
                });
            }
        }
    }
}