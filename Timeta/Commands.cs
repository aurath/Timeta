using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Timeta
{
    public static class Commands
    {
        public static readonly RoutedCommand SetTimer = new RoutedCommand(nameof(SetTimer), typeof(Commands));
    }
}
