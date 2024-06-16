
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace asg_form.Controllers
{
   public class Video
    {
        public int id { get; set; }
        public string Video_name { get; set; }
        public string Video_msg { get; set; }
        public string Video_url { get; set; }

    }

    class videoeConfig : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.ToTable("T_video");
            builder.Property(e => e.id).IsRequired();
            builder.Property(e => e.Video_name).IsRequired();
            builder.Property(e => e.Video_msg).IsRequired();
            builder.Property(e => e.Video_url).IsRequired();



        }
    }





    class TestDbContext : DbContext
    {

              public DbSet<Video> video { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr = @"Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;TrustServerCertificate=true";
            optionsBuilder.UseSqlServer(connStr);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }

    }



}
