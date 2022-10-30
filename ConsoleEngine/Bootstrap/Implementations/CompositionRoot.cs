using ConsoleEngine.Systems;
using IServiceProvider = DependencyInjection.IServiceProvider;

namespace ConsoleEngine.Bootstrap.Implementations
{
    internal class CompositionRoot : ICompositionRoot
    {
        private CancellationTokenSource? cancellationTokenSource;

        private readonly ISystem[] systems;

        public CompositionRoot(IServiceProvider serviceProvider)
        {
            var types = typeof(ISystem).Assembly.GetTypes().Where(x =>
            x.IsInterface && !x.IsGenericType && typeof(ISystem).IsAssignableFrom(x) &&
            x != typeof(ISystem));

            List<ISystem> systemList = new List<ISystem>();
            foreach(var type in types)
            {
                ISystem? system = serviceProvider.Resolve(type) as ISystem;
                if(system != null)
                {
                    systemList.Add(system);
                }                
            }
            systems = systemList.ToArray();
        }

        public void Run(CancellationTokenSource cancellationTokenSource)
        {
            this.cancellationTokenSource = cancellationTokenSource;
            for(int i = 0; i < systems.Length; i++)
            {
                systems[i].Run();
            }
        }
       
        public void Update()
        {
            for (int i = 0; i < systems.Length; i++)
            {
                systems[i].Update();
            }
        }

        public void LateUpdate()
        {
            for (int i = 0; i < systems.Length; i++)
            {
                systems[i].LateUpdate();
            }
        }

        public void Stop()
        {
            for (int i = 0; i < systems.Length; i++)
            {
                systems[i].Stop();
            }
            cancellationTokenSource?.Cancel();
        }
    }
}
