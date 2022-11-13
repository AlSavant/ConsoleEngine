using ConsoleEngine.Editor.Views;
using System.Windows.Controls;

namespace ConsoleEngine.Editor.Services.Factories
{
    internal interface IViewFactory : IService
    {
        T CreateView<T>() where T : Control, IView, new();
    }
}
