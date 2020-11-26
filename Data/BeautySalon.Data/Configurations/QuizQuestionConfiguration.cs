namespace BeautySalon.Data.Configurations
{
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class QuizQuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> quizQuestion)
        {
            quizQuestion
                .HasMany(qq => qq.QuizAnswers)
                .WithOne(qa => qa.QuizQuestion)
                .HasForeignKey(qa => qa.QuizQuestionId);
        }
    }
}
