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
    }
}
