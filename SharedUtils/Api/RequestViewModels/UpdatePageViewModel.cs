namespace SharedUtils.Api.RequestViewModels
{
    public class UpdatePageViewModel : PlatformRequest
    {
        /// <summary>
        /// Page Identifier to update
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Allow Anonymous Comments or Only from Authenticated Users
        /// </summary>
        public bool AllowAnonymousComments { get; set; }
    }
}
