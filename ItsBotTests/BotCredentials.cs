using ItsBot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ItsBotTests
{
    /// <summary>
    /// Tests related to the <see cref="BotCredentials"/> class.
    /// </summary>
    [TestClass]
    public class BotCredentialsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Client id should not be allowed as null.")]
        public void ClientIdNotNull()
        {
            new BotCredentials(clientId: null, clientSecret: "secret", userAgent: "agent");
        }

            
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Client secret should not be allowed as null.")]
        public void ClientSecretNotNull()
        {
            new BotCredentials(clientId: "clientId", clientSecret: null, userAgent: "agent");
        }

            
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "User agent should not be allowed as null.")]
        public void UserAgentNotNull()
        {
            new BotCredentials(clientId: "clientId", clientSecret: "secret", userAgent: null);
        }

        [TestMethod]
        public void ValidArgumentsMapped()
        {
            const string CLIENT_ID = "ClientId";
            const string CLIENT_SECRET = "ClientSecret";
            const string USER_AGENT = "UserAgent";

            var creds = new BotCredentials(CLIENT_ID, CLIENT_SECRET, USER_AGENT);

            Assert.AreEqual(CLIENT_ID, creds.ClientId);
            Assert.AreEqual(CLIENT_SECRET, creds.ClientSecret);
            Assert.AreEqual(USER_AGENT, creds.UserAgent);
        }

    }
}
