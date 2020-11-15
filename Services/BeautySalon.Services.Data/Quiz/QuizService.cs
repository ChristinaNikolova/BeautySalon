namespace BeautySalon.Services.Data.Quiz
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Data.SkinTypes;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class QuizService : IQuizService
    {
        private readonly IRepository<QuizQuestion> quizQuestionRepository;
        private readonly ISkinTypesService skinTypesService;

        public QuizService(IRepository<QuizQuestion> quizQuestionRepository, ISkinTypesService skinTypesService)
        {
            this.quizQuestionRepository = quizQuestionRepository;
            this.skinTypesService = skinTypesService;
        }

        public async Task<IEnumerable<T>> GetQuizAsync<T>()
        {
            var quiz = await this.quizQuestionRepository
                .All()
                .OrderByDescending(q => q.QuizAnswers.Count)
                .To<T>()
                .ToListAsync();

            return quiz;
        }
    }
}
