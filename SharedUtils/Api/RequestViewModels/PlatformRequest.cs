namespace SharedUtils.Api.RequestViewModels
{
    /// <summary>
    /// Platform Authentification sent to OpenPost System.
    /// </summary>
    public class PlatformRequest
    {
        /// <summary>
        /// Source Platform registered
        /// </summary>
        public string SourcePlatform { get; set; }
        /// <summary>
        /// AuthKey for Distant Platform to interact with us
        /// </summary>
        public string PlatformAuthKey { get; set; }
        /// <summary>
        /// AuthPassword for Distant Platform to interact with us
        /// </summary>
        public string PlatformAuthPassword { get; set; }
    }
}
