namespace ConsoleEngine.Systems
{
    public interface ISystem
    {
        bool IsRunning { get; }
        bool IsAutoRun();
        void Run();
        void Update();
        void LateUpdate();
        void Stop();
    }
}
