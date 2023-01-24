using BackendApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestionsController : ControllerBase
{
	[HttpPost]
	public async Task<ActionResult> PostQuestion([FromBody]QuestionDto question)
	{
		return Ok();
	}
}