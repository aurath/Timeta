using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Timeta.Controls
{
    public class Incrementer : Control
    {
        static Incrementer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Incrementer),
                new FrameworkPropertyMetadata(typeof(Incrementer)));
        }

        public Incrementer()
        {
            CommandBindings.Add(new CommandBinding(Increment, ExecutedIncrement));
            CommandBindings.Add(new CommandBinding(Set, ExecutedSet));
        }

        public int CurrentValue
        {
            get { return (int)GetValue(CurrentValueProperty); }
            set { SetValue(CurrentValueProperty, value); }
        }

        public void IncrementBy(int addition)
        {
            CurrentValue += addition;
        }

        public void SetTo(int value)
        {
            CurrentValue = value;
        }

        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register("CurrentValue", typeof(int), typeof(Incrementer), 
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly RoutedCommand Increment = new RoutedCommand(nameof(Increment), typeof(Incrementer));

        public static readonly RoutedCommand Set = new RoutedCommand(nameof(Set), typeof(Incrementer));

        private void ExecutedIncrement(object sender, ExecutedRoutedEventArgs args)
        {
            var addition = args?.Parameter switch
            {
                int val => val,
                string raw when int.TryParse(raw, out int val) => val,
                _ => 1
            };
            IncrementBy(addition);
        }

        private void ExecutedSet(object sender, ExecutedRoutedEventArgs args)
        {
            var newVal = args.Parameter switch
            {
                int val => val,
                string raw when int.TryParse(raw, out int val) => val,
                _ => throw new ArgumentException("Parameter must be int or string.")
            };
            SetTo(newVal);
        }
    }
}
