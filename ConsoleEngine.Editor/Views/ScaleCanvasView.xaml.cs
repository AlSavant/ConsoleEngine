using ConsoleEngine.Editor.ViewModels;
using System.Windows;

namespace ConsoleEngine.Editor.Views
{
    /// <summary>
    /// Interaction logic for ScaleCanvasView.xaml
    /// </summary>
    public partial class ScaleCanvasView : Window, IView<IScaleCanvasViewModel>
    {
        public ScaleCanvasView()
        {
            DataContext = null;
            InitializeComponent();
        }
    }
}
