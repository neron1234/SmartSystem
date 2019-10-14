using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using MMK.CNC.Core.LaserLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.CNC.Application.LaserLibrary.Dto
{
    public class BaseDataService : ITransientDependency
    {
        public IRepository<Gas, int> GasRepository { set; get; }
        public IRepository<Material, int> MaterialRepository { get; set; }
        public IRepository<MachiningDataGroup, int> MachiningDataGroupRepository { get; set; }
        public IRepository<MachiningKind, int> MachiningKindRepository { get; set; }
        public IRepository<NozzleKind, int> NozzleKindRepository { get; set; }

        private readonly ICacheManager _cacheManager;

        public BaseDataService(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public MachiningDataGroup GetMachiningData(int id)
        {
            var machining = _cacheManager.GetCache("MachiningDataGroup").Get("Default", () => MachiningDataGroupRepository.GetAllIncluding().ToDictionary(d => d.Id));
            if (machining.ContainsKey(id))
            {
                return machining[id];
            }
            return default;
        }

        public Gas GetGas(short id)
        {
            var gas = _cacheManager.GetCache("Gas").Get("Default", () => GasRepository.GetAllIncluding().ToDictionary(d => d.Code));
            if (gas.ContainsKey(id))
            {
                return gas[id];
            }
            return default;

        }

        public MachiningKind GetMachiningKind(short id)
        {
            var machiningKind = _cacheManager.GetCache("MachiningKind").Get("Default", () => MachiningKindRepository.GetAllIncluding().ToDictionary(d => d.Code));
            if (machiningKind.ContainsKey(id))
            {
                return machiningKind[id];
            }
            return default;
        }

        public Material GetMaterial(int id)
        {
            var material = _cacheManager.GetCache("Material").Get("Default", () => MaterialRepository.GetAllIncluding().ToDictionary(d => d.Code));
            if (material.ContainsKey(id))
            {
                return material[id];
            }
            return default;
        }

        public NozzleKind GetNozzleKind(short id)
        {
            var nozzleKind = _cacheManager.GetCache("NozzleKind").Get("Default", () => NozzleKindRepository.GetAllIncluding().ToDictionary(d => d.Code));
            if (nozzleKind.ContainsKey(id))
            {
                return nozzleKind[id];
            }
            return default;
        }
    }

}
