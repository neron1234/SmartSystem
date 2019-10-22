using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.Configs
{
    public class HomeEventDataConfig
    {
        public List<CncEventData> GetInitEventData()
        {
            return new List<CncEventData>()
            {
                GetPositionEventData(),
                GetProgramEventData(),
                GetProgramNameEventData(),
                GetProgramBlockEventData(),
                GetFeedrateEventData(),
                GetMacroEventData()
            };
        }

        private CncEventData GetPositionEventData()
        {
            return new CncEventData()
            {
                Kind = CncEventEnum.ReadPosition,
                Para = Newtonsoft.Json.JsonConvert.SerializeObject(new ReadPositionModel()
                {
                    Decompilers = new List<DecompReadPositionItemModel>()
                     {
                          new DecompReadPositionItemModel() {Id="positonX", AxisNum=1,PositionType=CncPositionTypeEnum.Absolute },
                          new DecompReadPositionItemModel() {Id="positonY", AxisNum=2,PositionType=CncPositionTypeEnum.Absolute  },
                          new DecompReadPositionItemModel() {Id="positonZ",AxisNum=3,PositionType=CncPositionTypeEnum.Absolute }
                     },
                    Readers = new List<ReadPositionTypeModel>()
                    {
                          new ReadPositionTypeModel(){ PositionType=CncPositionTypeEnum.Absolute}
                    }
                })
            };
        }

        private CncEventData GetProgramEventData()
        {
            return new CncEventData()
            {
                Kind = CncEventEnum.ReadProgramStr,
                Para = Newtonsoft.Json.JsonConvert.SerializeObject(new ReadProgramStrModel()
                {
                    Decompilers = new List<string>()
                    {
                        "Home-Program"
                    },
                    Readers = new List<string>()
                    {
                        "programStrControl"
                    }
                })
            };
        }

        private CncEventData GetProgramNameEventData()
        {
            return new CncEventData()
            {
                Kind = CncEventEnum.ReadProgramName,
                Para = Newtonsoft.Json.JsonConvert.SerializeObject(new ReadProgramNameModel()
                {

                    Decompilers = new List<string>()
                    {
                        "Home-ProgramName"
                    }
                })
            };
        }

        private CncEventData GetProgramBlockEventData()
        {
            return new CncEventData()
            {
                Kind = CncEventEnum.ReadProgramBlock,
                Para = Newtonsoft.Json.JsonConvert.SerializeObject(new ReadProgramBlockModel()
                {

                    Decompilers = new List<string>()
                    {
                        "Home-ProgramBlock"
                    }
                })
            };

        }
        private CncEventData GetFeedrateEventData()
        {
            return new CncEventData()
            {

                Kind = CncEventEnum.ReadFeedrate,
                Para = Newtonsoft.Json.JsonConvert.SerializeObject(new ReadFeedrateModel()
                {
                    Decompilers = new List<string>()
                    {
                        "Home-Feedrate"
                    },
                    Readers = new List<string>()
                    {
                        "homeFeedrate"
                    }
                })
            };
        }

        private CncEventData GetMacroEventData()
        {
            return new CncEventData()
            {
                Kind = CncEventEnum.ReadMacro,
                Para = Newtonsoft.Json.JsonConvert.SerializeObject(new ReadMacroModel()
                {
                    Decompilers = new List<DecompReadMacroItemModel>()
                    {
                        new DecompReadMacroItemModel(){ Id="macroPc",StartNum=500},
                        new DecompReadMacroItemModel(){ Id="macroFr",StartNum=501},
                        new DecompReadMacroItemModel(){ Id="macroDu",StartNum=502},
                        new DecompReadMacroItemModel(){ Id="macroPa",StartNum=503}
                    }
                })
            };
        }
    }
}
