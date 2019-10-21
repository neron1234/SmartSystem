using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.DeviceModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHandlers
{
    public class MacroHandler : BasePollCNCHandler<ReadMacroModel, ReadMacroResultItemModel, ReadMacroTypeModel, DecompReadMacroItemModel>
    {
        Dictionary<CncMacroTypeEnum, double[]> datas;
        public MacroHandler() 
        {
            datas = new Dictionary<CncMacroTypeEnum, double[]>();
        }
        protected override object PollDecompiler(List<ReadMacroResultItemModel> res, DecompReadMacroItemModel deModel)
        {
            double data = 0;
            var ret_dec = new MacroHelper().DecompilerReadMacroInfo(datas, deModel, ref data);
            if (string.IsNullOrEmpty(ret_dec))
            {
                res.Add(new ReadMacroResultItemModel() { Id = deModel.Id, Value = data });
            }
            return ret_dec;
        }

        protected override Tuple<short, string> PollRead(ReadMacroTypeModel inputModel)
        {
            var qty = inputModel.EndNum - inputModel.StartNum + 1;

            var temp_data = new double[qty];

            var ret = new MacroHelper().ReadMacroRange(flib, inputModel.StartNum, qty, ref temp_data);
            if (ret.Item1 == 0)
            {
                if (datas.ContainsKey(inputModel.Type))
                {
                    datas[inputModel.Type] = temp_data;
                }
                else
                {
                    datas.Add(inputModel.Type, temp_data);

                }
            }

            return ret;
        }

        public override ReadMacroModel MargePollRequest(ReadMacroModel pre, ReadMacroModel current)
        {
            foreach(var item in current.Decompilers)
            {
                var macroType = GetMacroType(item.StartNum);

                var pre_item = pre.Readers.Where(x => x.Type == macroType).FirstOrDefault();
                if (pre_item!=null)
                {
                    pre.Decompilers.Add(new DecompReadMacroItemModel()
                    {
                        Id = item.Id,
                        StartNum = item.StartNum,
                        Type = macroType,
                        RelStartNum = (short)(item.StartNum - pre_item.StartNum)
                    });

                    if (pre_item.StartNum > item.StartNum)
                    {
                        pre_item.StartNum = item.StartNum;

                        var decomps = pre.Decompilers.Where(x => x.Type == macroType).ToList() ?? new List<DecompReadMacroItemModel>();
                        foreach (var dItem in decomps)
                        {
                            item.RelStartNum = (short)(dItem.StartNum - pre_item.StartNum);
                        }
                    }

                    pre_item.EndNum = pre_item.EndNum < item.StartNum ? item.StartNum : pre_item.EndNum;
                }
                else
                {
                    pre.Readers.Add(new ReadMacroTypeModel()
                    {

                        StartNum= item.StartNum,
                        EndNum=item.StartNum,
                        Type= macroType
                    });

                    pre.Decompilers.Add(new DecompReadMacroItemModel()
                    {
                        Id = item.Id,
                        StartNum = item.StartNum,
                        Type = macroType,
                        RelStartNum = 0
                    });
                }
            }

            

            //var macro = pre.Readers[0];
            //var start = current.Readers[0].StartNum > macro.StartNum ? current.Readers[0].StartNum : macro.StartNum;
            //var end = (current.Readers[0].StartNum + current.Readers[0].Quantity)
            //    > (macro.StartNum + macro.Quantity) ? (current.Readers[0].StartNum + current.Readers[0].Quantity) :
            //    (macro.StartNum + macro.Quantity);

            //macro.StartNum = start;
            //macro.Quantity = end - start;

            //pre.Decompilers.AddRange(current.Decompilers);

            //foreach (var item in pre.Decompilers)
            //{
            //    item.RelStartNum = (short)(item.StartNum - start);
            //}

            return pre;
        }

        private CncMacroTypeEnum GetMacroType(ushort num)
        {
            if (1 <= num && num <= 33) return CncMacroTypeEnum.LOCAL;
            else if (100 <= num && num <= 199) return CncMacroTypeEnum.VOLATILE;
            else if (500 <= num && num <= 999) return CncMacroTypeEnum.NONVOLATILE;
            else if (6000 <= num && num <= 6300) return CncMacroTypeEnum.LASER_COMMON;
            else return CncMacroTypeEnum.OTHER;
        }

    }
}
