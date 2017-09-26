using System;
using System.Threading.Tasks;

namespace ItsBot
{
    internal class RateLimiter
    {
        /// <summary>
        /// A record of the last time the action to be limited was hit.
        /// </summary>
        private DateTime? LastActionHit { get; set; }

        /// <summary>
        /// How far to space apart calls in time.
        /// </summary>
        private TimeSpan RequestInterval { get; }

        public RateLimiter(TimeSpan requestInterval)
        {
            RequestInterval = requestInterval;
        }

        public async Task<T> LimitAsync<T>(Func<Task<T>> limitedFunction)
        {
            // Deal immediately with the case that no requests have been made yet.
            if(LastActionHit == null)
            {
                return await CallActionAsync(limitedFunction);
            }

            var timeSinceLastAction = 
                DateTime.UtcNow - LastActionHit.Value;


            if(timeSinceLastAction < RequestInterval)
            {
                var timeToWait = RequestInterval - timeSinceLastAction;

                await (Task.Delay(timeToWait));
            }

            return await CallActionAsync(limitedFunction);
        }

        private async Task<T> CallActionAsync<T>(Func<Task<T>> func)
        {
            LastActionHit = DateTime.UtcNow;
            return await func();
        }
    }
}
