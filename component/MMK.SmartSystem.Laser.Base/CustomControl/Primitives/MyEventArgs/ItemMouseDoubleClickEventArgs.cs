using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMK.SmartSystem.Laser.Base.CustomControl
{
    public class ItemMouseDoubleClickEventArgs<T> : EventArgs
    {
        public ItemMouseDoubleClickEventArgs() { }

        public T NewValue { get; private set; }

        public static ItemMouseDoubleClickEventArgs<T> ItemDoubleClick(T newValue)
        {
            return new ItemMouseDoubleClickEventArgs<T>() { NewValue = newValue };
        }
    }
}
