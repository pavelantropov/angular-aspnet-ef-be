namespace Domain.Entities;

public class Question
{
	public Guid? Guid { get; set; }
	public string Text { get; set; }
	public virtual List<Answer> Answers { get; set; } //= new();
}