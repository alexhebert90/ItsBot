using System;
using ItsBot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace ItsBot.Tests
{
    /// <summary>
    /// Tests against the <see cref="RateLimiter"/> class.
    /// </summary>
    [TestClass]
    public class RateLimiterTests
    {
        /// <summary>
        /// Validates the limiter does not accept null functions, as this makes no sense (for the time being).
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Null functions should not be accepted by the limiter.")]
        public async Task NullFuncResultsInArgumentNullAsync()
        {
            // Showing off with C# 7.1... The timespan doesn't matter for this test. Honestly I'm not sure what
            // the default for TimeSpan is... it's got to be a time of "0" ticks, right?
            await new RateLimiter(default).LimitAsync<string>(null);
        }



        /// <summary>
        /// Validates that there is in fact a limit in place and that it is behaving like a limiter should.
        /// This is not an exact science and doesn't need to be, as long as the limited function is not running faster
        /// than a maximum projected speed.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task LimitEnforcedAsync()
        {
            // The definition of arbitrary. For this test, I'm suggesting that the limited function should
            // execute at least 87% of the times it could in a best-case scenario in order to be considered valid.
            const double FinalCallTolerancePerecentage = 87.0;

            // Set the amount of time to let this test run for total.
            var testDuration = TimeSpan.FromSeconds(3.23);

            // Define arbitrary wait limit to enforce between actions.
            var rateLimit = TimeSpan.FromMilliseconds(15);

            // Set what the maximum possible call count should be based on the test parameters.
            var maximumCallCount = testDuration / rateLimit;

            // Set up the limiter instance.
            var limiter = new RateLimiter(rateLimit);

            int callCounter = 0;
            var testStartTime = DateTime.Now;
            
            // Call the limiter function for the duration specified by the test duration.
            while(DateTime.Now <= testStartTime + testDuration)
            {
                await limiter.LimitAsync(() => Task.FromResult(0));
                callCounter++;
            }

            // Make sure the total call count is within a valid range. It should not be larger than the maximum we've decided on.
            Assert.IsTrue(callCounter <= maximumCallCount);

            var callCountPercentageOfMax = callCounter / maximumCallCount * 100;

            // Make sure the total number of calls made was still close to the maximum possible in the time window, with some tolerance
            // to handle the unknown.
            Assert.IsTrue(callCountPercentageOfMax > FinalCallTolerancePerecentage);
        }
    }
}
