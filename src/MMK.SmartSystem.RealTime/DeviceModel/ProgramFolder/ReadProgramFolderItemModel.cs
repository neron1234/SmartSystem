using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.RealTime.DeviceModel.ProgramFolder
{
    public  class ReadProgramFolderItemModel
    {
        public string Name { get; set; }

        public List<ReadProgramFolderItemModel> Nodes { get; set; } = new List<ReadProgramFolderItemModel>();

    }
}
