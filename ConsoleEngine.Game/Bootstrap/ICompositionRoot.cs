namespace ConsoleEngine.Game.Bootstrap
{
    internal interface ICompositionRoot
    {
        void Run(CancellationTokenSource cancellationTokenSource);
        void Update();
        void LateUpdate();
        void Stop();        
    }
}
