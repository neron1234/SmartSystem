using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.Configs
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
                GetMacroEventData(),
                GetPmcEventData(),
                GetCycleTimeEventData()
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
                    },
                    Readers = new List<string>() { "programName" }
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
                    },
                    Readers = new List<string>() { "programBlock" }
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
                        new DecompReadMacroItemModel(){ Id="Home-macroPc",StartNum=1},
                        new DecompReadMacroItemModel(){ Id="Home-macroFr",StartNum=501},
                        new DecompReadMacroItemModel(){ Id="Home-macroDu",StartNum=502},
                        new DecompReadMacroItemModel(){ Id="Home-macroPa",StartNum=503}
                    },
                    Readers = new List<ReadMacroTypeModel>()
                    {
                        new ReadMacroTypeModel(){ }
                    }
                })
            };
        }

        private CncEventData GetPmcEventData()
        {

            return new CncEventData()
            {
                Kind = CncEventEnum.ReadPmc,
                Para = Newtonsoft.Json.JsonConvert.SerializeObject(new ReadPmcModel()
                {
                    Decompilers = new List<DecompReadPmcItemModel>()
                    {
                        new DecompReadPmcItemModel(){Id="Home-alarmState",AdrType=12,DataType=DataTypeEnum.Boolean,StartAdr= 1002,Bit=3 },
                        new DecompReadPmcItemModel(){Id="Home-cncState",AdrType=12,DataType=DataTypeEnum.Int16,StartAdr= 1000,Bit=0},
                        new DecompReadPmcItemModel(){Id="Home-xState",AdrType=12,DataType=DataTypeEnum.Boolean,StartAdr= 1002,Bit=0},
                        new DecompReadPmcItemModel(){Id="Home-yState",AdrType=12,DataType=DataTypeEnum.Boolean,StartAdr= 1002,Bit=1},
                        new DecompReadPmcItemModel(){Id="Home-zState",AdrType=12,DataType=DataTypeEnum.Boolean,StartAdr= 1002,Bit=2},
                        new DecompReadPmcItemModel(){Id="Home-feedov",AdrType=12,DataType=DataTypeEnum.Int16,StartAdr= 1004,Bit=0},
                        new DecompReadPmcItemModel(){Id="Home-finshTime",AdrType=12,DataType=DataTypeEnum.Int32,StartAdr= 1008,Bit=0}
                    },
                    Readers = new List<ReadPmcTypeModel>() { new ReadPmcTypeModel() }
                })
            };
        }

        private CncEventData GetCycleTimeEventData()
        {

            return new CncEventData()
            {
                Kind = CncEventEnum.ReadCycleTime,
                Para = Newtonsoft.Json.JsonConvert.SerializeObject(new ReadCycleTimeModel()
                {
                    Decompilers = new List<string>() { "Home-cycleTime" },
                    Readers = new List<string>() { "homeCycleTime" }


                })

            };
        }
    }
}
