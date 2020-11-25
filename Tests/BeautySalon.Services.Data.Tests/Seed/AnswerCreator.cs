namespace BeautySalon.Services.Data.Tests.Seed
{
    using System;

    using BeautySalon.Data.Models;

    public class AnswerCreator
    {
        public static Answer Create(string stylistId, string clientId, string questionId)
        {
            var answer = new Answer()
            {
                Id = Guid.NewGuid().ToString(),
                Title = Guid.NewGuid().ToString(),
                Content = Guid.NewGuid().ToString(),
                StylistId = stylistId,
                ClientId = clientId,
                QuestionId = questionId,
            };

            return answer;
        }
    }
}
