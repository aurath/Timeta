using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;

namespace Timeta.Domain.Framework
{
    public class RelayCommand : ICommand
    {
        private Action<object> CommandExecuted { get; }

        private Func<object, bool> CommandCanExecute { get; }

        /// <summary>
        /// Constructs a new <see cref="RelayCommand"/> with an Executed delegate without parameters.
        /// </summary>
        /// <param name="commandExecuted">An Executed delegate without no parameter.</param>
        public RelayCommand(Action commandExecuted) 
        {
            CommandExecuted = _ => commandExecuted();
            CommandCanExecute = _ => true;
        }

        /// <summary>
        /// Constructs a new <see cref="RelayCommand"/> with both delegates.
        /// </summary>
        /// <param name="commandExecuted">An Executed delegate with no parameter.</param>
        /// <param name="commandCanExecute">A CanExecute delegate with no parameter.</param>
        public RelayCommand(Action commandExecuted, Func<bool> commandCanExecute)
        {
            CommandExecuted = _ => commandExecuted();
            CommandCanExecute = _ => commandCanExecute();
        }

        /// <summary>
        /// Construct a new <see cref="RelayCommand"/> with a Executed delegate accepting a CommandParameter
        /// </summary>
        /// <param name="executed">An Executed delegate with a CommandParameter.</param>
        public RelayCommand(Action<object> executed)
        {
            CommandExecuted = executed;
            CommandCanExecute = _ => true;
        }

        /// <summary>
        /// Construct a new <see cref="RelayCommand"/> with both delegates accepting a CommandParameter
        /// </summary>
        /// <param name="executed">An Executed delegate with a CommandParameter.</param>
        /// <param name="canExecute">A CanExecute delegate with a CommandParameter.</param>
        public RelayCommand(Action<object> executed, Func<object, bool> canExecute)
        {
            CommandExecuted = executed;
            CommandCanExecute = canExecute;
        }

        /// <summary>
        /// Cause the command to invoke the CanExecuteChanged event.
        /// Call this to manually invalidate the CanExecute value.
        /// </summary>
        public void OnCommandCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region ICommand Implementation

        public bool CanExecute(object parameter) =>
            CommandCanExecute(parameter);

        public void Execute(object parameter)
        {
            if(CanExecute(parameter)) 
                CommandExecuted(parameter);
        }

        public event EventHandler CanExecuteChanged;

        #endregion
    }
}
