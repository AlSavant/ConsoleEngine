namespace ConsoleEngine.Bootstrap.Implementations
{
    internal class CompositionRoot : ICompositionRoot
    {
        private CancellationTokenSource? cancellationTokenSource;        

        public void Run(CancellationTokenSource cancellationTokenSource)
        {
            this.cancellationTokenSource = cancellationTokenSource;
        }
       
        public void Update()
        {
            
        }

        public void LateUpdate()
        {

        }

        public void Stop()
        {
            cancellationTokenSource?.Cancel();
        }
    }
}
