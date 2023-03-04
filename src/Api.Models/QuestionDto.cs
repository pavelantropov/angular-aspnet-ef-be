namespace Api.Models;

public class QuestionDto
{
    public string Text { get; set; }
	public List<string> CorrectAnswers { get; set; }
	public List<string> WrongAnswers { get; set; }
}