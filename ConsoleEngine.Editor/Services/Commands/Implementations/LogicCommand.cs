using GalaSoft.MvvmLight.CommandWpf;
using System;

namespace ConsoleEngine.Editor.Services.Commands.Implementations
{
    internal abstract class LogicCommand : ILogicCommand
    {
        private readonly RelayCommand relayCommand;

        public LogicCommand()
        {
            relayCommand = new RelayCommand(ExecuteCommand, CanExecuteCommand);
            relayCommand.CanExecuteChanged += CanExecuteChanged;            
        }        

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return relayCommand.CanExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            relayCommand.Execute(parameter);
        }

        protected abstract bool CanExecuteCommand();
        protected abstract void ExecuteCommand();
    }

    internal abstract class LogicCommand<T> : ILogicCommand<T>
    {
        private readonly RelayCommand<T> relayCommand;

        public LogicCommand()
        {
            relayCommand = new RelayCommand<T>(ExecuteCommand, CanExecuteCommand);
            relayCommand.CanExecuteChanged += CanExecuteChanged;
        }

        public event EventHandler? CanExecuteChanged;

        private Func<object?, T>? resolver;
        public void SetParameterResolver(Func<object?, T> resolver)
        {
            this.resolver = resolver;
        }

        public bool CanExecute(object? parameter)
        {
            if(resolver != null)
            {
                parameter = resolver(parameter);
            }
            return relayCommand.CanExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            if (resolver != null)
            {
                parameter = resolver(parameter);
            }
            relayCommand.Execute(parameter);
        }

        protected abstract bool CanExecuteCommand(T param);
        protected abstract void ExecuteCommand(T param);
    }
}
