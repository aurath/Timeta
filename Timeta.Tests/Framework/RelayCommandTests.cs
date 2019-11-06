using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Timeta.Domain.Framework;

namespace Timeta.Tests.Framework
{
    public class RelayCommandTests
    {
        [Fact]
        public void TestSimpleCommand()
        {
            bool executedCalled = false;
            void executed() { executedCalled = true; }

            var cmd = new RelayCommand(executed);

            Assert.True(cmd.CanExecute(null));

            cmd.Execute(null);

            Assert.True(executedCalled);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSimpleCanExecuteCommand(bool shouldBeAbleToExecute)
        {
            bool executedCalled = false;
            void executed() { executedCalled = true; }
            bool canExecute() { return shouldBeAbleToExecute;  }

            var cmd = new RelayCommand(executed, canExecute);

            Assert.Equal(cmd.CanExecute(null), shouldBeAbleToExecute);

            cmd.Execute(null);

            Assert.Equal(executedCalled, shouldBeAbleToExecute);
        }

        [Fact]
        public void TestParameterCommand()
        {
            object testObject = "Test Object";
            object executedWithParam = null;
            void executed(object param) { executedWithParam = param; }

            var cmd = new RelayCommand(executed);

            Assert.True(cmd.CanExecute(testObject));

            cmd.Execute(testObject);

            Assert.Equal(testObject, executedWithParam);
        }

        //Can only execute if the parameter is a string.
        [Theory]
        [InlineData(null)]
        [InlineData("Test String")]
        [InlineData("")]
        [InlineData(14)]
        public void TestCanExecuteParameterCommand(object parameter)
        {
            bool executedCalled = false;
            void executed(object param) 
            {
                Assert.IsType<string>(param);
                executedCalled = true;
            }

            bool canExecute(object param) => param is string;

            var cmd = new RelayCommand(executed, canExecute);

            Assert.Equal(cmd.CanExecute(parameter), parameter is string);

            cmd.Execute(parameter);

            Assert.Equal(executedCalled, parameter is string);
        }
    }
}
