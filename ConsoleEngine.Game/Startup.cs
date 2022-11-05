using ConsoleEngine.Game.Bootstrap.Implementations;
using Nito.AsyncEx;

namespace ConsoleEngine.Game
{
    internal class Startup
    {
        [STAThread]
        static void Main(string[] args)
        {
            AsyncContext.Run(() =>
            {
                try
                {
                    var compositionRoot = new Bootstrapper().BootstrapCompositionRoot();
                    var cancellationTokenSource = new CancellationTokenSource();
                    compositionRoot.Run(cancellationTokenSource);
                    while (!cancellationTokenSource.IsCancellationRequested)
                    {
                        compositionRoot.Update();
                        compositionRoot.LateUpdate();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());                    
                }
            });                              
        }
    }
}