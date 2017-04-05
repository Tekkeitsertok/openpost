using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace openpost.Models
{
    public class Author
    {
        [Key]
        [MaxLength(22)]
        public string Id { get; set; }

        [MaxLength(22)]
        public string TokenId { get; set; }

        public string PlatformId { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string AvatarUrl { get; set; }
        //fields below are for anonymous authors.
        public bool IsAnonymous { get; set; }
        public string WebSite { get; set; }

        [MaxLength(22)]
        public string SourcePlatformId { get; set; }
        public virtual Platform SourcePlatform { get; set; }

        public ICollection<Comment> UserComments { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
