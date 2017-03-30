namespace SharedUtils.Api.RequestViewModels
{
    /// <summary>
    /// Sent from Client to OpenPost System in order to Delete a Post.
    /// </summary>
    public class DeleteCommentViewModel : AuthRequest
    {
        /// <summary>
        /// OpenPost PostId to delete.
        /// </summary>
        public string CommentId { get; set; }
    }
}
