using System;

namespace openpost.ViewModels
{
    public class CommentViewModel
    {
        public string Id { get; set; }
        public int Depth { get; set; }
        public string AuthorId { get; set; }
        public string Author { get; set; }
        public bool HasAvatar { get { return !string.IsNullOrEmpty(AvatarUrl); } }
        public string AvatarUrl { get; set; }
        public string ProfileUrl { get; set; }
        public DateTime PostDate { get; set; }
        public bool IsDeleted { get { return Content == null; } }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
