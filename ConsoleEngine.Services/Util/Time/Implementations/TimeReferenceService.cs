using ConsoleEngine.Services.Util.Time;

namespace ConsoleEngine.Services.Util.Time.Implementations
{
    internal sealed class TimeReferenceService : ITimeReferenceService
    {
        public float DeltaTime { get; set; }
        public float Time { get; set; }
    }
}
