using ItsBot.TokenManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ItsBot.Tests.TokenManagement
{
    /// <summary>
    /// Tests against the <see cref="SuccessfulTokenResult"/> class.
    /// </summary>
    [TestClass]
    public class SuccessfulTokenResultTests
    {

        /// <summary>
        /// Validates that null is not an appropriate value for the token argument of the constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Constructor expected to throw exception for null input.")]
        public void ConstructorResponseObjectEnforced()
        {
            new SuccessfulTokenResult(null);
        }

        /// <summary>
        /// Checks exact timing of the "expires" field using the ability to pass in the current time.
        /// </summary>
        [TestMethod]
        public void ValidateExpiresOn()
        {
            const int TestMinuteCount = 453;

            DateTime timeFetched = DateTime.Now;

            DateTime expected = timeFetched + TimeSpan.FromMinutes(TestMinuteCount);

            var result = new SuccessfulTokenResult(new NewTokenResponse
            {
                ExpiresIn = TestMinuteCount
            }, 
            tokenRetrievedAt: timeFetched);

            Assert.AreEqual(result.TokenExpiresAt, expected);
        }

        /// <summary>
        /// Checks that the expires field is "close enough" when an acquired time is not passed in.
        /// </summary>
        [TestMethod]
        public void ValidateExpiresOnNonExact()
        {
            const int TestMinuteCount = 24;

            var result = new SuccessfulTokenResult(new NewTokenResponse
            {
                ExpiresIn = TestMinuteCount
            });

            var expected = DateTime.Now + TimeSpan.FromMinutes(TestMinuteCount);

            TimeSpan difference = expected - result.TokenExpiresAt;

            Assert.IsTrue(difference.TotalMilliseconds <= 20);
        }

        /// <summary>
        /// Tests that the instance passed into the class is bound correctly and maintained.
        /// </summary>
        [TestMethod]
        public void TokenResponseInstanceMaintained()
        {
            var tokenResponse = new NewTokenResponse();

            var result = new SuccessfulTokenResult(tokenResponse);

            Assert.AreEqual(tokenResponse, result.TokenResponse);
        }

    }
}
