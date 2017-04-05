namespace SharedUtils.Api.ResponseViewModels
{
    /// <summary>
    /// Response sent to Source after Updating a Page in OpenPost System.
    /// </summary>
    public class ResponseUpdatePageViewModel
    {
        public enum OperationResult
        {
            Invalid,
            AuthorizationFailed,
            PageUpdated
        }

        /// <summary>
        /// Operation Result Code
        /// </summary>
        public OperationResult Retcode { get; set; }
    }
}
