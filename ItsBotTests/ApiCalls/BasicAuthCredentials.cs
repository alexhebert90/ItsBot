using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http.Headers;
using ItsBot.ApiCalls;

namespace ItsBot.Tests.ApiCalls
{
    /// <summary>
    /// Tests against the <see cref="BasicAuthCredentials"/> class.
    /// </summary>
    [TestClass]
    public class BasicAuthCredentialsTests
    {
        private const string Basic = "Basic";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorUserNameEnforced()
        {
            var testCreds = new
                BasicAuthCredentials(username: null, password: "password");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorPasswordEnforced()
        {
            var testCreds = new
                BasicAuthCredentials(username: "userName", password: null);
        }

        [TestMethod]
        public void CorrectHeaderGenerated()
        {
            var test1 = new
                BasicAuthCredentials("TestUser", "TestPassword");

            Assert.AreEqual(test1.AsAuthHeader(), new AuthenticationHeaderValue(Basic, "VGVzdFVzZXI6VGVzdFBhc3N3b3Jk"), "Auth header output is not as expected.");

            var test2 = new
                BasicAuthCredentials("GeddyLee", "playsPrettyGoodBass");

            Assert.AreEqual(test2.AsAuthHeader(), new AuthenticationHeaderValue(Basic, "R2VkZHlMZWU6cGxheXNQcmV0dHlHb29kQmFzcw=="), "Auth header output is not as expected.");
        }
    }
}
