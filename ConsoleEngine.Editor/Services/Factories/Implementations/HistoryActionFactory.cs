using ConsoleEngine.Editor.Services.History.Actions;
using IServiceProvider = DependencyInjection.IServiceProvider;

namespace ConsoleEngine.Editor.Services.Factories.Implementations
{
    internal sealed class HistoryActionFactory : IHistoryActionFactory
    {
        private readonly IServiceProvider serviceProvider;

        public HistoryActionFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public T CreateInstance<T>() where T : IHistoryAction
        {
            return serviceProvider.Resolve<T>();
        }

        public void Dispose(IHistoryAction historyAction)
        {
            serviceProvider.ReleaseComponent(historyAction);
        }
    }
}
