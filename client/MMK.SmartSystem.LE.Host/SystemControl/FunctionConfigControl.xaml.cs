using MMK.SmartSystem.LE.Host.SystemControl.ViewModel;
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

namespace MMK.SmartSystem.LE.Host.SystemControl
{
    /// <summary>
    /// FunctionConfigControl.xaml 的交互逻辑
    /// </summary>
    public partial class FunctionConfigControl : UserControl
    {
        public FunctionConfigViewModel FunctionConfigViewModel { get; set; }

        public FunctionConfigControl()
        {
            InitializeComponent();
            this.DataContext = FunctionConfigViewModel = new FunctionConfigViewModel();
        }

        private string SysSelectedName = string.Empty;
        private void SysZGroupBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SysSelectedName = ((ZGroupBox)sender).Header.ToString();

            List<ZGroupBox> boxList = GetChildObjects<ZGroupBox>(SysItemControl, "");
            foreach (var item in boxList)
            {
                item.BorderBrush = Brushes.Transparent;
                item.BorderThickness = new Thickness();
            }
          ((ZGroupBox)sender).BorderBrush = Brushes.Red;
            ((ZGroupBox)sender).BorderThickness = new Thickness(2);
        }

        private string CostomSelectedName = string.Empty;
        private void CostomZGroupBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CostomSelectedName = ((ZGroupBox)sender).Header.ToString();

            List<ZGroupBox> boxList = GetChildObjects<ZGroupBox>(CustomItemControl, "");
            foreach (var item in boxList)
            {
                item.BorderBrush = Brushes.Transparent;
                item.BorderThickness = new Thickness();
            }
            ((ZGroupBox)sender).BorderBrush = Brushes.Yellow;
            ((ZGroupBox)sender).BorderThickness = new Thickness(2);
        }

        public List<T> GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Name == name || string.IsNullOrEmpty(name)))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, ""));
            }
            return childList;
        }

        private void UpMoveBtn_Click(object sender, RoutedEventArgs e){
            var moveSysModel = FunctionConfigViewModel.SysModuleViews.FirstOrDefault(n => n.ModuleName == SysSelectedName);
            if (moveSysModel == null){
                return;
            }
            MoveSysModule(moveSysModel, moveSysModel.Sort - 1);
        }

        private void DownMoveBtn_Click(object sender, RoutedEventArgs e)
        {
            var moveSysModel = FunctionConfigViewModel.SysModuleViews.FirstOrDefault(n => n.ModuleName == SysSelectedName);
            if (moveSysModel == null){
                return;
            }
            MoveSysModule(moveSysModel, moveSysModel.Sort + 1);
        }

        private void TopMoveBtn_Click(object sender, RoutedEventArgs e)
        {
            var moveSysModel = FunctionConfigViewModel.SysModuleViews.FirstOrDefault(n => n.ModuleName == SysSelectedName);
            if (moveSysModel != null)
            {
                moveSysModel.Sort = 0;
                var list = FunctionConfigViewModel.SysModuleViews.Where(n => n.ModuleName != SysSelectedName).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Sort = i + 1;
                }
            }
            FunctionConfigViewModel.SysModuleViews = new ObservableCollection<SystemMenuModuleViewModel>(FunctionConfigViewModel.SysModuleViews.OrderBy(n => n.Sort));
        }

        private void MoveSysModule(SystemMenuModuleViewModel moveSysModel,int sort)
        {
            var topSysModel = FunctionConfigViewModel.SysModuleViews.FirstOrDefault(n => n.Sort == sort);
            if (topSysModel != null)
            {
                topSysModel.Sort = moveSysModel.Sort;
                moveSysModel.Sort = sort;
            }
            FunctionConfigViewModel.SysModuleViews = new ObservableCollection<SystemMenuModuleViewModel>(FunctionConfigViewModel.SysModuleViews.OrderBy(n => n.Sort));
        }

        private void Button_Drop(object sender, DragEventArgs e)
        {
            
        }

        private void AddCustemGroup_Click(object sender, RoutedEventArgs e)
        {
            FunctionConfigViewModel.CustomModuleViews.Add(new SystemMenuModuleViewModel
            {
                BackColor = "#f26a80",
                ModuleName = "自定义分组0" + FunctionConfigViewModel.CustomModuleViews.Count(),
                MainMenuViews = new ObservableCollection<MainMenuViewModel>(),
                Show = Visibility.Visible,
                Sort = 1
            });
        }

        private void AddFunctionBtn_Click(object sender, RoutedEventArgs e)
        {
            var name = ((Button)sender).Tag.ToString();
            var customModule = FunctionConfigViewModel.CustomModuleViews.FirstOrDefault(n => n.ModuleName == name);
            if (customModule == null)
            {
                return;
            }
            foreach (var item in selectedFunctions)
            {
                customModule.MainMenuViews.Add(item);
                FunctionConfigViewModel.SysModuleViews.FirstOrDefault(n => n.MainMenuViews.Any(m => m.Id == item.Id))?.MainMenuViews.Remove(item);
            }
            for (int i = FunctionConfigViewModel.SysModuleViews.Count -1; i >=0 ; i--)
            {
                if(!FunctionConfigViewModel.SysModuleViews[i].MainMenuViews.Any(n => n.Show == Visibility.Visible))
                {
                    FunctionConfigViewModel.SysModuleViews.RemoveAt(i);
                }
            }
            //FunctionConfigViewModel.SysModuleViews.Remove(FunctionConfigViewModel.SysModuleViews.)
            selectedFunctions.Clear();
        }

        private List<MainMenuViewModel> selectedFunctions = new List<MainMenuViewModel>();
        private void SelectFunctionBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var id = ((Tag)sender).Tag.ToString();

            var selectedFunction = selectedFunctions.FirstOrDefault(n => n.Id == id);
            if (selectedFunction != null)
            {
                ((Tag)sender).BorderBrush = (Brush)new BrushConverter().ConvertFromString(selectedFunction.BackColor);
                ((Tag)sender).BorderThickness = new Thickness(1);
                selectedFunctions.Remove(selectedFunction);
                return;
            }
            var function = FunctionConfigViewModel.SysModuleViews.FirstOrDefault(n => n.MainMenuViews.Any(m => m.Id == id)).MainMenuViews.FirstOrDefault(n => n.Id == id);
            //List<Tag> boxList = GetChildObjects<Tag>(SysItemControl, "");
            if (function != null)
            {
                selectedFunctions.Add(function);
            }
            ((Tag)sender).BorderBrush = Brushes.Red;
            ((Tag)sender).BorderThickness = new Thickness(2);
        }


        /// <summary>
        /// 克隆分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloneGroupBtn_Click(object sender, RoutedEventArgs e)
        {
            var name = ((Button)sender).Tag.ToString();
            var moveSysModel = FunctionConfigViewModel.SysModuleViews.FirstOrDefault(n => n.ModuleName == name);
            if (moveSysModel == null)
            {
                return;
            }
            FunctionConfigViewModel.CustomModuleViews.Add(moveSysModel);
            FunctionConfigViewModel.SysModuleViews.Remove(moveSysModel);
        }

        private void RemoveGroupBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
