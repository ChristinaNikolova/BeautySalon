namespace BeautySalon.Services.Data.Questions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class QuestionsService : IQuestionsService
    {
        private readonly IRepository<Question> questionsRepository;

        public QuestionsService(IRepository<Question> questionsRepository)
        {
            this.questionsRepository = questionsRepository;
        }

        public async Task CreateAsync(string title, string content, string stylistId, string userId)
        {
            var question = new Question()
            {
                Title = title,
                Content = content,
                StylistId = stylistId,
                ClientId = userId,
                CreatedOn = DateTime.UtcNow,
            };

            await this.questionsRepository.AddAsync(question);
            await this.questionsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllForStylistAsync<T>(string stylistId)
        {
            var questions = await this.questionsRepository
                .All()
                .Where(q => q.StylistId == stylistId && q.AnswerId == null)
                .OrderByDescending(q => q.CreatedOn)
                .To<T>()
                .ToListAsync();

            return questions;
        }
    }
}
