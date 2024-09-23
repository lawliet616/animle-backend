using Animle;
using Animle.Models;
using Microsoft.EntityFrameworkCore;
using NHibernate.Linq;

public class AnimleDbContext : DbContext
{
    public AnimleDbContext(DbContextOptions<AnimleDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnimeWithEmoji>()
            .HasMany(a => a.Threebythree)
            .WithMany(t => t.Animes)
             .UsingEntity(j => j.ToTable("AnimeThreebythree"));

        modelBuilder.Entity<Threebythree>()
        .HasOne(t => t.User)
        .WithOne(u => u.Threebythree)
        .HasForeignKey<Threebythree>(t => t.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Quiz>()
       .HasOne(t => t.user)
       .WithMany(u => u.Quizzes)
       .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<QuizLikes>()
           .HasKey(l => new { l.UserId, l.quizId });

        modelBuilder.Entity<QuizLikes>()
            .HasOne(l => l.User)
            .WithMany(u => u.QuizLikes)
            .HasForeignKey(l => l.UserId);

        modelBuilder.Entity<QuizLikes>()
            .HasOne(l => l.LikedQuiz)
            .WithMany(c => c.Likes)
            .HasForeignKey(l => l.quizId);


        modelBuilder.Entity<ThreebythreeLike>()
            .HasOne(l => l.User)
            .WithMany(u => u.ThreebythreeLikes)
            .HasForeignKey(l => l.UserId);

        modelBuilder.Entity<ThreebythreeLike>()
            .HasOne(l => l.LikedThreebyThree)
            .WithMany(c => c.ThreebythreeLikes)
            .HasForeignKey(l => l.ThreebythreeId);

        modelBuilder.Entity<GameContest>()
         .HasOne(t => t.User)
         .WithMany(u => u.GameContests)
         .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Versus>()
         .HasOne(t => t.User)
          .WithMany(u => u.VersusRecords)
         .OnDelete(DeleteBehavior.Cascade);


    }

    public DbSet<GameContest> GameContests { get; set; }
    public DbSet<Versus> Versus { get; set; }
    public DbSet<QuizLikes> QuizLikes { get; set; }
    public DbSet<ThreebythreeLike> threebythreeLikes { get; set; }
    public DbSet<Threebythree> Threebythree { get; set; }
    public DbSet<Quiz> Quizes { get; set; }
    public DbSet<AnimeWithEmoji> AnimeWithEmoji { get; set; }
    public DbSet<User> Users { get; set; }

}