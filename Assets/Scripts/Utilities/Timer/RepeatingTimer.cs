using System;

namespace Core.Utilities.Timer
{
    public class RepeatingTimer : Timer
    {
        public RepeatingTimer(float newTime, Action onElapsed = null) : base(newTime, onElapsed) { }

        public override bool Tick(float deltaTime)
        {
            if (AssessTime(deltaTime))
            {
                Reset();
            }

            return false;
        }
    }
}
