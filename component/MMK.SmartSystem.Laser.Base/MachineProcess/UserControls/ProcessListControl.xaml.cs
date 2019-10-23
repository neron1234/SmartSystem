using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Laser.Base.MachineProcess.UserControls.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MMK.SmartSystem.Laser.Base.MachineProcess.UserControls
{
    /// <summary>
    /// InciseProcessControl.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessListControl : UserControl
    {
        public ProcessListViewModel pcListVewModel { get; set; }
        public ProcessListControl()
        {
            InitializeComponent();
            this.DataContext = pcListVewModel = new ProcessListViewModel();
            this.TotolWidth = this.ProcessDataGrid.Width;
            Messenger.Default.Register<PagedResultDtoOfCuttingDataDto>(this, (result) =>{
                pcListVewModel.PageListData = new ObservableCollection<object>(result.Items.ToList());
                LoadColum();
            });
            Messenger.Default.Register<PagedResultDtoOfEdgeCuttingDataDto>(this, (result) =>
            {
                pcListVewModel.PageListData = new ObservableCollection<object>(result.Items.ToList());
                LoadColum();
            });
            Messenger.Default.Register<PagedResultDtoOfPiercingDataDto>(this, (result) =>
            {
                pcListVewModel.PageListData = new ObservableCollection<object>(result.Items.ToList());
                LoadColum();
            });
            Messenger.Default.Register<PagedResultDtoOfSlopeControlDataDto>(this, (result) =>
            {
                pcListVewModel.PageListData = new ObservableCollection<object>(result.Items.ToList());
                LoadColum();
            });
            Messenger.Default.Register<string>(this, (str) => {
                if (str == "NextColumns")
                {
                    SetDtaColumHeader(true);
                }
                else if(str == "LastColumns")
                {
                    SetDtaColumHeader(false);
                }
            });
        }

        private List<DataColumn> DataColumns { get; set; }

        /// <summary>
        /// DataGrid宽度
        /// </summary>
        private double TotolWidth = 1290;
        private int ColumWidth = 100;
        private int PageNumber = 0;
        private int TotalPage = 0;
        private int CurrentPage = 0;
        private void SetDtaColumHeader(bool next)
        {              
            int count = DataColumns.Count;
            int pageSize = 0;
            
            if (count % PageNumber == 0){
                pageSize = count / PageNumber;
            }else{
                pageSize = count / PageNumber + 1;
            }
            TotalPage = pageSize;
            if (next && CurrentPage >= 1 && CurrentPage < TotalPage){
                CurrentPage++;
            }else{
                CurrentPage = 1;
            }
            this.ProcessDataGrid.Dispatcher.Invoke(new Action(() => {
                var list = DataColumns.Take(PageNumber * CurrentPage).Skip(PageNumber * (CurrentPage - 1)).ToList();
                ProcessDataGrid.Columns.Clear();
                foreach (var item in list)
                {
                    ProcessDataGrid.Columns.Add(new DataGridTextColumn()
                    {
                        Header = item.Header,
                        Binding = new Binding(item.BindingName),
                        Width = item.Width
                    }) ;
                }
            }));
        }

        private void LoadColum()
        {
            if (pcListVewModel.PageListData.Count == 0){
                return;
            }
            DataColumns = new List<DataColumn>();
            var properties = pcListVewModel.PageListData.First().GetType().GetProperties();
            foreach (var item in properties)
            {
                if (pcListVewModel.ColumnArray.ContainsKey(item.Name))
                {
                    var headerName = pcListVewModel.ColumnArray[item.Name];
                    if (headerName.Length < 4)
                    {
                        ColumWidth = 80;
                    }
                    DataColumns.Add(new DataColumn()
                    {
                        Header = headerName,
                        BindingName = item.Name,
                        Width = ColumWidth
                    });
                }
            }
            PageNumber = 0;
            var nowTotolWidth = TotolWidth;
            foreach (var item in DataColumns)
            {
                nowTotolWidth -= item.Width;
                if (nowTotolWidth > item.Width)
                {
                    PageNumber++;
                }
            }
            SetDtaColumHeader(false);
        }

        private void ProcessDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
