using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace openpost.Models
{
    public class Page
    {
        [MaxLength(22)]
        public string Id { get; set; }

        public string PublicIdentifier { get; set; }

        [MaxLength(22)]
        public string SourcePlatformId { get; set; }
        public virtual Platform SourcePlatform { get; set; }

        public int CommentsCount { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
