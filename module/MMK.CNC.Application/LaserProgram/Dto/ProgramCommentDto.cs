using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using MMK.CNC.Core.LaserProgram;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.CNC.Application.LaserProgram.Dto
{
    [AutoMap(typeof(ProgramComment))]
    public class ProgramCommentFromCncDto : EntityDto<int>
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

    [AutoMap(typeof(ProgramComment))]
    public class CreateProgramDto
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
    }

    [AutoMap(typeof(ProgramComment))]
    public class UpdateProgramDto : EntityDto<int>
    {
        public string Name { get; set; }

        public string FullPath { get; set; }

        public double Size { get; set; }

        public string Material { get; set; }

        public double Thickness { get; set; }

        public string Gas { get; set; }
    }
}
