using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Microsoft.AspNetCore.Http;
using MMK.CNC.Core.LaserProgram;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// <summary>
        /// 程序名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 程序路径
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// 程序大小
        /// </summary>
        public double Size { get; set; }

        /// <summary>
        /// 材料类型
        /// </summary>
        public string Material { get; set; }

        /// <summary>
        /// 材料厚度
        /// </summary>
        public double Thickness { get; set; }

        /// <summary>
        /// 辅助气体种类
        /// </summary>
        public string Gas { get; set; }

        /// <summary>
        /// 焦点位置
        /// </summary>
        public double FocalPosition { get; set; }

        /// <summary>
        /// 割嘴类型
        /// </summary>
        public string NozzleKind { get; set; }

        /// <summary>
        /// 割嘴内径
        /// </summary>
        public double NozzleDiameter { get; set; }

        /// <summary>
        /// 板材尺寸
        /// </summary>
        public string PlateSize { get; set; }

        /// <summary>
        /// 已用板材尺寸
        /// </summary>
        public string UsedPlateSize { get; set; }

        /// <summary>
        /// 切割总长度
        /// </summary>
        public double CuttingDistance { get; set; }

        /// <summary>
        /// 穿孔总次数
        /// </summary>
        public int PiercingCount { get; set; }

        /// <summary>
        /// 加工时间
        /// </summary>
        public double CuttingTime { get; set; }

        /// <summary>
        /// 缩略图类型
        /// </summary>
        public int ThumbnaiType { get; set; }

        public string ThumbnaiInfo { get; set; }

        /// <summary>
        /// 程序更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        public CreateProgramDto()
        {
            UpdateTime = DateTime.Now;
        }

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

    public class UploadProgramDto
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
