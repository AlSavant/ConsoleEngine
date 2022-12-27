using System.Windows;

namespace ConsoleEngine.Editor.Services.Commands.Implementations
{
    internal sealed class QuitApplicationCommand : LogicCommand, IQuitApplicationCommand
    {
        protected override bool CanExecuteCommand(object? parameter)
        {
            return true;
        }

        protected override void ExecuteCommand(object? parameter)
        {
            Application.Current.Shutdown();
        }
    }
}
