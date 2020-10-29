namespace BeautySalon.Services.Data.Questions
{
    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;

    public class QuestionsService : IQuestionsService
    {
        private readonly IRepository<Question> questionsRepository;
        public QuestionsService(IRepository<Question> questionsRepository)
        {
            this.questionsRepository = questionsRepository;
        }
    }
}
