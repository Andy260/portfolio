using System.ComponentModel.DataAnnotations;

namespace Portfolio.Shared.Models
{
    /// <summary>
    /// Represents a request from a client to be logged in
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Username of the login request
        /// </summary>
        [Required]
        public string? Username { get; set; }
        /// <summary>
        /// Password of the login request
        /// </summary>
        [Required]
        public string? Password { get; set; }
        /// <summary>
        /// Should the user not have to login again until
        /// a long term expiration?
        /// </summary>
        public bool RememberMe { get; set; }
    }
}
