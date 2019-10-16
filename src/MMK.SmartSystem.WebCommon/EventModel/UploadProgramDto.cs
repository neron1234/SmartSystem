﻿using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.EventModel
{
    public class UploadProgramEventData : EventData
    {
        public string FullName { get; set; }

        public Stream FileStream { set; get; }

    }

    public class ProgramResovleDto
    {
        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string BmpPath { get; set; }
    }
}
