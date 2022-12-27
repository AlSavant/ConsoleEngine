using ConsoleEngine.Editor.ViewModels;

namespace ConsoleEngine.Editor.Services.Factories
{
    internal interface IViewModelFactory : IService
    {
        T? CreateViewModel<T>() where T : IViewModel;
    }
}
