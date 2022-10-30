namespace ConsoleEngine.Services.Util.Time
{
    public interface ITimeReferenceService : IService
    {
        float DeltaTime { get; set; }
        float Time { get; set; }
    }
}
