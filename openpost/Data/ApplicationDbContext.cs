using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using openpost.Models;

namespace openpost.Data
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<Page> Pages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(p => p.RowVersion).IsConcurrencyToken();

                entity.HasMany(e => e.Childrens).WithOne(e => e.Parent).HasForeignKey(e => e.ParentId);

                entity.HasOne(e => e.Page).WithMany(e => e.Comments).HasForeignKey(e => e.PageId);
            });

            builder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(p => p.RowVersion).IsConcurrencyToken();

                entity.HasMany(e => e.UserComments).WithOne(e => e.Author).HasForeignKey(e => e.AuthorId);

                entity.HasOne(e => e.SourcePlatform).WithMany().HasForeignKey(e => e.SourcePlatformId);
            });

            builder.Entity<Platform>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(p => p.RowVersion).IsConcurrencyToken();
            });

            builder.Entity<Page>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.PublicIdentifier).IsRequired();

                entity.Property(e => e.SourcePlatformId).IsRequired();

                entity.HasOne(e => e.SourcePlatform).WithMany(e => e.Pages).HasForeignKey(e => e.SourcePlatformId);

            });
        }
    }
}
