using Api.Models;
using Microsoft.AspNetCore.Mvc;

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
		return Ok(_context.Questions);
	}

	[Route(""), HttpPost]
	public async Task<ActionResult> PostQuestion([FromBody]QuestionDto question, CancellationToken cancellation)
	{
		await _context.Questions.AddAsync(question, cancellation);
		await _context.SaveChangesAsync(cancellation);

		return Ok();
	}

	[Route("chatGpt"), HttpPost]
	public async Task<ActionResult> AskChatGpt([FromBody] QuestionDto question)
	{
		return Ok();
	}
}