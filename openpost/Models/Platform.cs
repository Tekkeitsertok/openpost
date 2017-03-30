using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace openpost.Models
{
    public class Platform
    {
        [Key]
        [MaxLength(22)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProviderApi { get; set; }
        public string ProviderAuthKey { get; set; }
        public string ProviderAuthPassword { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public ICollection<Page> Pages { get; set; }
    }
}
