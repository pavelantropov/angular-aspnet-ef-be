using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api;

public class QuizContext : DbContext
{
	public QuizContext(DbContextOptions<QuizContext> options) : base(options) { }

	public DbSet<QuestionDto> Questions { get; set; }
}