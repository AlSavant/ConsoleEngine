using SpriteEditor.ViewModels;

namespace SpriteEditor.Views
{
    internal interface IView
    {
    }

    internal interface IView<T> : IView where T : IViewModel
    {

    }
}
