using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadPmcModel : CncReadDecoplilersModel<ReadPmcTypeModel, DecompReadPmcItemModel>
    {
        protected override void ReaderUnionOperation(ReadPmcTypeModel read)
        {
            var temp_read = Readers.Where(x => x.AdrType == read.AdrType).FirstOrDefault();
            if (temp_read != null)
            {
                var start = temp_read.StartNum < read.StartNum ? temp_read.StartNum : read.StartNum;
                var end = (temp_read.StartNum + temp_read.DwordQuantity) > (read.StartNum + read.DwordQuantity) ? (temp_read.StartNum + temp_read.DwordQuantity) : (read.StartNum + read.DwordQuantity);

                temp_read.StartNum = start;
                temp_read.DwordQuantity = (ushort)(end - start);
            }
            else
            {
                Readers.Add(read);
            }

        }
        

    }
}
