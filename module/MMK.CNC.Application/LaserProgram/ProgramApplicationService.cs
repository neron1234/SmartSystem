using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Microsoft.AspNetCore.Http;
using MMK.CNC.Application.LaserProgram.Dto;
using MMK.CNC.Core.LaserProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.CNC.Application.LaserProgram
{
    public interface IProgramApplicationService : IAsyncCrudAppService<ProgramCommentFromCncDto, int, PagedResultRequestDto, CreateProgramDto, UpdateProgramDto>
    {
       
        Task<string> UploadProgram([SwaggerFileUpload]UploadProgramDto input);

        Task<string> UploadProgram2(IFormFile file);

    }
    public class ProgramApplicationService : AsyncCrudAppService<ProgramComment, ProgramCommentFromCncDto, int, PagedResultRequestDto, CreateProgramDto, UpdateProgramDto>, IProgramApplicationService
    {
        private readonly ICacheManager _cacheManager;

        public ProgramApplicationService(IRepository<ProgramComment, int> repository, ICacheManager cacheManager) : base(repository)
        {
            _cacheManager = cacheManager;
        }
        public override Task<ProgramCommentFromCncDto> Get(EntityDto<int> input)
        {

            return _cacheManager.GetCache("ProgramApplicationGet").Get(input.Id.ToString(), () => base.Get(input));

        }

        public async Task<string> UploadProgram([SwaggerFileUpload] UploadProgramDto input)
        {
            var stream = input.File.OpenReadStream();
            await Task.CompletedTask;
            return "";
        }

        public async Task<string> UploadProgram2(IFormFile file)
        {
            var stream = file.OpenReadStream();
            await Task.CompletedTask;
            return "";
        }
    }
}
