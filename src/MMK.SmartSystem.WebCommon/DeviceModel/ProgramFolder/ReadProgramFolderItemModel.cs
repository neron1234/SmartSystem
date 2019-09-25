using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public  class ReadProgramFolderItemModel
    {
        public string Name { get; set; }

        public string Folder { get; set; }

        public List<ReadProgramFolderItemModel> Nodes { get; set; } = new List<ReadProgramFolderItemModel>();

    }
}
