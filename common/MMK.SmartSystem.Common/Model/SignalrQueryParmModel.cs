using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.Model
{
    public class SignalrQueryParmModel
    {
        public string Module { get; set; }
        public List<SignlarPageParm> Pages { set; get; }
    }

    public class SignlarPageParm
    {
        public string PageName { get; set; }

        public List<PageParmEventNode> EventNodes { get; set; }

    }

    public class PageParmEventNode
    {
        public string Kind { get; set; }

        public string Type { get; set; }

        public ParmDescriptionNode CncReadDecopliler { get; set; }
    }


    public class ParmDescriptionNode {
        public ParmItemNode Readers { get; set; }

        public ParmItemNode Decompilers { get; set; }
    }
    public class ParmItemNode
    {
        public string Type { get; set; }

        public List<object> Data { get; set; }
    }
}
