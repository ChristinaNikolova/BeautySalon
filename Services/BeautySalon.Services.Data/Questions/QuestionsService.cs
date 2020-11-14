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
                .Where(q => q.StylistId == stylistId && q.IsAnswered == false)
                .OrderByDescending(q => q.CreatedOn)
                .To<T>()
                .ToListAsync();

            return questions;
        }

        public async Task<int> GetNewQuestionsCountAsync(string stylistId)
        {
            var questionsCount = await this.questionsRepository
                .All()
                .Where(q => q.StylistId == stylistId
                    && q.IsAnswered == false)
                .CountAsync();

            return questionsCount;
        }

        public async Task<T> GetQuestionDetailsAsync<T>(string id)
        {
            var question = await this.questionsRepository
                .All()
                .Where(q => q.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return question;
        }
    }
}
