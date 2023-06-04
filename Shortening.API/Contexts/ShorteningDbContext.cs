using Microsoft.EntityFrameworkCore;
using Shortening.API.Constants;
using Shortening.API.Entities;

namespace Shortening.API.Contexts
{
    public class ShorteningDbContext : DbContext
    {
        public virtual DbSet<UrlShorteningEntity> UrlShortenings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DatabaseConstants.MSSQL_CONNECTION_STRING);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<UrlShorteningEntity>(entity =>
            {
                entity.ToTable("url_shortenings");

                entity.Property(i => i.Id).HasColumnName("id").HasColumnType("int").UseIdentityColumn().IsRequired();
                entity.Property(i => i.OriginalUrl).HasColumnName("original_url").HasColumnType("nvarchar(MAX)");
                entity.Property(i => i.ShortenedUrl).HasColumnName("shortened_url").HasColumnType("nvarchar").HasMaxLength(256);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
