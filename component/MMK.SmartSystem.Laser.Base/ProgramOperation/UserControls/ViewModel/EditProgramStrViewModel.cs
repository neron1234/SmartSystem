using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Laser.Base.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class EditProgramStrViewModel : ViewModelBase
    {
        public EditProgramStrViewModel()
        {
            pagingModel = new PagingModel<ProgramStr>();
            ProgramStrList = new List<ProgramStr>();
            pagingModel.PagePagingEvent += PagingModel_PagePagingEvent; ;
        }

        private void PagingModel_PagePagingEvent(IEnumerable<ProgramStr> arg1, int arg2, int arg3)
        {
            this.ProgramStrList.Clear();
            arg1.ToList().ForEach(d => ProgramStrList.Add(d));
            this.ProgramStr = string.Join("", ProgramStrList.Select(n => n.Str));
            CurrentPage = arg2;
            TotalPage = arg3;
            PagePagingEvent?.Invoke();
        }

        private PagingModel<ProgramStr> pagingModel;

        public event Action PagePagingEvent;

        public async void LoadProgramStr()
        {
            await Task.Factory.StartNew(new Action(() =>
            {
                try
                {
                    var strList = System.IO.File.ReadAllLines(this.Url).ToList();
                    strList.ForEach(d => ProgramStrList.Add(new ProgramStr() { Str = d }));
                    this.DataPaging();
                    //this.ProgramStr = string.Join("  ", str.Take(100).ToArray());
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send(new Common.ViewModel.NotifiactionModel()
                    {
                        Title = "操作失败",
                        Content = $"失败信息：{ex} {DateTime.Now}",
                        NotifiactionType = Common.ViewModel.EnumPromptType.Error
                    });
                }
            }));
        }

        public void DataPaging()
        {
            pagingModel.Init<object>(ProgramStrList, null, 1, PageNumber);
        }

        private int _CurrentPage;
        public int CurrentPage
        {
            get { return _CurrentPage; }
            set
            {
                if (_CurrentPage != value)
                {
                    _CurrentPage = value;
                    RaisePropertyChanged(() => CurrentPage);
                }
            }
        }

        private int _TotalPage;
        public int TotalPage
        {
            get { return _TotalPage; }
            set
            {
                if (_TotalPage != value)
                {
                    _TotalPage = value;
                    RaisePropertyChanged(() => TotalPage);
                }
            }
        }

        public int PageNumber = 20;

        public List<ProgramStr> ProgramStrList { get; set; }

        private string _ProgramStr;
        public string ProgramStr
        {
            get { return _ProgramStr; }
            set
            {
                if (_ProgramStr != value)
                {
                    _ProgramStr = value;
                    RaisePropertyChanged(() => ProgramStr);
                }
            }
        }
        public string Url { get; set; }

        public event Action CloseEvent;
        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand<string>((str) =>
                {
                    CloseEvent?.Invoke();
                });
            }
        }

        public ICommand LastColumnsCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    pagingModel.LastPage();
                });
            }
        }

        public ICommand NestColumnsCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    pagingModel.NextPage();
                });
            }
        }
    }

    public class ProgramStr : ViewModelBase
    {
        public string Str { get; set; }
    }
}
