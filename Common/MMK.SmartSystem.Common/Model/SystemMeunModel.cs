using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.Model
{
    public class SystemMenuModule
    {
        public string ModuleName { get; set; }

        public string Icon { get; set; }

        public List<SystemPageModel> Pages { get; set; }

    }
    public class SystemPageModel
    {
        public string Title { get; set; }
        public bool WebPage { get; set; }

        public string Url { get; set; }
        public string Icon { get; set; }
        public int Sort { get; set; }
        public string FullName { get; set; }

        public string Permission { get; set; }

        public bool IsAuth { get; set; }
    }

   

}
