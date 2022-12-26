using CommunityToolkit.Mvvm.Input;
using System;

namespace ConsoleEngine.Editor.Services.Commands
{
    internal interface ILogicCommand : IService, IRelayCommand
    {
       
    }

    internal interface ILogicCommand<T> : ILogicCommand
    {
        void SetParameterResolver(Func<object?, T> resolver);
    }
}
