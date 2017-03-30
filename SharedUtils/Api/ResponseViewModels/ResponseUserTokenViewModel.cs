namespace SharedUtils.Api.ResponseViewModels
{
    /// <summary>
    /// Response sent to Source after Registering/Updating User with OpenPost System.
    /// Both Id & Token should be given to client in order to add, edit & delete Posts.
    /// </summary>
    public class ResponseUserTokenViewModel
    {
        public enum OperationResult
        {
            Invalid,
            AuthorizationFailed,
            UserCreated,
            UserUpdated
        }

        /// <summary>
        /// Operation Result Code
        /// </summary>
        public OperationResult Retcode { get; set; }
        /// <summary>
        /// OpenPost generated Author Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// OpenPost generated Token for Author
        /// </summary>
        public string Token { get; set; }
    }
}
