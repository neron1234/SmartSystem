using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.HubModel
{
    public class ProgramResolveDto
    {
        public string ImagePath { get; set; }

        public ProgramDetailDto Data { get; set; }
    }

    public class ProgramDetailDto
    {
        public string Name { get; set; }

        public string FullPath { get; set; }

        public double Size { get; set; }

        public string Material { get; set; }

        public double Thickness { get; set; }

        public string Gas { get; set; }

        public double FocalPosition { get; set; }

        public string NozzleKind { get; set; }

        public double NozzleDiameter { get; set; }

        public string PlateSize { get; set; }

        public string UsedPlateSize { get; set; }

        public double CuttingDistance { get; set; }

        public int PiercingCount { get; set; }

        public double CuttingTime { get; set; }

        public int ThumbnaiType { get; set; }

        public string ThumbnaiInfo { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
