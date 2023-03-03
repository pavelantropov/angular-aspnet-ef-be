using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public class QuizContext : DbContext
{
	public QuizContext(DbContextOptions<QuizContext> options) : base(options) { }

	public DbSet<Question> Questions { get; set; }
	//public DbSet<Answer> Answers { get; set; }
}