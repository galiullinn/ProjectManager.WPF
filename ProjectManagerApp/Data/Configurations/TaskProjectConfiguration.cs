using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProjectManagerApp.Model;

namespace ProjectManagerApp.Data.Configurations
{
    internal class TaskProjectConfiguration : IEntityTypeConfiguration<TaskProject>
    {
        public void Configure(EntityTypeBuilder<TaskProject> builder)
        {
            builder.HasKey(t => t.TaskProjectId);

            builder
                .HasOne(t => t.AssignedUser)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.AssignedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
