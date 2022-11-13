using ConsoleEngine.Editor.ViewModels;

namespace ConsoleEngine.Editor.Views
{
    internal interface IView
    {
    }

    internal interface IView<T> : IView where T : IViewModel
    {

    }
}
