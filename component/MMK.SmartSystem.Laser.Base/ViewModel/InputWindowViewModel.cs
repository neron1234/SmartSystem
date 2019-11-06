using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.ViewModel
{
    public class InputWindowViewModel : ViewModelBase
    {
        public ObservableCollection<InputItemModel> InputButtonItems = new ObservableCollection<InputItemModel>();

        public string Title { get; set; }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    RaisePropertyChanged(() => Value);
                }
            }
        }
        private bool _canSave;
        public bool CanSave
        {
            get { return _canSave; }
            set
            {
                if (_canSave != value)
                {
                    _canSave = value;
                    RaisePropertyChanged(() => CanSave);
                }
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Value = "";
                    CanSave = false;
                });
            }
        }
        public InputWindowViewModel()
        {
            InputButtonItems.Add(new InputItemModel() { Text = "7" });
            InputButtonItems.Add(new InputItemModel() { Text = "8" });
            InputButtonItems.Add(new InputItemModel() { Text = "9" });
            InputButtonItems.Add(new InputItemModel() { Text = "4" });
            InputButtonItems.Add(new InputItemModel() { Text = "5" });
            InputButtonItems.Add(new InputItemModel() { Text = "6" });
            InputButtonItems.Add(new InputItemModel() { Text = "1" });
            InputButtonItems.Add(new InputItemModel() { Text = "2" });
            InputButtonItems.Add(new InputItemModel() { Text = "3" });
            InputButtonItems.Add(new InputItemModel() { Text = "⬅" });
            InputButtonItems.Add(new InputItemModel() { Text = "0" });
            InputButtonItems.Add(new InputItemModel() { Text = "." });

            InputButtonItems.ToList().ForEach(d => d.ButtonInputEvent += ButtonClickEvent);
        }

        private void ButtonClickEvent(InputItemModel item) {
            var number = 0;
            if (int.TryParse(item.Text, out number)) {
                Value = Convert.ToDouble(Value + number).ToString();
            }else{
                if (item.Text == "." && !Value.Contains(".")){
                    Value += item.Text;
                }else{
                    if (Value.Length > 0){
                        Value = Value.Substring(0, Value.Length - 1);
                    }
                }
            }
            CheckCanSave();
        }

        public void CheckCanSave()
        {
            if (string.IsNullOrEmpty(Value) || Value.LastOrDefault() == '.' || (Convert.ToDouble(Value) > MaxValue || Convert.ToDouble(Value) < MinValue))
            {
                CanSave = false;
                return;
            }
            CanSave = true;
        }
    }

    public class InputItemModel
    {
        public string Text { get; set; }
        public Visibility Show
        {
            get { return string.IsNullOrEmpty(Text) ? Visibility.Hidden : Visibility.Visible; }
        }
        public event Action<InputItemModel> ButtonInputEvent;
        public ICommand InputCommand
        {
            get
            {
                return new RelayCommand(() => ButtonInputEvent?.Invoke(this));
            }
        }
    }
}
