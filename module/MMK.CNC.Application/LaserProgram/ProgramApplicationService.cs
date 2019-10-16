using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Events.Bus;
using Abp.Runtime.Caching;
using Microsoft.AspNetCore.Http;
using MMK.CNC.Application.LaserProgram.Dto;
using MMK.CNC.Core.LaserProgram;
using MMK.SmartSystem.WebCommon.EventModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.CNC.Application.LaserProgram
{
    public interface IProgramApplicationService : IAsyncCrudAppService<ProgramCommentFromCncDto, int, PagedResultRequestDto, CreateProgramDto, UpdateProgramDto>
    {


        Task<string> UploadProgram(IFormFile file);

    }
    public class ProgramApplicationService : AsyncCrudAppService<ProgramComment, ProgramCommentFromCncDto, int, PagedResultRequestDto, CreateProgramDto, UpdateProgramDto>, IProgramApplicationService
    {
        private readonly ICacheManager _cacheManager;


        public ProgramApplicationService(IRepository<ProgramComment, int> repository,
            ICacheManager cacheManager) : base(repository)
        {
            _cacheManager = cacheManager;
        }
        public override Task<ProgramCommentFromCncDto> Get(EntityDto<int> input)
        {

            return _cacheManager.GetCache("ProgramApplicationGet").Get(input.Id.ToString(), () => base.Get(input));

        }
        public override async Task<ProgramCommentFromCncDto> Update(UpdateProgramDto input)
        {
            var defaultCode = Repository.FirstOrDefault(d => d.Name == input.Name);
            var entity = ObjectMapper.Map<ProgramComment>(input);

            if (defaultCode == null)
            {
                await Repository.InsertAsync(entity);

            }
            else
            {
                entity.Id = defaultCode.Id;
                await Repository.UpdateAsync(entity);


            }
            return ObjectMapper.Map<ProgramCommentFromCncDto>(entity);
        }


        public async Task<string> UploadProgram(IFormFile file)
        {
            var stream = file.OpenReadStream();

            await EventBus.Default.TriggerAsync(new UploadProgramEventData() { FullName = file.Name,FileStream = stream });
            return "True";
        }
    }
}
