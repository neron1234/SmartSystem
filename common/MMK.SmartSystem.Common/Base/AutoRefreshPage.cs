using Abp.Dependency;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MMK.SmartSystem.Common.Base
{

    public class AutoRefreshPage : Page, ITransientDependency
    {
        public event Action RefreshAuth;
        public AutoRefreshPage()
        {
            Messenger.Default.Register<MainSystemNoticeModel>(this, loadModel);
        }
        private void loadModel(MainSystemNoticeModel model)
        {
            if (model.EventType == EventEnum.RefreshAuth)
            {
                RefreshAuth?.Invoke();
                return;
            }
            if (model.EventType == EventEnum.NavHome)
            {

            }
        }
        public void ClearRegister()
        {
            Messenger.Default.Unregister<MainSystemNoticeModel>(this, loadModel);
        }
    }
}
