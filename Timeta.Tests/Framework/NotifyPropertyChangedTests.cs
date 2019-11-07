using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Timeta.Domain.Framework;

namespace Timeta.Tests.Framework
{
    public class NotifyPropertyChangedTests
    {
        [Fact]
        public void TestEventFires()
        {
            bool eventFired = false;
            var obj = new MockObj();
            obj.PropertyChanged += (sender, args) => { eventFired = true; };
            obj.Number = 20;
            Assert.True(eventFired);
        }

        private class MockObj : NotifyPropertyChanged
        {
            private int number = 0;
            public int Number
            {
                get => number;
                set { number = value; OnPropertyChanged(); }
            }
        }
    }
}
