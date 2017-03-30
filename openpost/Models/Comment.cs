using SharedUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace openpost.Models
{
    public class Comment
    {
        /// <summary>
        /// Comment unique Guid representated with 22 characters
        /// </summary>
        [Key]
        [MaxLength(22)]
        public string Id { get; set; }

        /// <summary>
        /// Can be a reply to one comment
        /// </summary>
        [MaxLength(22)]
        public string ParentId { get; set; }
        public virtual Comment Parent { get; set; }

        /// <summary>
        /// From 0 to 3 maximum.
        /// </summary>
        public byte Depth { get; set; }

        /// <summary>
        /// Can contains 0, 1 to n children.
        /// </summary>
        public ICollection<Comment> Childrens { get; set; }

        /// <summary>
        /// Comment issued by
        /// </summary>
        [MaxLength(22)]
        public string AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public DateTime PostDate { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }

        /// <summary>
        /// Comment on Page
        /// </summary>
        [MaxLength(22)]
        public string PageId { get; set; }
        public virtual Page Page { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Comment()
        {
            //Set Default Key
            Id = ShortGuid.NewGuid().Value;
        }
    }
}
