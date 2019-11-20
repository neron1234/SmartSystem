using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Events.Bus;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.WebCommon.HubModel;
using Newtonsoft.Json.Linq;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation
{
    public class ProgramApiService : IProgramNotice
    {
        public event Action<HubReadWriterModel> RealReadWriterEvent;

        public bool CanWork(HubReadWriterResultModel resultModel)
        {
            return resultModel.Id == "getProgramFolder";
        }

        public void DoWork(HubReadWriterResultModel resultModel)
        {
            JObject jObject2 = JObject.Parse(resultModel.Result.ToString());
            GetProgramFolder(jObject2);
        }
        private void GetProgramFolder(JObject jObject)
        {
            ReadProgramFolderItemViewModel readProgramFolder = new ReadProgramFolderItemViewModel();
            if (jObject != null)
            {
                readProgramFolder.RegNum = (int)jObject["regNum"];
                readProgramFolder.Name = jObject["name"].ToString();
                readProgramFolder.Folder = jObject["folder"].ToString();
                var jArray = JArray.Parse(jObject["nodes"].ToString());

                ReadProgramFolderNode(jArray, readProgramFolder);
                ProgramConfigConsts.CurrentReadProgramFolder = readProgramFolder;
            }
        }

        private void ReadProgramFolderNode(JArray jArray, ReadProgramFolderItemViewModel node)
        {
            if (jArray == null) return;

            node.Nodes = new System.Collections.ObjectModel.ObservableCollection<ReadProgramFolderItemViewModel>();
            foreach (var item in jArray)
            {
                var childNode = new ReadProgramFolderItemViewModel
                {
                    RegNum = (int)item["regNum"],
                    Name = item["name"].ToString(),
                    Folder = item["folder"].ToString(),
                };
                node.Nodes.Add(childNode);
                ReadProgramFolderNode(JArray.Parse(item["nodes"].ToString()), childNode);
            }
        }
        public void Init(Action success = null)
        {
            RealReadWriterEvent?.Invoke(new HubReadWriterModel()
            {
                ProxyName = "ProgramFolderInOut",
                Action = "Reader",
                Id = "getProgramFolder",
                Data = new object[] { "//CNC_MEM/" }
            });

            Task.Factory.StartNew(new Action(() =>
            {
                EventBus.Default.Trigger(new ProgramClientEventData()
                {
                    SuccessAction = (s) =>
                    {
                        ProgramConfigConsts.CurrentProgramCommentFromCncDtos = s;
                        success?.Invoke();
                    }
                });
            }));

        }
    }
}
