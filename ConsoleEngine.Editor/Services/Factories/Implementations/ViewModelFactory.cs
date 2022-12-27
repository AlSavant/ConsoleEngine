using ConsoleEngine.Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IServiceProvider = DependencyInjection.IServiceProvider;

namespace ConsoleEngine.Editor.Services.Factories.Implementations
{
    internal sealed class ViewModelFactory : IViewModelFactory
    {
        private readonly Dictionary<Type, Type> viewModelToViewMap;

        private readonly IServiceProvider serviceProvider;

        public ViewModelFactory(IServiceProvider serviceProvider)
        {
            viewModelToViewMap = new Dictionary<Type, Type>();
            var views = typeof(IViewModel).Assembly.GetTypes().Where(x => !x.IsInterface && !x.IsAbstract && x.GetInterface("IView`1") != null);
            foreach(var view in views)
            {
                var viewModelType = view.GetInterface("IView`1")?.GetGenericArguments()[0];
                if (viewModelType == null)
                    continue;
                if (viewModelToViewMap.ContainsKey(viewModelType))
                    continue;
                viewModelToViewMap.Add(viewModelType, view);
            }
            this.serviceProvider = serviceProvider;
        }        

        public T? CreateViewModel<T>() where T : IViewModel
        {
            if (!viewModelToViewMap.ContainsKey(typeof(T)))
                return default;
            var viewType = viewModelToViewMap[typeof(T)];
            var control = Activator.CreateInstance(viewType) as Control;
            if (control == null)
                return default;
            var viewModel = serviceProvider.Resolve<T>();
            control.DataContext = viewModel;
            (control as Window)?.ShowDialog();
            return viewModel;
        }
    }
}
