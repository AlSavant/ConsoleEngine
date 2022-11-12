using SpriteEditor.Views;
using System.Windows.Controls;

namespace SpriteEditor.Services.Factories
{
    internal interface IViewFactory : IService
    {
        T CreateView<T>() where T : Control, IView, new();
    }
}
