namespace BeautySalon.Services.Data.Answers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Data.Common.Repositories;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<T>> GetAllForStylistAsync<T>(string stylistId)
        {
            var answerdQuestions = await this.answersRepository
                .All()
                .Where(a => a.StylistId == stylistId)
                .OrderByDescending(a => a.CreatedOn)
                .To<T>()
                .ToListAsync();

            return answerdQuestions;
        }

        public async Task<T> GetAnswerDetailsAsync<T>(string id)
        {
            var answer = await this.answersRepository
                .All()
                .Where(a => a.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return answer;
        }
    }
}
