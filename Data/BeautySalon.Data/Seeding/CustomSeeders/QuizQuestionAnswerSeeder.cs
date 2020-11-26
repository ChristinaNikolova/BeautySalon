namespace BeautySalon.Data.Seeding.CustomSeeders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Data.Seeding.Dtos;
    using Microsoft.EntityFrameworkCore.Internal;
    using Newtonsoft.Json;

    public class QuizQuestionAnswerSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.QuizQuestions.Any())
            {
                var quizQuestionData = JsonConvert
                    .DeserializeObject<List<QuizQuestionAnswerDto>>(File.ReadAllText(GlobalConstants.QuizQuestionAnswerSeederPath))
                    .ToList();

                List<Question> quizQuestions = new List<Question>();
                List<QuizAnswer> quizAnswers = new List<QuizAnswer>();

                foreach (var currentQuizQuestion in quizQuestionData)
                {
                    var quizQuestion = new Question()
                    {
                        Text = currentQuizQuestion.QuestionText,
                    };

                    quizQuestions.Add(quizQuestion);

                    foreach (var currentQuizAnswer in currentQuizQuestion.Answers)
                    {
                        var quizAnswer = new QuizAnswer()
                        {
                            Text = currentQuizAnswer,
                            QuizQuestionId = quizQuestion.Id,
                        };

                        quizAnswers.Add(quizAnswer);
                    }
                }

                await dbContext.QuizQuestions.AddRangeAsync(quizQuestions);
                await dbContext.QuizAnswers.AddRangeAsync(quizAnswers);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
