namespace BeautySalon.Data.Seeding.Dtos
{
    using System.Collections.Generic;

    public class QuizQuestionAnswerDto
    {
        public string QuestionText { get; set; }

        public ICollection<string> Answers { get; set; }
    }
}
