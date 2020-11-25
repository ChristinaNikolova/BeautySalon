namespace BeautySalon.Services.Data.Tests.Seed
{
    using System;

    using BeautySalon.Data.Models;

    public class QuestionCreator
    {
        public static Question Create(string stylistId, string userId)
        {
            var question = new Question()
            {
                Id = Guid.NewGuid().ToString(),
                Title = Guid.NewGuid().ToString(),
                Content = Guid.NewGuid().ToString(),
                StylistId = stylistId,
                ClientId = userId,
            };

            return question;
        }
    }
}
