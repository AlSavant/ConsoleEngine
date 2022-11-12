using DependencyInjection;
using SpriteEditor.Views;
using System.Windows.Controls;

namespace SpriteEditor.Services.Factories.Implementations
{
    internal sealed class ViewFactory : IViewFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ViewFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public T CreateView<T>() where T : Control, IView, new()
        {
            var control = new T();
            var dataModelType = typeof(T).GetInterface("IView`1").GetGenericArguments()[0];
            control.DataContext = serviceProvider.Resolve(dataModelType);
            return control;
        }
    }
}
