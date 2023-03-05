using Api.Models;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestionsController : ControllerBase
{
	private readonly QuizContext _context;

	public QuestionsController(QuizContext context)
	{
		_context = context;
	}

	[Route(""), HttpGet]
	public async Task<ActionResult> GetQuestions()
	{
		// TODO mapping to dto
		return Ok(_context.Questions.Include(x => x.Answers));
	}

	[Route("{guid:guid}"), HttpGet]
	public async Task<ActionResult> GetQuestion(Guid guid)
	{
		var question = await _context.Questions
									 .Include(x => x.Answers)
									 .FirstOrDefaultAsync(q => q.Guid == guid);

		if (question == null)
		{
			return NotFound();
		}

		var questionDto = new QuestionDto
		{
			Text = question.Text,
			CorrectAnswers = question.Answers.Where(x => x.IsCorrect).Select(a => a.Text).ToList(),
			WrongAnswers = question.Answers.Where(x => !x.IsCorrect).Select(a => a.Text).ToList(),
		};

		return Ok(questionDto);
	}

	[Route(""), HttpPost]
	public async Task<ActionResult> PostQuestion([FromBody]QuestionDto question, CancellationToken cancellation)
	{
		var answers = new List<Answer>();

		answers.AddRange(question.CorrectAnswers.Select(
			answer => new Answer { Text = answer, IsCorrect = true }));

		answers.AddRange(question.WrongAnswers.Select(
			answer => new Answer { Text = answer, IsCorrect = false }));

		var newQuestion = new Question
		{
			Text = question.Text,
			Answers = answers
		};

		await _context.Questions.AddAsync(newQuestion, cancellation);
		await _context.SaveChangesAsync(cancellation);

		return Ok();
	}

	[Route("chatGpt"), HttpPost]
	public async Task<ActionResult> AskChatGpt([FromBody] QuestionDto question)
	{
		return Ok();
	}
}