using ConsoleEngine.Editor.Views;
using System;
using System.Windows.Controls;
using IServiceProvider = DependencyInjection.IServiceProvider;

namespace ConsoleEngine.Editor.Services.Factories.Implementations
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
            var interf = typeof(T).GetInterface("IView`1");
            if (interf == null)
                throw new InvalidOperationException($"Unsupported type {typeof(T)}!");
            var dataModelType = interf.GetGenericArguments()[0];
            control.DataContext = serviceProvider.Resolve(dataModelType);
            return control;
        }
    }
}
