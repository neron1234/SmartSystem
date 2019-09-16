using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.LE.Host.SystemControl.ViewModel
{
    public enum PageEnum
    { 
        WPFPage,
        WebPage
    }
    public class PageChangeModel
    {
        public Type FullType { get; set; }

        public PageEnum Page { get; set; }

    }
}
