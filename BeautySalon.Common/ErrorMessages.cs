namespace BeautySalon.Common
{
    public static class ErrorMessages
    {
        public const string InputModel = "The {0} must be at least {2} and at max {1} characters long!";

        public const string InputModelMaxLength = "The {0} must be at max {1} characters long.";

        // User
        public const string DifferentPasswords = "The password and confirmation password do not match!";

        public const string EmailExists = "User with this email already exists!";

        public const string UsernameExists = "User with this username already exists!";

        public const string UserExists = "User with this email and username already exists!";

        public const string UsernameErrorRegex = "Username must containts only latin letters and/or digits!";

        // Procedures
        public const string InvalidSkinTypeName = "Invalid skin type.";

        public const string InvalidCombinationCategoryAndSkinType = "You can choose skin type only when the category name is equal to skin care.";

        // Comment
        public const string InvalidCommentContentLength = "Content must be between 3 and 1000 characters long!";
    }
}
