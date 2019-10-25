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
                var rs = materialClientServiceProxy.GetMaterialAllAsync(eventData.IsCheckSon, 0, 50).Result;
                errorMessage = rs.Error?.Details;
                if (rs.Success)
                {
                    eventData.SuccessAction?.Invoke(rs.Result.Items.ToList());
                    return;
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

    public class AddMaterialHandler : IEventHandler<AddMachiningInfoEventData>, ITransientDependency
    {
        public void HandleEvent(AddMachiningInfoEventData eventData)
        {
            MaterialClientServiceProxy materialClientServiceProxy = new MaterialClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            string errorMessage = string.Empty;
            try
            {
                var rs = materialClientServiceProxy.GetAllAsync(false, 0, 50).Result;
                errorMessage = rs.Error?.Details;
                if (rs.Success)
                {
                    eventData.CreateMaterial.Code = rs.Result.Items.LastOrDefault() == null ? 1 : rs.Result.Items.LastOrDefault()?.Code + 1;
                    var addRs = materialClientServiceProxy.CreateAsync(eventData.CreateMaterial).Result;
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
                else
                {
                    errorMessage = "保存失败";
                    Messenger.Default.Send(new MainSystemNoticeModel
                    {
                        Tagret = eventData.Tagret,
                        Error = errorMessage,
                        ErrorAction = eventData.ErrorAction,
                        HashCode = eventData.HashCode
                    });
                }
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

    public class AddMaterialGroupHandler : IEventHandler<AddMachiningGroupInfoEventData>, ITransientDependency
    {
        public void HandleEvent(AddMachiningGroupInfoEventData eventData)
        {
            MachiningGroupClientServiceProxy machiningGroupClientServiceProxy = new MachiningGroupClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            string errorMessage = string.Empty;
            try
            {
                var rs = machiningGroupClientServiceProxy.GetAllAsync(eventData.CreateMachiningGroup.MaterialCode, 0, 50).Result;
                errorMessage = rs.Error?.Details;
                if (rs.Success && !rs.Result.Items.Any(n => n.MaterialThickness == eventData.CreateMachiningGroup.MaterialThickness))
                {
                    eventData.CreateMachiningGroup.Code = rs.Result.Items.LastOrDefault() == null ? 1 : rs.Result.Items.LastOrDefault()?.Code + 1;
                    var addRs = machiningGroupClientServiceProxy.CreateAsync(eventData.CreateMachiningGroup).Result;
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
                else {
                    errorMessage = "保存失败"; 
                    Messenger.Default.Send(new MainSystemNoticeModel
                    {
                        Tagret = eventData.Tagret,
                        Error = errorMessage,
                        ErrorAction = eventData.ErrorAction,
                        HashCode = eventData.HashCode
                    });
                }
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

    public class MaterialGroupHandler : IEventHandler<MachiningGroupInfoEventData>, ITransientDependency
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

    public class DeleteMaterialInfo : IEventHandler<DeleteMachiningInfoEventData>, ITransientDependency
    {
        public void HandleEvent(DeleteMachiningInfoEventData eventData)
        {
            MaterialClientServiceProxy materialClientServiceProxy = new MaterialClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            string errorMessage = string.Empty;
            try
            {
                
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = ex.ToString(),
                    ErrorAction = eventData.ErrorAction,
                    HashCode = eventData.HashCode

                });
            }
        }
    }

    public class DeleteMaterialGroup : IEventHandler<DeleteMachiningGroupInfoEventData>, ITransientDependency
    {
        public void HandleEvent(DeleteMachiningGroupInfoEventData eventData)
        {
            MachiningGroupClientServiceProxy machiningGroupClientServiceProxy = new MachiningGroupClientServiceProxy(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            string errorMessage = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new MainSystemNoticeModel
                {
                    Tagret = eventData.Tagret,
                    Error = ex.ToString(),
                    ErrorAction = eventData.ErrorAction,
                    HashCode = eventData.HashCode
                });
            }
        }
    }
}
