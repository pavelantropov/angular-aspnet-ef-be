using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public class QuizContext : DbContext
{
	public QuizContext(DbContextOptions<QuizContext> options) : base(options) { }

	public DbSet<Question> Questions { get; set; }
	public DbSet<Answer> Answers { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<Question>(item =>
		{
			item.HasKey(x => x.Guid);
			item.HasMany(x => x.Answers).WithOne(a => a.Question).OnDelete(DeleteBehavior.Cascade);
		});

		builder.Entity<Answer>(item =>
		{
			item.HasKey(x => x.Guid);
			item.HasOne(x => x.Question).WithMany(q => q.Answers).OnDelete(DeleteBehavior.SetNull);
		});
	}
}