﻿using Abp.Events.Bus;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.Common.SerivceProxy;
using MMK.SmartSystem.LE.Host.AccountControl.ViewModel;
using MMK.SmartSystem.LE.Host.SystemControl;
using MMK.SmartSystem.LE.Host.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MMK.SmartSystem.LE.Host.AccountControl
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public LoginControlViewModel LoginModel { get; set; }

        public string Error = string.Empty;
        public LoginControl()
        {
            InitializeComponent();
            LoginModel = new LoginControlViewModel()
            {
                Account = "",
                Pwd = "",
                IsError = false,
                IsLogin = false
            };
            this.DataContext = LoginModel;

            //Messenger.Default.Register<UserInfo>(this, (u) =>
            // {
            //     this.Dispatcher.InvokeAsync(() =>
            //     {
            //         Close();
            //     });
            // });
            Messenger.Default.Register<MainSystemNoticeModel>(this, (ms) => {
                if (ms.HashCode == this.GetHashCode()){
                    if (ms.Success) {
                        ms.SuccessAction?.Invoke();
                    } else{
                        Error = ms.Error;
                        ms.ErrorAction?.Invoke();
                    }
                }
            });
            Loaded += UserControl_Loaded;
          
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.maskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, true);
            Loaded -= UserControl_Loaded;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }

        private void Login()
        {
            var msg = string.Empty;

            EventBus.Default.TriggerAsync(new UserLoginEventData()
            {
                UserName = LoginModel.Account,
                Pwd = LoginModel.Pwd,
                HashCode = this.GetHashCode(),
                SuccessAction = SuccessAction,
                Tagret = ErrorTagretEnum.UserControl,
                ErrorAction = ErrorAction
            });
        }

        private void ErrorAction()
        {
            MessageBox.Show(Error);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.maskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, false);
        }
        private void SuccessAction()
        {
            EventBus.Default.Trigger(new UserInfoEventData() { UserId = (int)SmartSystemCommonConsts.AuthenticateModel.UserId, Tagret = ErrorTagretEnum.UserControl });
            this.maskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, false);
        }
    }
}
