namespace BeautySalon.Common
{
    public static class DataValidation
    {
        // Answer
        public const int AnswerTitleMinLenght = 3;

        public const int AnswerTitleMaxLenght = 50;

        public const int AnswerContentMinLenght = 5;

        public const int AnswerContentMaxLenght = 500;

        // AppUser
        public const int UserFirstNameMinLenght = 2;

        public const int UserFirstNameMaxLenght = 50;

        public const int UserLastNameMinLenght = 2;

        public const int UserLastNameMaxLenght = 50;

        public const int UsernameMinLenght = 3;

        public const int UsernameMaxLenght = 50;

        public const string UsernameAllowedSymbols = @"^[a-zA-Z0-9.]+$";

        public const int PasswordMinLenght = 6;

        public const int PasswordMaxLenght = 50;

        public const int AddressMinLenght = 6;

        public const int AddressMaxLenght = 50;

        public const int StylistDescriptionMinLenght = 10;

        public const int StylistDescriptionMaxLenght = 1000;

        // Appointment
        public const int AppointmentMaxLenght = 500;

        // Article
        public const int ArticleTitleMinLenght = 5;

        public const int ArticleTitleMaxLenght = 100;

        public const int ArticleContentMinLenght = 10;

        public const int ArticleContentMaxLenght = 6000;

        // Brand
        public const int BrandNameMinLenght = 2;

        public const int BrandNameMaxLenght = 50;

        public const int BrandDescriptionMinLenght = 6;

        public const int BrandDescriptionMaxLenght = 1000;

        // Category
        public const int CategoryNameMaxLenght = 50;

        // Comment
        public const int CommentContentMinLenght = 3;

        public const int CommentContentMaxLenght = 1000;

        // JobType
        public const int JobTypeNameMaxLenght = 50;

        // Order
        public const int OrderCommentMaxLenght = 1000;

        // Procedure
        public const int ProcedureNameMinLenght = 5;

        public const int ProcedureNameMaxLenght = 100;

        public const int ProcedureDescriptionMinLenght = 10;

        public const int ProcedureDescriptionMaxLenght = 1000;

        public const string ProcedureMinPrice = "10";

        public const string ProcedureMaxPrice = "1000";

        // ProcedureReview
        public const int ProcedureReviewContentMaxLenght = 1000;

        // Product
        public const int ProductNameMinLenght = 3;

        public const int ProductNameMaxLenght = 60;

        public const int ProductDescriptionMinLenght = 10;

        public const int ProductDescriptionMaxLenght = 1000;

        public const string ProductMinPrice = "10";

        public const string ProductMaxPrice = "1000";

        // ProductReview
        public const int ProductReviewContentMaxLenght = 1000;

        // Question
        public const int QuestionTitleMinLenght = 3;
        public const int QuestionTitleMaxLenght = 50;

        public const int QuestionContentMinLenght = 5;
        public const int QuestionContentMaxLenght = 1000;

        // SkinProblem
        public const int SkinProblemNameMaxLenght = 100;

        public const int SkinProblemDescriptionMaxLenght = 1000;

        // SkinType
        public const int SkinTypeNameMaxLenght = 100;

        public const int SkinTypeDescriptionMaxLenght = 1000;

        // StylistReview
        public const int StylistReviewContentMaxLenght = 1000;

        // Verification Code
        public const int VerificationCodeMinLenght = 6;

        public const int VerificationCodeMaxLenght = 7;
    }
}
