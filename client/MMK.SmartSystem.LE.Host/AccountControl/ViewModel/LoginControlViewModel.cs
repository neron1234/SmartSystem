﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.SerivceProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.LE.Host.AccountControl.ViewModel
{
    public class LoginControlViewModel:ViewModelBase
    {
        private bool isLogin;
        public bool IsLogin
        {
            get { return isLogin; }
            set
            {
                isLogin = value;

                RaisePropertyChanged(() => IsLogin);
            }
        }

        private bool isError;
        public bool IsError
        {
            get { return isError; }
            set
            {
                isError = value;
                IsLogin = !isError;
                if (!isError){
                    AccountError = "";
                    PwdError = "";
                }
                RaisePropertyChanged(() => IsError);
            }
        }

        private string accountError;
        public string AccountError
        {
            get { return accountError; }
            set
            {
                accountError = value;
                RaisePropertyChanged(() => AccountError);
            }
        }

        private string pwdError;
        public string PwdError
        {
            get { return pwdError; }
            set
            {
                pwdError = value;
                RaisePropertyChanged(() => PwdError);
            }
        }

        private string _Account;
        public string Account
        {
            get { return _Account; }
            set
            {
                if (_Account != value)
                {
                    _Account = value;
                    if (string.IsNullOrEmpty(_Account)){
                        AccountError = "账号不能为空";
                        IsError = true;
                    }
                    else
                    {
                        AccountError = "";
                    }
                    RaisePropertyChanged(() => Account);
                }
            }
        }

        private string _Pwd;
        public string Pwd
        {
            get { return _Pwd; }
            set
            {
                if (_Pwd != value)
                {
                    _Pwd = value;
                    if (string.IsNullOrEmpty(_Pwd)){
                        PwdError = "账号不能为空";
                        IsError = true;
                    }
                    else
                    {
                        PwdError = "";
                    }
                    RaisePropertyChanged(() => Pwd);
                }
            }
        }

        public ICommand AccountChangedCommand{
            get{
                return new RelayCommand<string>((str) => {
                   Account = str;
                   if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(Pwd)){
                        IsError = false;
                    }
                    else
                    {
                        IsError = true;
                    }
                });
            }
        }

        public ICommand PwdChangedCommand{
            get{
                return new RelayCommand<string>((str) => {
                    Pwd = str;
                    if (!string.IsNullOrEmpty(Account) && !string.IsNullOrEmpty(str))
                    {
                        IsError = false;
                    }
                    else
                    {
                        IsError = true;
                    }
                });
            }
        }

        public ICommand LoginCommand{
            get{
                return new RelayCommand<string>((s) => {
                    try { 
                        TokenAuthClient tokenAuthClient = new TokenAuthClient(MMK.SmartSystem.Common.SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
                        var ts = tokenAuthClient.AuthenticateAsync(new MMK.SmartSystem.Common.AuthenticateModel() { UserNameOrEmailAddress = Account, Password = Pwd }).Result;
                        if (ts.Success){
                            Common.SmartSystemCommonConsts.AuthenticateModel = ts.Result;
                            var obj2 = tokenAuthClient.GetUserConfiguraionAsync().Result;
                            if (obj2.Success)
                            {
                                Common.SmartSystemCommonConsts.UserConfiguration = obj2.Result;
                            }
                            s = "登陆成功";
                        }else{
                            s = ts.Error.Details;
                        }
                    }catch(Exception ex){
                        s = ex.ToString();
                    }
                    Messenger.Default.Send(s);

                    //Messenger.Default.Send(Common.SmartSystemCommonConsts.UserConfiguration);

                    Messenger.Default.Send(Common.SmartSystemCommonConsts.AuthenticateModel);
                });
            }
        }
    }
}
