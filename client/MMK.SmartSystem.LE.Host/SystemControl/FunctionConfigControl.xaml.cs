using GalaSoft.MvvmLight.Messaging;
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
            Messenger.Default.Register<string>(this, (s) => {
                //保存配置
                if (FunctionConfigViewModel.CustomModuleViews.Count(n => n.Show == Visibility.Visible) > 0)
                {
                    Messenger.Default.Send(FunctionConfigViewModel.CustomModuleViews);
                }
                else
                {
                    Messenger.Default.Send(FunctionConfigViewModel.SysModuleViews);
                }
                SmartSystemLEConsts.CustemModules = FunctionConfigViewModel.CustomModuleViews.CloneJson();
                SmartSystemLEConsts.SystemModules = FunctionConfigViewModel.SysModuleViews.CloneJson();
            });
            this.DataContext = FunctionConfigViewModel = new FunctionConfigViewModel();
        }

        private void UnSelectedControl<T>(ItemsControl itemsControl, Thickness ts, Brush brush,string tag = "") where T : ContentControl
        {
            List<T> btnList = GetChildObjects<T>(itemsControl, tag);
            foreach (var item in btnList)
            {
                ((T)(item)).BorderBrush = brush;
                ((T)(item)).BorderThickness = ts;
            }
        }

        public List<T> GetChildObjects<T>(DependencyObject obj, string tag) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Tag != null))
                {
                    if (((T)child).Tag.ToString() == tag || string.IsNullOrEmpty(tag))
                    {
                        childList.Add((T)child);
                    }
                }
                childList.AddRange(GetChildObjects<T>(child, tag));
            }
            return childList;
        }

        private void UnSelectedZGroupBox(ObservableCollection<SystemMenuModuleViewModel> systemMenus,string sysMenusName = "",bool ClearMenu = false)
        {
            foreach (var module in systemMenus)
            {
                if (!string.IsNullOrEmpty(sysMenusName) && module.ModuleName != sysMenusName)
                {
                    continue;
                }
                UnSelectedControl<ZGroupBox>(SysItemControl, new Thickness(1), (Brush)new BrushConverter().ConvertFromString(module.BackColor), module.ModuleName);
                if (!ClearMenu)
                {
                    continue;
                }
                foreach (var item in module.MainMenuViews)
                {
                    UnSelectedControl<Tag>(SysItemControl, new Thickness(1), (Brush)new BrushConverter().ConvertFromString(item.BackColor), item.Id);
                    selectedFunctions.Remove(item);
                }
            }
        }

        private string SysSelectedName = string.Empty;
        private bool SysGroupClick = true;
        private void SysZGroupBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!SysGroupClick)
            {
                SysGroupClick = true;
                return;
            }
            SysSelectedName = ((ZGroupBox)sender).Header.ToString();
            UnSelectedZGroupBox(FunctionConfigViewModel.SysModuleViews);
            ((ZGroupBox)sender).BorderBrush = Brushes.Red;
            ((ZGroupBox)sender).BorderThickness = new Thickness(2);
        }

        private string CostomSelectedName = string.Empty;
        private void CostomZGroupBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CostomSelectedName = ((ZGroupBox)sender).Header.ToString();
            Brush brush = Brushes.Transparent;
            UnSelectedZGroupBox(FunctionConfigViewModel.CustomModuleViews);
            ((ZGroupBox)sender).BorderBrush = Brushes.Yellow;
            ((ZGroupBox)sender).BorderThickness = new Thickness(2);
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

        /// <summary>
        /// 添加自定义分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 添加功能项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                customModule.MainMenuViews.Add(item.CloneJson());
                UnSelectedControl<Tag>(SysItemControl, new Thickness(1), (Brush)new BrushConverter().ConvertFromString(item.BackColor),item.Id);
                item.Show = Visibility.Collapsed;
            }
            FunctionConfigViewModel.CheckMenuSetShowCommand.Execute(null);
            selectedFunctions.Clear();
        }

        private List<MainMenuViewModel> selectedFunctions = new List<MainMenuViewModel>();
        /// <summary>
        /// 选择功能项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFunctionBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SysGroupClick = false;
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
            if (function != null)
            {
                selectedFunctions.Add(function);
            }
            ((Tag)sender).BorderBrush = Brushes.Red;
            ((Tag)sender).BorderThickness = new Thickness(2);
        }

        /// <summary>
        /// 删除功能项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FunctionBtn_Closed(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var id = ((Tag)sender).Tag.ToString();
            var customFunction = FunctionConfigViewModel.CustomModuleViews.FirstOrDefault(n => n.MainMenuViews.Any(m => m.Id == id)).MainMenuViews.FirstOrDefault(n => n.Id == id);
            if (customFunction != null)
            {
                FunctionConfigViewModel.CustomModuleViews.FirstOrDefault(n => n.MainMenuViews.Any(m => m.Id == id)).MainMenuViews.Remove(customFunction);
            }
            var sysFunction = FunctionConfigViewModel.SysModuleViews.FirstOrDefault(n => n.MainMenuViews.Any(m => m.Id == id)).MainMenuViews.FirstOrDefault(n => n.Id == id);
            if (sysFunction != null)
            {
                sysFunction.Show = Visibility.Visible;
            }
            FunctionConfigViewModel.CheckMenuSetShowCommand.Execute(null);
        }

        /// <summary>
        /// 克隆分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloneGroupBtn_Click(object sender, RoutedEventArgs e)
        {
            var name = ((Button)sender).Tag.ToString();
            var sysModel = FunctionConfigViewModel.SysModuleViews.FirstOrDefault(n => n.ModuleName == name);
            if (sysModel == null)
            {
                return;
            }
            FunctionConfigViewModel.CustomModuleViews.Add(sysModel.CloneJson());
            sysModel.Show = Visibility.Collapsed;
            FunctionConfigViewModel.SysMenuSetHiddenCommand.Execute(null);
            UnSelectedZGroupBox(FunctionConfigViewModel.SysModuleViews, name, true);
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveGroupBtn_Click(object sender, RoutedEventArgs e)
        {
            var name = ((Button)sender).Tag.ToString();
            //找到自定义分组
            var custemModel = FunctionConfigViewModel.CustomModuleViews.FirstOrDefault(n => n.ModuleName == name);
            if (custemModel != null)
            {
                //遍历自定义分组中的所有功能项，通过功能项找到系统分组中的对应功能项，并修改显示状态
                foreach (var item in custemModel.MainMenuViews)
                {
                    var sysFunction = FunctionConfigViewModel.SysModuleViews.First(n => n.MainMenuViews.Any(m => m.Id == item.Id)).MainMenuViews.FirstOrDefault(n => n.Id == item.Id);
                    if (sysFunction != null)
                    {
                        if (custemModel.ModuleName == "WebModule"){
                            sysFunction.Show = Visibility.Visible;
                        }else { 
                            //查找权限
                            if (!sysFunction.Auth || sysFunction.Permission.IsGrantedPermission()){
                                sysFunction.Show = Visibility.Visible;
                            }
                        }
                    }
                }
                FunctionConfigViewModel.CheckMenuSetShowCommand.Execute(null);
                FunctionConfigViewModel.CustomModuleViews.Remove(custemModel);
            }
        }
    }
}
