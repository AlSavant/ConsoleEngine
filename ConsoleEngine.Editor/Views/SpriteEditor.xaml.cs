using ConsoleEngine.Editor.ViewModels;
using ConsoleEngine.Editor.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace ConsoleEngine.Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IView<ISpriteEditorViewModel>
    {
        public MainWindow()
        {
            DataContext = null;
            InitializeComponent();
        }

        //Collapse MenuItem when item is clicked
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Hyperlink;
            if (b == null)
                return;
            var mi = FindParent<MenuItem>(b.Parent);
            if (mi == null)
                return;
            mi.RaiseEvent(
               new MouseButtonEventArgs(
                  Mouse.PrimaryDevice, 0, MouseButton.Left
               )
               { RoutedEvent = Mouse.MouseUpEvent }
            );
        }

        private T? FindParent<T>(DependencyObject child)
        where T : DependencyObject
        {
            if (child == null) return null;

            T? foundParent = null;
            var currentParent = VisualTreeHelper.GetParent(child);

            do
            {
                var frameworkElement = currentParent as FrameworkElement;
                if (frameworkElement is T)
                {
                    foundParent = (T)currentParent;
                    break;
                }

                currentParent = VisualTreeHelper.GetParent(currentParent);

            } while (currentParent != null);

            return foundParent;
        }           
    }
}
