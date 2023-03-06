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
		// TODO mapping to dto
		return Ok(_context.Questions.Include(x => x.Answers));
	}

	[Route("{guid:guid}"), HttpGet]
	public async Task<ActionResult> GetQuestion(Guid guid, CancellationToken cancellation)
	{
		var question = await _context.Questions
									 .Include(x => x.Answers)
									 .FirstOrDefaultAsync(q => q.Guid == guid, cancellation);

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

	[Route("{guid:guid}"), HttpPut]
	public async Task<ActionResult> UpdateQuestion(Guid guid, [FromBody] QuestionDto questionModel, CancellationToken cancellation)
	{
		//_context.Entry(_mapper questionData).State = EntityState.Modified;
		//_mapper.Map<Question>(questionData);

		var question = await _context.Questions
									 .Include(x => x.Answers)
									 .FirstOrDefaultAsync(q => q.Guid == guid, cancellation);

		if (question == null)
		{
			return NotFound();
		}

		question.Text = questionModel.Text;

		question.Answers.Clear();
		question.Answers.AddRange(questionModel.CorrectAnswers.Select(answer 
			=> new Answer { Text = answer, IsCorrect = true }));
		question.Answers.AddRange(questionModel.WrongAnswers.Select(answer 
			=> new Answer { Text = answer, IsCorrect = false }));

		await _context.SaveChangesAsync(cancellation);

		return Ok();
	}

	[Route("chatGpt"), HttpPost]
	public async Task<ActionResult> AskChatGpt([FromBody] QuestionDto question)
	{
		return Ok();
	}
}