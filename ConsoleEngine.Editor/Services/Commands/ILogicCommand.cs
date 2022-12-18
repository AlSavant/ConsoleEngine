using System;
using System.Windows.Input;

namespace ConsoleEngine.Editor.Services.Commands
{
    internal interface ILogicCommand : IService, ICommand
    {
    }

    internal interface ILogicCommand<T> : ILogicCommand
    {
        void SetParameterResolver(Func<object?, T> resolver);
    }
}
