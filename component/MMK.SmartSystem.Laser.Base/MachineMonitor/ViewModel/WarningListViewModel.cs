﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineMonitor.ViewModel
{
    public class WarningListViewModel:ViewModelBase
    {
        private ObservableCollection<WarningInfo> _WarningList;
        public ObservableCollection<WarningInfo> WarningList
        {
            get { return _WarningList; }
            set {
                if (_WarningList != value){
                    _WarningList = value;
                    RaisePropertyChanged(() => WarningList);
                }
            }
        }
        public WarningListViewModel()
        {
            WarningList = new ObservableCollection<WarningInfo>() {
                new WarningInfo {
                    Id = "15",
                    Number = "1001",
                    Message = "THIS IS A ALARM NO 1 CURRENT"
                },new WarningInfo {
                    Id = "15",
                    Number = "1002",
                    Message = "THIS IS A ALARM NO 2 CURRENT"
                },new WarningInfo {
                    Id = "15",
                    Number = "1003",
                    Message = "THIS IS A ALARM NO 3 CURRENT"
                },new WarningInfo {
                    Id = "16",
                    Number = "1004",
                    Message = "THIS IS A ALARM NO 4 CURRENT"
                },new WarningInfo {
                    Id = "16",
                    Number = "1005",
                    Message = "THIS IS A ALARM NO 5 CURRENT"
                }
                ,new WarningInfo {
                    Id = "17",
                    Number = "1006",
                    Message = "THIS IS A ALARM NO 6 CURRENT"
                }
                ,new WarningInfo {
                    Id = "17",
                    Number = "1007",
                    Message = "THIS IS A ALARM NO 7 CURRENT"
                }
                ,new WarningInfo {
                    Id = "18",
                    Number = "1008",
                    Message = "THIS IS A ALARM NO 8 CURRENT"
                }
                ,new WarningInfo {
                    Id = "18",
                    Number = "1009",
                    Message = "THIS IS A ALARM NO 9 CURRENT"
                },new WarningInfo {
                    Id = "19",
                    Number = "1010",
                    Message = "THIS IS A ALARM NO 10 CURRENT"
                }
            };
        }
    }

    public class WarningInfo
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string Message { get; set; }
    }
}