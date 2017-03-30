namespace SharedUtils.Api.RequestViewModels
{
    /// <summary>
    /// Request sent by platform to register a specific user to OpenPost System.
    /// </summary>
    public class RequestUserTokenViewModel : PlatformRequest
    {
        /// <summary>
        /// ProviderId is OpenPost Author ID generated with ShortGuid (Author.Id)
        /// </summary>
        public string ProviderId { get; set; }
        /// <summary>
        /// UserId provided by platform
        /// </summary>
        public string PlatformId { get; set; }
        /// <summary>
        /// User Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// User Display Name
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// User Avatar Url
        /// </summary>
        public string AvatarUrl { get; set; }
    }
}
