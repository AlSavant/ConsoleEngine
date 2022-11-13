using SpriteEditor.ViewModels;
using System.Windows.Controls;

namespace SpriteEditor.Views
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
