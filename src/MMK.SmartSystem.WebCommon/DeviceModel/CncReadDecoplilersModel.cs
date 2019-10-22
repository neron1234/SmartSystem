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
        public List<R> Readers { get; set; } = new List<R>() ;

        public List<D> Decompilers { get; set; } = new List<D>();


    }
}
