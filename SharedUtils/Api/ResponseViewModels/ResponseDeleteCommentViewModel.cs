namespace SharedUtils.Api.ResponseViewModels
{
    /// <summary>
    /// Response sent to client after a DeleteCommentRequest.
    /// </summary>
    public class ResponseDeleteCommentViewModel
    {
        public enum OperationResult
        {
            Invalid,
            AuthorizationFailed,
            CommentDeleted,
        }

        public ResponseDeleteCommentViewModel() { }
        public ResponseDeleteCommentViewModel(OperationResult retcode) { Retcode = retcode; }

        /// <summary>
        /// Operation Result
        /// </summary>
        public OperationResult Retcode { get; set; }
    }
}
