using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.LE.Host.ViewModel
{
    public class SystemTranslate
    {
        public SmartSystem SmartSystem { get; set; } = new SmartSystem();

    }

    public class SmartSystem
    {
        public string Account { get; set; }

        public string Pwd { get; set; }

    }
}
