using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizMasterPro.Models;
using QuizMasterPro.Models.Quiz;

namespace QuizMasterPro.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Quiz> Questions { get; set; }
        public DbSet<Quiz> Answers { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);


            builder.Entity<Quiz>()
                .ToTable("Quizes")
                .HasMany(_ => _.Questions);

            builder.Entity<Question>()
                .ToTable("Questions")
                .HasMany(_ => _.Answers);

            builder.Entity<Answer>()
                .ToTable("Answers");

            builder.Entity<Result>()
                .ToTable("Results")
                .HasKey(x => new { x.QuizId, x.QuestionId, x.AnswerId, x.UserId });
        }
    }
}
