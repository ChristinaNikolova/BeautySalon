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
        private readonly IRepository<Question> questionsRepository;

        public AnswersService(
            IRepository<Answer> answersRepository,
            IRepository<Question> questionsRepository)
        {
            this.answersRepository = answersRepository;
            this.questionsRepository = questionsRepository;
        }

        public async Task ChangeIsRedAsync(string id)
        {
            var answer = await this.answersRepository
                .All()
                .FirstOrDefaultAsync(a => a.Id == id);

            answer.IsRed = true;

            this.answersRepository.Update(answer);
            await this.answersRepository.SaveChangesAsync();
        }

        public async Task<bool> CheckNewAnswerAsync(string userId)
        {
            bool isNewAnswer = await this.answersRepository
                .All()
                .AnyAsync(a => a.ClientId == userId && a.IsRed == false);

            return isNewAnswer;
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

            await this.ChangeQuestionIsAnseredPropertyAsync(questionId);

            await this.answersRepository.AddAsync(answer);
            await this.answersRepository.SaveChangesAsync();

            return answer.Id;
        }

        public async Task<IEnumerable<T>> GetAllAnswersForUserAsync<T>(string userId)
        {
            var answerdQuestions = await this.answersRepository
                .All()
                .Where(a => a.ClientId == userId)
                .OrderByDescending(a => a.CreatedOn)
                .To<T>()
                .ToListAsync();

            return answerdQuestions;
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

        public async Task<IEnumerable<T>> GetAllNewAnswersForUserAsync<T>(string userId)
        {
            var answers = await this.answersRepository
                .All()
                .Where(a => a.ClientId == userId && a.IsRed == false)
                .OrderByDescending(a => a.CreatedOn)
                .To<T>()
                .ToListAsync();

            return answers;
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

        private async Task ChangeQuestionIsAnseredPropertyAsync(string questionId)
        {
            var question = await this.questionsRepository
                .All()
                .FirstOrDefaultAsync(q => q.Id == questionId);

            question.IsAnswered = true;
        }
    }
}
