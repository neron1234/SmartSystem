using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.Common.SerivceProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.EventHandler
{
    public class MaterialInfoHandler : IEventHandler<MaterialInfoEventData>, ITransientDependency
    {
        public void HandleEvent(MaterialInfoEventData eventData)
        {
            MaterialClientServiceProxy materialClientServiceProxy = new MaterialClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            string errorMessage = string.Empty;
            try
            {
                var rs = materialClientServiceProxy.GetAllAsync(0, 50).Result;
                errorMessage = rs.Error?.Details;
                if (rs.Success)
                {
                    MachiningGroupClientServiceProxy machiningGroupClientServiceProxy = new MachiningGroupClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());

                    List<MaterialDto> materials = new List<MaterialDto>();
                    foreach (var item in rs.Result.Items)
                    {
                        if (!eventData.IsAll)
                        {
                            var mgrs = machiningGroupClientServiceProxy.GetAllAsync(item.Id, 0, 50).Result;
                            errorMessage = rs.Error?.Details;
                            if (mgrs.Success && mgrs.Result.Items.Count > 0)
                            {
                                materials.Add(item);
                            }
                        }
                        else
                        {
                            materials.Add(item);
                        }
                    }
                    Messenger.Default.Send(materials);
                }
                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = errorMessage,
                    ErrorAction = eventData.ErrorAction,
                    HashCode = eventData.HashCode
                });
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = ex.Message,
                    ErrorAction = eventData.ErrorAction,
                    HashCode = eventData.HashCode

                });
            }
        }
    }

    public class AddMaterialHandler : IEventHandler<AddMaterialEventData>, ITransientDependency
    {
        public void HandleEvent(AddMaterialEventData eventData)
        {

            MachiningGroupClientServiceProxy machiningGroupClientServiceProxy = new MachiningGroupClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            string errorMessage = string.Empty;
            try
            {
                var rs = machiningGroupClientServiceProxy.GetAllAsync(eventData.MaterialId, 0, 50).Result;
                errorMessage = rs.Error?.Details;
                if (rs.Success && !rs.Result.Items.Any(n => n.MaterialThickness == eventData.MaterialThickness))
                {
                    var addRs = machiningGroupClientServiceProxy.CreateAsync(new CreateMachiningGroupDto
                    {
                        Code = rs.Result.TotalCount,
                        MaterialId = eventData.MaterialId,
                        Description = "",
                        MaterialThickness = eventData.MaterialThickness
                    }).Result;
                    errorMessage = rs.Error?.Details;
                    if (addRs.Success)
                    {
                        Messenger.Default.Send(new MainSystemNoticeModel
                        {
                            Tagret = eventData.Tagret,
                            Error = "",
                            Success = true,
                            SuccessAction = eventData.SuccessAction,
                            HashCode = eventData.HashCode
                        });
                    }
                }

                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = errorMessage,
                    ErrorAction = eventData.ErrorAction,
                    HashCode = eventData.HashCode
                });
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = ex.Message,
                    ErrorAction = eventData.ErrorAction,
                    HashCode = eventData.HashCode

                });
            }
        }
    }

    public class MaterialTypeHandler : IEventHandler<MachiningGroupInfoEventData>, ITransientDependency
    {
        public void HandleEvent(MachiningGroupInfoEventData eventData)
        {

            MachiningGroupClientServiceProxy machiningGroupClientServiceProxy = new MachiningGroupClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            string errorMessage = string.Empty;
            try
            {
                var rs = machiningGroupClientServiceProxy.GetAllAsync(eventData.MaterialId, 0, 50).Result;
                errorMessage = rs.Error?.Details;
                if (rs.Success)
                {
                    Messenger.Default.Send(rs.Result);
                }
                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = errorMessage,
                    ErrorAction = eventData.ErrorAction,
                    HashCode = eventData.HashCode
                });
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = ex.Message,
                    ErrorAction = eventData.ErrorAction,
                    HashCode = eventData.HashCode

                });
            }
        }
    }
}
