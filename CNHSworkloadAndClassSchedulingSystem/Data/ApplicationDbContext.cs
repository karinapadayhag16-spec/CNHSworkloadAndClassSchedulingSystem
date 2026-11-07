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

        // DBSets for application entities
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<SchoolYear> SchoolYears { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<TimeSchedule> TimeSchedules { get; set; }
        public DbSet<TimePeriod> TimePeriods { get; set; }
        public DbSet<ClassSectionSchedule> ClassSectionSchedules { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ClassSection> ClassSections { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        //added
        
         
    


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
                     t.TimeScheduleId,
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

            modelBuilder.Entity<Schedule>()
                .HasIndex(s => new
                {
                    s.TeacherID,
                    s.TimePeriodID,
                    s.DayOfWeek
                })
                .IsUnique();

            modelBuilder.Entity<ClassSection>()
                .HasOne(c => c.Adviser)
                .WithMany()
                .HasForeignKey(c => c.AdviserID)
                .OnDelete(DeleteBehavior.NoAction);
            // TimeSchedule - TimePeriod
            modelBuilder.Entity<TimeSchedule>()
                .HasMany(ts => ts.TimePeriods)
                .WithOne(tp => tp.TimeSchedule)
                .HasForeignKey(tp => tp.TimeScheduleId)
                .OnDelete(DeleteBehavior.Cascade);


            // TimeSchedule - Previous Version
            modelBuilder.Entity<TimeSchedule>()
                .HasOne(ts => ts.BasedOnTimeSchedule)
                .WithMany()
                .HasForeignKey(ts => ts.BasedOnTimeScheduleId)
                .OnDelete(DeleteBehavior.Restrict);


            // ClassSectionSchedule - ClassSection
            modelBuilder.Entity<ClassSectionSchedule>()
                .HasOne(css => css.ClassSection)
                .WithMany(cs => cs.ScheduleAssignments)
                .HasForeignKey(css => css.ClassSectionID)
                .OnDelete(DeleteBehavior.Restrict);


            // ClassSectionSchedule - TimeSchedule
            modelBuilder.Entity<ClassSectionSchedule>()
                .HasOne(css => css.TimeSchedule)
                .WithMany(ts => ts.ClassSectionSchedules)
                .HasForeignKey(css => css.TimeScheduleId)
                .OnDelete(DeleteBehavior.Restrict);


            // ClassSectionSchedule - Term
            modelBuilder.Entity<ClassSectionSchedule>()
                .HasOne(css => css.Term)
                .WithMany(t => t.ClassSectionSchedules)
                .HasForeignKey(css => css.TermID)
                .OnDelete(DeleteBehavior.Restrict);


            // Prevent duplicate schedule assignments
            modelBuilder.Entity<ClassSectionSchedule>()
                .HasIndex(css => new
                {
                    css.ClassSectionID,
                    css.TermID
                })
                .IsUnique();
            modelBuilder.Entity<ClassSection>()
                .HasOne(cs => cs.PreviousClassSection)
                .WithMany()
                .HasForeignKey(cs => cs.PreviousClassSectionID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
