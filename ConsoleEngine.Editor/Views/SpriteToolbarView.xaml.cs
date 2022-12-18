using ConsoleEngine.Editor.ViewModels;
using System.Windows.Controls;

namespace ConsoleEngine.Editor.Views
{
    /// <summary>
    /// Interaction logic for SpriteToolbarView.xaml
    /// </summary>
    public partial class SpriteToolbarView : UserControl, IView<ISpriteToolbarViewModel>
    {
        public SpriteToolbarView()
        {
            DataContext = null;
            InitializeComponent();
        }
    }
}
