using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using openpost.Data;

namespace openpost.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170401161021_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("openpost.Models.Author", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(22);

                    b.Property<string>("AvatarUrl");

                    b.Property<string>("DisplayName");

                    b.Property<string>("Email");

                    b.Property<string>("PlatformId");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("SourcePlatformId")
                        .HasMaxLength(22);

                    b.Property<string>("TokenId")
                        .HasMaxLength(22);

                    b.HasKey("Id");

                    b.HasIndex("SourcePlatformId");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("openpost.Models.Comment", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(22);

                    b.Property<string>("AuthorId")
                        .HasMaxLength(22);

                    b.Property<string>("Content");

                    b.Property<byte>("Depth");

                    b.Property<string>("PageId")
                        .HasMaxLength(22);

                    b.Property<string>("ParentId")
                        .HasMaxLength(22);

                    b.Property<DateTime>("PostDate");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("PageId");

                    b.HasIndex("ParentId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("openpost.Models.Page", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(22);

                    b.Property<int>("CommentsCount");

                    b.Property<string>("PublicIdentifier")
                        .IsRequired();

                    b.Property<string>("SourcePlatformId")
                        .IsRequired()
                        .HasMaxLength(22);

                    b.HasKey("Id");

                    b.HasIndex("SourcePlatformId");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("openpost.Models.Platform", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(22);

                    b.Property<string>("Name");

                    b.Property<string>("ProviderApi");

                    b.Property<string>("ProviderAuthKey");

                    b.Property<string>("ProviderAuthPassword");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Platforms");
                });

            modelBuilder.Entity("openpost.Models.Author", b =>
                {
                    b.HasOne("openpost.Models.Platform", "SourcePlatform")
                        .WithMany()
                        .HasForeignKey("SourcePlatformId");
                });

            modelBuilder.Entity("openpost.Models.Comment", b =>
                {
                    b.HasOne("openpost.Models.Author", "Author")
                        .WithMany("UserComments")
                        .HasForeignKey("AuthorId");

                    b.HasOne("openpost.Models.Page", "Page")
                        .WithMany("Comments")
                        .HasForeignKey("PageId");

                    b.HasOne("openpost.Models.Comment", "Parent")
                        .WithMany("Childrens")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("openpost.Models.Page", b =>
                {
                    b.HasOne("openpost.Models.Platform", "SourcePlatform")
                        .WithMany("Pages")
                        .HasForeignKey("SourcePlatformId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
