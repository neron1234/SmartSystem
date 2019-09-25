using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace MMK.SmartSystem.Laser.Base.CustomControl
{
    public class FlatToggleButton : ToggleButton
    {
        static FlatToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatToggleButton), new FrameworkPropertyMetadata(typeof(FlatToggleButton)));
        }
    }
}
