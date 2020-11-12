namespace BeautySalon.Services.Data.Answers
{
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;

    public class AnswersService : IAnswersService
    {
        private readonly IRepository<Answer> answersRepository;

        public AnswersService(IRepository<Answer> answersRepository)
        {
            this.answersRepository = answersRepository;
        }

        public async Task<string> CreateAsync(string title, string content, string questionStylistId, string questionClientId, string questionId)
        {
            var answer = new Answer()
            {
                Title = title,
                Content = content,
                StylistId = questionStylistId,
                ClientId = questionClientId,
                QuestionId = questionId,
            };

            await this.answersRepository.AddAsync(answer);
            await this.answersRepository.SaveChangesAsync();

            return answer.Id;
        }
    }
}
