using ItsBot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ItsBot.Tests
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
            const string ClientId = "ClientId";
            const string ClientSecret = "ClientSecret";
            const string UserAgent = "UserAgent";

            var creds = new BotCredentials(ClientId, ClientSecret, UserAgent);

            Assert.AreEqual(ClientId, creds.ClientId);
            Assert.AreEqual(ClientSecret, creds.ClientSecret);
            Assert.AreEqual(UserAgent, creds.UserAgent);
        }

    }
}
