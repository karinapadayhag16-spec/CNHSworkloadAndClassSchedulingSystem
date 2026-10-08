using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CNHSworkloadAndClassSchedulingSystem.Models.Domain;

namespace CNHSworkloadAndClassSchedulingSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Add DBSets for your application entities
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SchoolYear>()
                .HasIndex(s => s.YearLabel)
                .IsUnique();

            modelBuilder.Entity<Term>()
                .HasIndex(t => new
                {
                    t.SchoolYearID,
                    t.TermNumber
                })
                .IsUnique();

            modelBuilder.Entity<TimePeriod>()
                .HasIndex(t => new
                {
                    t.TermID,
                    t.Department,
                    t.PeriodNumber
                })
                .IsUnique();

            modelBuilder.Entity<TeacherExpertise>()
                .HasOne(te => te.Teacher)
                .WithMany(t => t.SubjectExpertise)
                .HasForeignKey(te => te.TeacherID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeacherExpertise>()
                .HasOne(te => te.Subject)
                .WithMany(s => s.TeacherExpertises)
                .HasForeignKey(te => te.SubjectID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeacherExpertise>()
                .HasIndex(te => new
                {
                    te.TeacherID,
                    te.SubjectID
                })
                .IsUnique();

            modelBuilder.Entity<SubjectOffering>()
                .HasOne(so => so.Subject)
                .WithMany(s => s.SubjectOfferings)
                .HasForeignKey(so => so.SubjectID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubjectOffering>()
                .HasOne(so => so.ClassSection)
                .WithMany()
                .HasForeignKey(so => so.ClassSectionID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubjectOffering>()
                .HasOne(so => so.Term)
                .WithMany()
                .HasForeignKey(so => so.TermID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubjectOffering>()
                .HasIndex(so => new
                {
                    so.SubjectID,
                    so.ClassSectionID,
                    so.TermID
                })
                .IsUnique();

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.SubjectOffering)
                .WithMany()
                .HasForeignKey(s => s.SubjectOfferingID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Teacher)
                .WithMany()
                .HasForeignKey(s => s.TeacherID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.TimePeriod)
                .WithMany()
                .HasForeignKey(s => s.TimePeriodID)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent teacher schedule conflicts
            modelBuilder.Entity<Schedule>()
                .HasIndex(s => new
                {
                    s.TeacherID,
                    s.TimePeriodID,
                    s.DayOfWeek
                })
                .IsUnique();
        }
    }
}
