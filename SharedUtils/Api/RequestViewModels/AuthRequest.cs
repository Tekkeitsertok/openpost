namespace SharedUtils.Api.RequestViewModels
{
    /// <summary>
    /// User Authentification to OpenPost System.
    /// </summary>
    public class AuthRequest
    {
        /// <summary>
        /// OpenPost UserId
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// OpenPost generated Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Source Platform Id
        /// </summary>
        public string SourcePlatform { get; set; }
        /// <summary>
        /// Use Id/Token by default or Username/Email/Website if set to false.
        /// </summary>
        public bool AuthenticatedMode { get; set; } = true;
        /// <summary>
        /// Facultative Pseudonym (if none, user will be marked as Anonymous)
        /// </summary>
        public string Pseudonym { get; set; }
        /// <summary>
        /// Facultative Email Address (mainly to get notifications if wanted)
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Facultative Field. (redirect to provided link instead of profile, mainly for bloggers, etc.)
        /// </summary>
        public string Website { get; set; }
    }
}
