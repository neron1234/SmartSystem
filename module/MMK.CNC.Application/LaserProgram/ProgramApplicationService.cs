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


        Task<string> UploadProgram(IFormFile file, string connectId, string fileHashCode);

    }
    public class ProgramApplicationService : AsyncCrudAppService<ProgramComment, ProgramCommentFromCncDto, int, PagedResultRequestDto, CreateProgramDto, UpdateProgramDto>, IProgramApplicationService
    {
        private readonly ICacheManager _cacheManager;


        public ProgramApplicationService(IRepository<ProgramComment, int> repository,
            ICacheManager cacheManager) : base(repository)
        {
            _cacheManager = cacheManager;
        }
        public override Task<ProgramCommentFromCncDto> GetAsync(EntityDto<int> input)
        {

            return _cacheManager.GetCache("ProgramApplicationGet").Get(input.Id.ToString(), () => base.GetAsync(input));

        }

        public override async Task<ProgramCommentFromCncDto> UpdateAsync(UpdateProgramDto input)
        {
            var defaultCode = Repository.FirstOrDefault(d => d.FileHash == input.FileHash);
            var entity = ObjectMapper.Map<ProgramComment>(input);

            if (defaultCode == null)
            {
                await Repository.InsertAsync(entity);

            }
            else
            {
                var props = defaultCode.GetType().GetProperties();
                var entityType = entity.GetType();

                foreach (var item in props)
                {
                    if (item.Name == "CreationTime" || item.Name == "Id")
                    {
                        continue;
                    }
                    var value = entityType.GetProperty(item.Name).GetValue(entity);
                    if (value != null)
                    {
                        try
                        {
                            var t = item.PropertyType== typeof(double?)?typeof(double):item.PropertyType;
                            item.SetValue(defaultCode, Convert.ChangeType(value ?? 0, t));

                        }
                        catch (Exception ex)
                        {


                        }

                    }
                }




                await Repository.UpdateAsync(defaultCode);


            }
            return ObjectMapper.Map<ProgramCommentFromCncDto>(entity);
        }


        public async Task<string> UploadProgram(IFormFile file, string connectId, string fileHashCode)
        {
            var stream = file.OpenReadStream();

            await EventBus.Default.TriggerAsync(new UploadProgramEventData() { FullName = file.FileName, FileStream = stream, ConnectId = connectId, FileHash = fileHashCode });
            return "True";
        }
    }
}
