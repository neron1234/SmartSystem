using Abp.AspNetCore.SignalR.Hubs;
using Abp.Auditing;
using Abp.BackgroundJobs;
using Abp.RealTime;
using MMK.SmartSystem.RealTime.DeviceHandlers;
using MMK.SmartSystem.RealTime.Job;
using MMK.SmartSystem.WebCommon.DeviceModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.RealTime.Hubs
{
    public class CNCHub : AbpCommonHub
    {
        public static bool IsFirstConnect = true;
        public const string GetDataAction = "GetCNCData";
        public const string GetErrorAction = "GetError";
        private readonly IBackgroundJobManager _backgroundJobManager;
        public CNCHub(IOnlineClientManager onlineClientManager, IClientInfoProvider clientInfoProvider, IBackgroundJobManager backgroundJobManager) :
          base(onlineClientManager, clientInfoProvider)
        {
            _backgroundJobManager = backgroundJobManager;

        }

        public Task Refresh(string info)
        {
            List<CncEventData> cncEvents = new List<CncEventData>();
            try
            {
                cncEvents = JsonConvert.DeserializeObject<List<CncEventData>>(info);
                Logger.Info($"【CncConfig】:{info}");
                foreach (var item in cncEvents)
                {
                    CncHandler.m_EventDatas.Add(item);

                }
            }
            catch (Exception ex)
            {

                Logger.Error(ex.Message);
            }

            return Task.CompletedTask;
        }

        

        public override Task OnConnectedAsync()
        {
            if (IsFirstConnect)
            {
                _backgroundJobManager.Enqueue<CncBackgroudJob, CncBackgroudArgs>(new CncBackgroudArgs());
                IsFirstConnect = false;
            }
            return base.OnConnectedAsync();
        }
    }
}
