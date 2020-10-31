namespace BeautySalon.Services.Data.Questions
{
    using System;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;

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
    }
}
