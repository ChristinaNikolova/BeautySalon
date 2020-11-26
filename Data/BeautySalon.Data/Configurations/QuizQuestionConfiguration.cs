namespace BeautySalon.Data.Configurations
{
    using BeautySalon.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
    {
        public void Configure(EntityTypeBuilder<QuizQuestion> quizQuestion)
        {
            quizQuestion
                .HasMany(qq => qq.QuizAnswers)
                .WithOne(qa => qa.QuizQuestion)
                .HasForeignKey(qa => qa.QuizQuestionId);
        }
    }
}
