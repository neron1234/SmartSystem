using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.Model;
using MMK.SmartSystem.LE.Host.SystemControl.ViewModel;
using MMK.SmartSystem.WebCommon.DeviceModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections.Concurrent;
namespace MMK.SmartSystem.LE.Host.SystemControl
{
    /// <summary>
    /// HeaderTitleWarnControl.xaml 的交互逻辑
    /// </summary>
    public partial class HeaderTitleWarnControl : UserControl
    {
        private HeaderTitleMenuViewModel headerVM;
        BlockingCollection<ReadAlarmResultItemModel> alarmResultItemModels = new BlockingCollection<ReadAlarmResultItemModel>();
        private int currentIndex = 0;
        private DispatcherTimer TimeTimer;

        public HeaderTitleWarnControl()
        {
            InitializeComponent();
            this.DataContext = headerVM = new HeaderTitleMenuViewModel();
            Loaded += HeaderTitleWarnControl_Loaded;
        }

        private void HeaderTitleWarnControl_Loaded(object sender, RoutedEventArgs e)
        {
            TimeTimer = new DispatcherTimer();
            TimeTimer.Tick += TimeTimer_Tick;
            TimeTimer.Interval = new TimeSpan(0, 0, 0, 1);
            TimeTimer.Start();
        }

        private void TimeTimer_Tick(object sender, EventArgs e)
        {
            if (currentIndex >= alarmResultItemModels.Count)
            {
                currentIndex = 0;

            }
            var obj = alarmResultItemModels.ToList()[currentIndex++];
            headerVM.WarnStr =$"{obj.Ttype}{obj.Num} {obj.Message} {obj.AxisStr}" ;

        }

        public void UpdateTitle(string title)
        {

            if (headerVM != null)
            {
                headerVM.Title = title;
            }
        }
        public void SetProgram(string name)
        {
            headerVM.ProgramName = name;
        }

        public void SetAlarm(List<ReadAlarmResultModel> readAlarms)
        {

            var list = readAlarms.Where(d => d.Id == "mainPage_ReadAlarm").FirstOrDefault()?.Value ?? new List<ReadAlarmResultItemModel>();
            var sourseList = alarmResultItemModels.ToList();
            if (list.Except(sourseList).Count() > 0 || sourseList.Except(list).Count() > 0)
            {
                while (alarmResultItemModels.Count > 0)
                {
                    alarmResultItemModels.Take();
                }
                list.ForEach(d => alarmResultItemModels.Add(d));

            }

        }
        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Messenger.Default.Send(new WaringMsgPopup { Visibility = Visibility.Visible });
        }
    }
}
