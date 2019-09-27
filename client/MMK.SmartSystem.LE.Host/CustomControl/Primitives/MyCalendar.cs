using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MMK.SmartSystem.LE.Host.CustomControl
{
    public class MyCalendar : Calendar
    {
        static MyCalendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyCalendar), new FrameworkPropertyMetadata(typeof(MyCalendar)));
        }
    }
}
