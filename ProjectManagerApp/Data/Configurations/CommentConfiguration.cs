using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectManagerApp.Model;

namespace ProjectManagerApp.Data.Configurations
{
    internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(p => p.CommentId);

            builder
                .HasOne(r => r.TaskProject)
                .WithMany(t => t.Comments)
                .HasForeignKey(t => t.TaskProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(t => t.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
