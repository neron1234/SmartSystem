﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using MMK.CNC.Application.LaserLibrary.Dto;
using MMK.CNC.Core.LaserLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.CNC.Application.LaserLibrary
{
    public interface IPiercingDataApplicationService: IAsyncCrudAppService<PiercingDataDto,int, PiercingDataResultRequestDto, CreatePiercingDataDto,UpdatePiercingDataDto>
    {
    }

    public class PiercingDataApplicationService:AsyncCrudAppService<PiercingData,PiercingDataDto,int, PiercingDataResultRequestDto, CreatePiercingDataDto,UpdatePiercingDataDto>, IPiercingDataApplicationService
    {
        public IRepository<Gas, int> GasRepository { set; get; }
        public IRepository<Material, int> MaterialRepository { get; set; }
        public IRepository<MachiningDataGroup, int> MachiningDataGroupRepository { get; set; }
        public IRepository<MachiningKind, int> MachiningKindRepository { get; set; }
        public IRepository<NozzleKind, int> NozzleKindRepository { get; set; }

        IRepository<PiercingData, int> repository;
        public PiercingDataApplicationService(IRepository<PiercingData, int> repository) : base(repository)
        {
            this.repository = repository;
        }

        protected override PiercingDataDto MapToEntityDto(PiercingData entity)
        {
            var resDto = AutoMapper.Mapper.Map<PiercingDataDto>(entity);
            resDto.GasName = GasRepository.FirstOrDefault(d => d.Id == resDto.GasId)?.Name_CN;


            var mGroup = MachiningDataGroupRepository.FirstOrDefault(d => d.Id == resDto.MachiningDataGroupId);

            resDto.MachiningKindName = MachiningKindRepository.FirstOrDefault(d => d.Id == resDto.MachiningKindId)?.Name_CN;
            resDto.MaterialName = MaterialRepository.FirstOrDefault(d => d.Id == mGroup.MaterialId)?.Name_CN;
            resDto.NozzleKindName = NozzleKindRepository.FirstOrDefault(d => d.Id == resDto.NozzleKindId)?.Name_CN;

            resDto.MaterialThickness = (double)mGroup?.MaterialThickness;
            return resDto;
        }
    }
}
