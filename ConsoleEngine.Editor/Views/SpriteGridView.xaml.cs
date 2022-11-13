using ConsoleEngine.Editor.ViewModels;
using System.Windows.Controls;

namespace ConsoleEngine.Editor.Views
{
    /// <summary>
    /// Interaction logic for SpriteGridView.xaml
    /// </summary>
    public partial class SpriteGridView : UserControl, IView<ISpriteEditorViewModel>
    {
        public SpriteGridView()
        {
            InitializeComponent();
        }
    }
}
