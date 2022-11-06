using SpriteEditor.ViewModels;
using System.Windows;

namespace SpriteEditor.Views
{
    /// <summary>
    /// Interaction logic for ScaleCanvasView.xaml
    /// </summary>
    public partial class ScaleCanvasView : Window
    {
        public ScaleCanvasView()
        {
            InitializeComponent();
            DataContext = new ScaleCanvasViewModel();
        }
    }
}
