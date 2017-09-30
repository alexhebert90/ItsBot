using System;

namespace ItsBot.TokenManagement
{
    /// <summary>
    /// Helper class used to persist the result of a previous token retrieval in order
    /// to help determine when to fetch a new token.
    /// </summary>
    internal class SuccessfulTokenResult
    {
        /// <summary>
        /// Holds the json object that represents a previous successful token retrieval.
        /// </summary>
        public NewTokenResponse TokenResponse { get; }

        /// <summary>
        /// Holds the timestamp of when the retrieved token actually expires.
        /// </summary>
        public DateTime TokenExpiresAt { get; }

        /// <summary>
        /// Instantiates an immutable instance of a single successful token result.
        /// </summary>
        /// <param name="newTokenResponse"></param>
        public SuccessfulTokenResult(
            NewTokenResponse newTokenResponse) : this(newTokenResponse, null) { }

        /// <summary>
        /// Instantiates an immutable instance of a single successful token result, allowing the user to pass in the time the token was retrieved at.
        /// </summary>
        /// <param name="newTokenResponse"></param>
        /// <param name="tokenRetrievedAt"></param>
        public SuccessfulTokenResult(
            NewTokenResponse newTokenResponse,
            DateTime? tokenRetrievedAt)
        {
            TokenResponse = newTokenResponse ?? throw new ArgumentNullException(nameof(newTokenResponse));
            TokenExpiresAt = (tokenRetrievedAt ?? DateTime.Now) + TimeSpan.FromMinutes(TokenResponse.ExpiresIn);
        }
    }

}