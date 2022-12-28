using ConsoleEngine.Editor.ViewModels;
using System.Windows.Controls;
using ConsoleEngine.Editor.ViewModels.Implementations;
using ConsoleEngine.Editor.Services.Commands.SpriteCanvas;

namespace ConsoleEngine.Editor.Views
{
    /// <summary>
    /// Interaction logic for SpriteGridView.xaml
    /// </summary>
    public partial class SpriteGridView : UserControl, IView<ISpriteGridViewModel>
    {
        public SpriteGridView()
        {
            DataContext = null;
            InitializeComponent();
        }

        private void OnGridCellClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            var context = (SpriteGridViewModel)DataContext;
            if (context == null)
                return;
            var command = (IPaintPixelCommand)context.PaintPixelCommand;
            if (command == null)
                return;
            command.Execute(((Button)sender)?.DataContext);
        }                       
    }
}
