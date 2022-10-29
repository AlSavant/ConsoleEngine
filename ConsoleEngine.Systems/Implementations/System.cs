namespace ConsoleEngine.Systems.Implementations
{
    internal abstract class System : ISystem
    {
        public virtual bool IsAutoRun()
        {
            return false;
        }

        private bool isRunning;
        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
            private set
            {
                if (isRunning != value)
                {
                    isRunning = value;                    
                }
            }
        }        

        public virtual void Run()
        {
            IsRunning = false;
        }

        public virtual void Stop()
        {
            IsRunning = false;
        }

        public virtual void Update() { }
        public virtual void LateUpdate() { }
        public virtual void FixedUpdate() { }
        public virtual void LateFixedUpdate() { }
    }
}
