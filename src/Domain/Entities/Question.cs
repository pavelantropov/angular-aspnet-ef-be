namespace Domain.Entities;

public class Question
{
	public int Id { get; set; }
	public string Text { get; set; }
	public List<string> CorrectAnswers { get; set; }
	public List<string> WrongAnswers { get; set; }
}