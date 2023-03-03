﻿namespace Domain.Entities;

public class Question
{
	public int? Id { get; set; }
	public string Text { get; set; }
	public List<Answer> Answers { get; set; } = new();
}