using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using Timeta.Domain.Framework;

namespace Timeta.Domain.Services
{
    public class ThreadTimerService : NotifyPropertyChanged
    {
        public ThreadTimerService()
        {
            Lock = new object();
            Seconds = 0;
            BackgroundThread = new Thread(new ThreadStart(BackgroundThreadEntry));
            BackgroundThread.SetApartmentState(ApartmentState.STA);
            BackgroundThread.IsBackground = true;
            BackgroundThread.Start();
            TimeChanged?.Invoke(this, EventArgs.Empty);
        }

        private Thread BackgroundThread { get; }

        private object Lock { get; }

        private void BackgroundThreadEntry()
        {
            while (true)
            {
                Thread.Sleep(1000);
                //Use a single lock to both get and set
                lock(Lock)
                {
                    seconds++;
                }
                TimeChanged?.Invoke(this, EventArgs.Empty);
                OnPropertyChanged(nameof(Seconds));
            }
        }

        #region Public API

        private int seconds;
        public int Seconds
        {
            get
            {
                lock (Lock) { return seconds; }
            }
            set
            {
                lock (Lock) { seconds = value; }
                TimeChanged?.Invoke(this, EventArgs.Empty);
                OnPropertyChanged();
            }
        }

        public bool CanSetTimer(int newSeconds)
        {
            //if (BackgroundThread.ThreadState != ThreadState.Running) return false;
            if (newSeconds < 0) return false;
            return true;
        }

        public void SetTimer(int newSeconds)
        {
            Seconds = newSeconds;
        }

        public event EventHandler<EventArgs> TimeChanged;

        #endregion
    }
}
