using DependencyInjection.Windsor;
using SpriteEditor.DependencyInjection;
using SpriteEditor.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using IServiceProvider = DependencyInjection.IServiceProvider;

namespace SpriteEditor
{    
    public partial class App : Application
    {
        private IServiceProvider serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            serviceProvider = new ServiceCollection()
                .AddServices()
                .BuildProvider();            
            CompositionTarget.Rendering += InjectWindows;            
        }

        private void InjectWindows(object sender, EventArgs args)
        {
            if (Windows == null || Windows.Count <= 0)
                return;
            for(int i = 0; i < Windows.Count; i++)
            {
                var window = Windows[i];
                if (window.DataContext != null)
                    continue;
                var view = window as IView;
                if (view == null)
                    continue;
                InjectView(view);
                window.Loaded -= OnWindowLoaded;
                window.Loaded += OnWindowLoaded;                
            }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs args)
        {
            var window = (Window)sender;
            window.Loaded -= OnWindowLoaded;
            foreach (var controlView in FindVisualChilds<UserControl>(window))
            {
                var view = controlView as IView;
                if (view == null)
                    continue;
                InjectView(view);
            }
        }

        private void InjectView(IView view)
        {
            var modelType = view.GetType().GetInterface("IView`1").GetGenericArguments()[0];            
            var control = (Control)view;
            control.DataContext = serviceProvider.Resolve(modelType);
        }

        private IEnumerable<T> FindVisualChilds<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) 
                yield return (T)Enumerable.Empty<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
                if (ithChild == null) 
                    continue;
                if (ithChild is T t) 
                    yield return t;
                foreach (T childOfChild in FindVisualChilds<T>(ithChild)) 
                    yield return childOfChild;
            }
        }
    }                               
}
