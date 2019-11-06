using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Timeta.Domain.Framework;
using Timeta.Domain.Services;
using System.ComponentModel;
using System.Collections;

namespace Timeta.Domain.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        private int currentTime;
        public int CurrentTime
        {
            get => currentTime;
            set { currentTime = value; OnPropertyChanged(); }
        }

        private int newTime;
        public int NewTime
        {
            get => newTime;
            set { newTime = value; OnPropertyChanged(); }
        }

        public ThreadTimerService TimerService { get; }

        public MainWindowViewModel(ThreadTimerService timerService)
        {
            TimerService = timerService ?? throw new ArgumentNullException(nameof(timerService));
            TimerService.TimeChanged += UpdateTime;
        }

        private void UpdateTime(object sender, EventArgs e)
        {
            CurrentTime = TimerService.Seconds;
        }

        #region Commands

        private RelayCommand resetTimerCommand;
        public ICommand ResetTimerCommand
        {
            get => resetTimerCommand ?? (resetTimerCommand = new RelayCommand(ExecutedResetTimer, CanExecuteResetTimer));
        }

        private bool CanExecuteResetTimer()
        {
            return TimerService?.CanSetTimer(0) ?? false;
        }

        private void ExecutedResetTimer()
        {
            if(CanExecuteResetTimer())
                TimerService.SetTimer(0);
        }

        private RelayCommand setTimerCommand;
        public ICommand SetTimerCommand
        {
            get => setTimerCommand ?? (setTimerCommand = new RelayCommand(ExecutedSetTimer, CanExecuteSetTimer));
        }

        private bool CanExecuteSetTimer()
        {
            return TimerService?.CanSetTimer(NewTime) ?? false;
        }

        private void ExecutedSetTimer()
        {
            if (CanExecuteSetTimer())
                TimerService.SetTimer(NewTime);
        }

        #endregion
    }
}
