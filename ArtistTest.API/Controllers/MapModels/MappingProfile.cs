using AutoMapper;
using SharedData;
using SharedData.TestModel;

namespace ArtistTest.API.Controllers.MapModels
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TestView, Test>();
            CreateMap<Test, TestView>();
            CreateMap<QuestionView, Question>();
            CreateMap<Question, QuestionView>();
            CreateMap<AnswerView, Answer>();
            CreateMap<Answer, AnswerView>();
            CreateMap<TestResult, TestResultView>();
            CreateMap<TestResult, TestResultViewAnswers>();
            CreateMap<TestResultViewAnswers, TestResult>();
            CreateMap<AnswerResultView, AnswerResult>();
            CreateMap<AnswerResult, AnswerResultView>();
            CreateMap<ApplicationUser, UserView>();
            CreateMap<UserView, ApplicationUser>();
            CreateMap<ArtTagView, ArtTag>();
            CreateMap<ArtTag, ArtTagView>();
        }
    }
}