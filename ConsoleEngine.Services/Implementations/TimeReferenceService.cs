namespace ConsoleEngine.Services.Implementations
{
    internal sealed class TimeReferenceService : ITimeReferenceService
    {
        public float DeltaTime { get; set; }
        public float Time { get; set; }
    }
}
