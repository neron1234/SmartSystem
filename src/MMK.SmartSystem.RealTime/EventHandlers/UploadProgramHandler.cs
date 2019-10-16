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
            string bmpPath = Path.Combine(_hostingEnvironment.WebRootPath, "Upload", "BMP");
            string saveFullName = "";
            if (Directory.Exists(savePath))
            {
                DirectoryInfo root = new DirectoryInfo(savePath);
                var nameCount = root.GetFiles().Where(n => n.Name == eventData.FullName).Count();
                saveFullName = nameCount > 0 ? eventData.FullName + nameCount : eventData.FullName;

                using (var stream = new FileStream(Path.Combine(savePath, saveFullName), FileMode.Create))
                {

                    eventData.FileStream.CopyToAsync(stream).Wait();
                }

            }
            hubContext.Clients.All.SendAsync(CncClientHub.ClientReadProgram, new ProgramResovleDto()
            {
                BmpPath = bmpPath,
                FileName = saveFullName,  //从文件信息里面获取文件名不要后缀
                FilePath = Path.Combine(savePath, saveFullName),
            });
        }
    }
}
