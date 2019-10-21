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
            return new List<CncEventData>() { GetPositionEventData() };
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
    }
}
