namespace ConsoleEngine.Services
{
    public interface ITimeReferenceService : IService
    {
        float DeltaTime { get; set; }
        float Time { get; set; }
    }
}
