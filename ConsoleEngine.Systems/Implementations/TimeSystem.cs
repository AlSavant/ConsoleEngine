using ConsoleEngine.Services;
using System.Diagnostics;

namespace ConsoleEngine.Systems.Implementations
{
    internal sealed class TimeSystem : System, ITimeSystem
    {
        private readonly ITimeReferenceService timeReferenceService;
        private readonly Stopwatch stopwatch;

        public TimeSystem(ITimeReferenceService timeReferenceService)
        {
            this.timeReferenceService = timeReferenceService;
            stopwatch = new Stopwatch();
        }

        public override void Run()
        {
            base.Run();
            timeReferenceService.Time = 0f;
            timeReferenceService.DeltaTime = 0f;
            stopwatch.Start();
        }

        public override void Stop()
        {
            stopwatch.Stop();
            base.Stop();
        }

        public override void Update()
        {          
            var seconds = stopwatch.Elapsed.TotalSeconds;
            timeReferenceService.DeltaTime = (float)(seconds - timeReferenceService.Time);
            timeReferenceService.Time = (float)seconds;
        }
    }
}
