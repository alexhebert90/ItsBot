using System;
using System.Net.Http.Headers;
using System.Text;

namespace ItsBot
{
    /// <summary>
    /// A wrapper class for storing and providing "Basic" Http auth header values.
    /// </summary>
    internal class BasicAuthCredentials
    {
        /// <summary>
        /// Initializes a set of basic auth credentials using the provided username and password.
        /// <para>Both fields, while not technically required, are required in this implementation to reduce the amount of manual error checking I have to do.</para>
        /// </summary>
        /// <param name="username">Not null.</param>
        /// <param name="password">Not null.</param>
        public BasicAuthCredentials(string username, string password)
        {
            UserName = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }

        /// <summary>
        /// Converts the current basic credentials into a <see cref="AuthenticationHeaderValue"/> instance.
        /// </summary>
        /// <returns></returns>
        public AuthenticationHeaderValue AsAuthHeader()
            => new AuthenticationHeaderValue(BASIC, EncodeUserNameAndPassword());

        /// <summary>
        /// Encodes the username and password into a format expected by basic authentication.
        /// </summary>
        /// <returns></returns>
        private string EncodeUserNameAndPassword()
        {
            var bytes = Encoding.ASCII.GetBytes($"{UserName}:{Password}");

            var base64 = Convert.ToBase64String(bytes);

            return base64;
        }

        /// <summary>
        /// #Basic
        /// </summary>
        private const string BASIC = "Basic";

        /// <summary>
        /// Holds the username portion of the header value.
        /// </summary>
        private string UserName { get; }

        /// <summary>
        /// Holds the password portion of the header value.
        /// </summary>
        private string Password { get; }
    }
}
