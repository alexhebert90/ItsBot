using System;
using System.Threading.Tasks;

namespace ItsBot
{
    /// <summary>
    /// Helper class that limits actions to never exceed the rate provided.
    /// </summary>
    internal class RateLimiter
    {
        /// <summary>
        /// Creates a limiter instance where the minimum time between requests is the provided interval.
        /// </summary>
        /// <param name="requestInterval"></param>
        public RateLimiter(TimeSpan requestInterval)
        {
            RequestInterval = requestInterval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="limitedFunction"></param>
        /// <returns></returns>
        public async Task<T> LimitAsync<T>(Func<Task<T>> limitedFunction)
        {
            // Validate parameter is not null. It does not make sense to be.
            if (limitedFunction == null)
                throw new ArgumentNullException(nameof(limitedFunction));


            // Deal immediately with the case that no requests have been made yet.
            // We know no limiting needs to be done in this case.
            if (LastActionHit == null)
            {
                return await CallActionAsync(limitedFunction);
            }

            // Get the amount of time that's passed since the last time the limiter was called.
            var timeSinceLastAction =
                DateTime.Now - LastActionHit.Value;

            // If enough time hasn't passed since the last action, asynchronously wait
            // until it's safe to call.
            if (timeSinceLastAction < RequestInterval)
            {
                var timeToWait = RequestInterval - timeSinceLastAction;
                await (Task.Delay(timeToWait));
            }

            // Call the original function after waiting.
            return await CallActionAsync(limitedFunction);
        }

        /// <summary>
        /// A record of the last time the action to be limited was hit.
        /// </summary>
        private DateTime? LastActionHit { get; set; }

        /// <summary>
        /// How far to space apart calls in time.
        /// </summary>
        private TimeSpan RequestInterval { get; }


        /// <summary>
        /// Private helper to wrap the logic of calling a function and setting the last call time.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        private async Task<T> CallActionAsync<T>(Func<Task<T>> func)
        {
            LastActionHit = DateTime.Now;
            return await func();
        }
    }
}
