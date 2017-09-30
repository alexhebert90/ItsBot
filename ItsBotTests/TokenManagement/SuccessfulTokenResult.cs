using ItsBot.TokenManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ItsBotTests.TokenManagement
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
            const int TESTMINUTECOUNT = 453;

            DateTime timeFetched = DateTime.UtcNow;

            DateTime expected = timeFetched + TimeSpan.FromMinutes(TESTMINUTECOUNT);

            var result = new SuccessfulTokenResult(new NewTokenResponse
            {
                ExpiresIn = TESTMINUTECOUNT
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
            const int TESTMINUTECOUNT = 24;

            var result = new SuccessfulTokenResult(new NewTokenResponse
            {
                ExpiresIn = TESTMINUTECOUNT
            });

            var expected = DateTime.UtcNow + TimeSpan.FromMinutes(TESTMINUTECOUNT);

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
