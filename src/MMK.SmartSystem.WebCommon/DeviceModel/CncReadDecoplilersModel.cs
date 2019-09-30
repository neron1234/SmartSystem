using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class CncReadDecoplilersModel<R, D> : Abp.Dependency.ITransientDependency
    {
        public List<R> Readers { get; set; } = new List<R>();

        public List<D> Decompilers { get; set; } = new List<D>();

        protected virtual void ReaderUnionOperation(R item) { }

        protected virtual void DecompilerUnionOperation(D item)
        {
            Decompilers.Add(item);

        }
        public void PollUnion(CncEventData eve)
        {
            var paraModel = JsonConvert.DeserializeObject<CncReadDecoplilersModel<R, D>>(eve.Para);

            foreach (var read in paraModel.Readers)
            {
                ReaderUnionOperation(read);
            }
            foreach (var item in paraModel.Decompilers)
            {
                DecompilerUnionOperation(item);
            }
        }
    }
}
