using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Timeta.Views;
using Timeta.Domain.ViewModels;
using Timeta.Domain.Services;

namespace Timeta
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ThreadTimerService TimerService { get; private set; }

        private void AppStartup(object sender, StartupEventArgs e)
        {
            ResolveServices();
            InitShell();
            BindServices();
            MainWindow.Show();
        }

        private void ResolveServices()
        {
            TimerService = Resources["TimerService"] as ThreadTimerService;
            if (TimerService == null) throw new InvalidOperationException("Timer Service Unavailable on Startup.");
        }

        private void InitShell()
        {
            MainWindow = new MainWindow()
            { DataContext = new MainWindowViewModel(TimerService) };
        }

        private void BindServices()
        {
            MainWindow.CommandBindings.Add(new CommandBinding(
                Commands.SetTimer, 
                (sender, args) =>
                {
                    if (args.Parameter is string raw)
                    {
                        if (int.TryParse(raw, out int newSeconds))
                        {
                            TimerService.SetTimer(newSeconds);
                        }
                    }
                },
                (sender, args) => 
                {
                    if (args.Parameter is string raw)
                    {
                        if (int.TryParse(raw, out int newSeconds))
                        {
                            args.CanExecute = TimerService.CanSetTimer(newSeconds);
                            return;
                        }
                    }
                    args.CanExecute = false;
                }));
        }

    }
}
