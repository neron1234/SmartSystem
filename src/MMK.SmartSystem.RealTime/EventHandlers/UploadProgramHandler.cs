using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using MMK.SmartSystem.RealTime.Hubs;
using MMK.SmartSystem.WebCommon.EventModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.RealTime.EventHandlers
{
    public class UploadProgramHandler : IEventHandler<UploadProgramEventData>, ITransientDependency
    {
        IHostingEnvironment _hostingEnvironment;
        IHubContext<CncClientHub> hubContext;

        public UploadProgramHandler(IHostingEnvironment hostingEnvironment, IServiceProvider service)
        {
            _hostingEnvironment = hostingEnvironment;
            hubContext = service.GetService(typeof(IHubContext<CncClientHub>)) as IHubContext<CncClientHub>;

        }
        public void HandleEvent(UploadProgramEventData eventData)
        {
            string savePath = Path.Combine(_hostingEnvironment.WebRootPath, "Upload", "CNCProgram");
            string saveFullName = "";

            if (Directory.Exists(savePath)){
                DirectoryInfo root = new DirectoryInfo(savePath);
                var nameCount = root.GetFiles().Where(n => n.Name == eventData.FullName).Count();
                saveFullName = nameCount > 0 ? eventData.FullName + nameCount : eventData.FullName;
                System.Runtime.Remoting.MetadataServices.MetaData.SaveStreamToFile(eventData.FileStream, Path.Combine(savePath, saveFullName));
            }
            hubContext.Clients.All.SendAsync(CncClientHub.ClientReadProgram, saveFullName);

        }
    }
}
