using Api.Models;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Maps;

public class QuestionMapProfile : Profile
{
	public QuestionMapProfile()
	{
		RegisterMappings();
	}

	private void RegisterMappings()
	{
		CreateMap<QuestionDto, Question>()
			.ForMember(dest => dest.Answers, opt 
				=> opt.MapFrom(src => QuestionResolver.UniteAnswers(src.CorrectAnswers, src.WrongAnswers)));
		CreateMap<Question, QuestionDto>()
			.ForMember(dest 
				=> dest.CorrectAnswers, opt => opt.MapFrom(src => QuestionResolver.GetCorrectAnswers(src.Answers)))
			.ForMember(dest 
				=> dest.WrongAnswers, opt => opt.MapFrom(src => QuestionResolver.GetWrongAnswers(src.Answers)));
	}

	public class QuestionResolver
	{
		public QuestionResolver()
		{
		}

		public static List<Answer> UniteAnswers(List<string> correctAnswers, List<string> wrongAnswers)
		{
			var answers = new List<Answer>();

			answers.AddRange(correctAnswers.Select(
				answer => new Answer { Text = answer, IsCorrect = true }));

			answers.AddRange(wrongAnswers.Select(
				answer => new Answer { Text = answer, IsCorrect = false }));

			return answers;
		}

		public static List<string> GetCorrectAnswers(List<Answer> answers)
		{
			return answers.Where(x => x.IsCorrect).Select(a => a.Text).ToList();
		}

		public static List<string> GetWrongAnswers(List<Answer> answers)
		{
			return answers.Where(x => !x.IsCorrect).Select(a => a.Text).ToList();
		}
	}
}