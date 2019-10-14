using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineProcess.UserControls.ViewModel
{
    public class ProcessListViewModel:ViewModelBase
    {
        private ObservableCollection<object> _PageListData;
        public ObservableCollection<object> PageListData
        {
            get { return _PageListData; }
            set
            {
                if (_PageListData != value)
                {
                    _PageListData = value;
                    RaisePropertyChanged(() => PageListData);
                }
            }
        }

        public ProcessListViewModel()
        {
            Messenger.Default.Register<PagedResultDtoOfCuttingDataDto>(this, (result) =>
            {
                this.PageListData = new ObservableCollection<object>();
                foreach (var item in result.Items)
                {
                    this.PageListData.Add(item);
                }
            });
            Messenger.Default.Register<PagedResultDtoOfEdgeCuttingDataDto>(this, (result) =>
            {
                this.PageListData = new ObservableCollection<object>();
                foreach (var item in result.Items)
                {
                    this.PageListData.Add(item);
                }
            });
            Messenger.Default.Register<PagedResultDtoOfPiercingDataDto>(this, (result) =>
            {
                this.PageListData = new ObservableCollection<object>();
                foreach (var item in result.Items)
                {
                    this.PageListData.Add(item);
                }
            });
            Messenger.Default.Register<PagedResultDtoOfSlopeControlDataDto>(this, (result) =>
            {
                this.PageListData = new ObservableCollection<object>();
                foreach (var item in result.Items)
                {
                    this.PageListData.Add(item);
                }
            });
        }
    }
}
