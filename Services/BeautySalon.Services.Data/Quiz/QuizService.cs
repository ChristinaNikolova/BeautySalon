﻿namespace BeautySalon.Services.Data.Quiz
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class QuizService : IQuizService
    {
        private readonly IRepository<QuizQuestion> quizQuestionRepository;

        public QuizService(IRepository<QuizQuestion> quizQuestionRepository)
        {
            this.quizQuestionRepository = quizQuestionRepository;
        }

        public async Task<IEnumerable<T>> GetQuizAsync<T>()
        {
            var quiz = await this.quizQuestionRepository
                .All()
                .To<T>()
                .ToListAsync();

            return quiz;
        }
    }
}
