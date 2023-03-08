using Api.Models;
using AutoMapper;
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
	private readonly IMapper _mapper;

	public QuestionsController(QuizContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	[Route(""), HttpGet]
	public async Task<ActionResult> GetQuestions(CancellationToken cancellation)
	{
		var questions = _context.Questions.Include(x => x.Answers);
		return Ok(_mapper.Map<List<QuestionDto>>(questions));
	}

	[Route("{guid:guid}"), HttpGet]
	public async Task<ActionResult> GetQuestion(Guid guid, CancellationToken cancellation)
	{
		var question = await _context.Questions
									 .Include(x => x.Answers)
									 .FirstOrDefaultAsync(q => q.Guid == guid, cancellation);

		return question == null ? NotFound() : Ok(_mapper.Map<QuestionDto>(question));
	}

	[Route(""), HttpPost]
	public async Task<ActionResult> PostQuestion([FromBody]QuestionDto question, CancellationToken cancellation)
	{
		await _context.Questions.AddAsync(_mapper.Map<Question>(question), cancellation);
		await _context.SaveChangesAsync(cancellation);

		return Ok();
	}

	[Route("{guid:guid}"), HttpPut]
	public async Task<ActionResult> UpdateQuestion(Guid guid, [FromBody] QuestionDto questionModel, CancellationToken cancellation)
	{
		var question = await _context.Questions
									 .Include(x => x.Answers)
									 .FirstOrDefaultAsync(q => q.Guid == guid, cancellation);

		if (question == null)
		{
			return NotFound();
		}

		_mapper.Map(questionModel, question);

		await _context.SaveChangesAsync(cancellation);

		return Ok();
	}

	[Route("chatGpt"), HttpPost]
	public async Task<ActionResult> AskChatGpt([FromBody] QuestionDto question)
	{
		return Ok();
	}
}