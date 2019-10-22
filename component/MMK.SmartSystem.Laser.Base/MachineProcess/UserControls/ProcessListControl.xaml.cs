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
            Messenger.Default.Register<PagedResultDtoOfCuttingDataDto>(this, (result) =>{
                pcListVewModel.PageListData = new ObservableCollection<object>();
                foreach (var item in result.Items){
                    pcListVewModel.PageListData.Add(item);
                }
                if (result.Items.Count > 0){
                    var types = result.Items.First().GetType().GetProperties();
                    this.ProcessDataGrid.Dispatcher.Invoke(new Action(() => {
                        foreach (var item in types){
                            if (pcListVewModel.ColumnArray.ContainsKey(item.Name)){
                                ProcessDataGrid.Columns.Add(new DataGridTextColumn(){
                                    Header = pcListVewModel.ColumnArray[item.Name],
                                    Binding = new Binding(item.Name),
                                    Width = 80
                                });
                            }
                        }
                    }));
                }
            });
            Messenger.Default.Register<PagedResultDtoOfEdgeCuttingDataDto>(this, (result) =>
            {
                pcListVewModel.PageListData = new ObservableCollection<object>();
                foreach (var item in result.Items)
                {
                    pcListVewModel.PageListData.Add(item);
                }
            });
            Messenger.Default.Register<PagedResultDtoOfPiercingDataDto>(this, (result) =>
            {
                pcListVewModel.PageListData = new ObservableCollection<object>();
                foreach (var item in result.Items)
                {
                    pcListVewModel.PageListData.Add(item);
                }
            });
            Messenger.Default.Register<PagedResultDtoOfSlopeControlDataDto>(this, (result) =>
            {
                pcListVewModel.PageListData = new ObservableCollection<object>();
                foreach (var item in result.Items)
                {
                    pcListVewModel.PageListData.Add(item);
                }
            });
        }

        private void ProcessDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
