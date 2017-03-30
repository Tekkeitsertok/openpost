namespace SharedUtils.Api.RequestViewModels
{
    /// <summary>
    /// Sent from Client to OpenPost System in order to post or edit a Post.
    /// </summary>
    public class PostCommentViewModel : AuthRequest
    {
        /// <summary>
        /// Unique page identifier, all comments with the same PageIdentifier and Platform will be grouped.
        /// </summary>
        public string PageIdentifier { get; set; }
        /// <summary>
        /// OpenPost PostId if we are currently editing previous post. (null for new post)
        /// </summary>
        public string Current { get; set; }
        /// <summary>
        /// OpenPost PostId if we are responding to a post. (null if no parent)
        /// </summary>
        public string Parent { get; set; }
        /// <summary>
        /// Post title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Post content
        /// </summary>
        public string Content { get; set; }
    }
}
