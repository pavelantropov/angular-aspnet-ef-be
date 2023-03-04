using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Answer
{
	public Guid? Guid { get; set; }
	public string Text { get; set; }
	public bool? IsCorrect { get; set; }
	
	[JsonIgnore]
	public virtual Question Question { get; set; }
}