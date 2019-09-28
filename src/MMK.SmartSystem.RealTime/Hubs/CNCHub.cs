using Abp.AspNetCore.SignalR.Hubs;
using Abp.Auditing;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.RealTime;
using Microsoft.AspNetCore.SignalR;
using MMK.SmartSystem.CNC.Core.Workers;
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
        private IIocManager _iocManager;
        public const string GetDataAction = "GetCNCData";
        public const string GetErrorAction = "GetError";
        IServiceProvider service;
        public CNCHub(IOnlineClientManager onlineClientManager, IIocManager iocManager, IClientInfoProvider clientInfoProvider, IServiceProvider _service) :
          base(onlineClientManager, clientInfoProvider)
        {
            _iocManager = iocManager;
            this.service = _service;

        }

        public BaseCNCResultModel<ReadProgramListItemResultModel> ReadProgramList(string folder)
        {

            return new CncCoreWorker(_iocManager).ReadProgramList(folder);

        }

        public Task Refresh(string info)
        {
            List<CncEventData> cncEvents = new List<CncEventData>();
            try
            {

                cncEvents = JsonConvert.DeserializeObject<List<CncEventData>>(info);
                var hubClient = service.GetService(typeof(IHubContext<CncClientHub>)) as IHubContext<CncClientHub>;
                if (hubClient != null)
                {
                    hubClient.Clients.All.SendAsync(CncClientHub.ClientGetCncEvent, cncEvents);
                }
                Logger.Info($"【CncConfig】:{info}");
                foreach (var item in cncEvents)
                {
                    CncCoreWorker.m_EventDatas.Add(item);

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

            return base.OnConnectedAsync();
        }
    }
}
