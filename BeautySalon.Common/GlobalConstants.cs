namespace BeautySalon.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "BeautySalon";

        public const string AdministratorRoleName = "Administrator";

        public const string StylistRoleName = "Stylist";

        public const string AdminArea = "Administration";

        public const string AdminEmail = "admin@admin.com";

        public const string AdminName = "Admin";

        public const string AdminPicture = "https://res.cloudinary.com/dieu4mste/image/upload/v1600920611/BeautySalonLogo_ulueqn.png";

        public const string StylistsArea = "Stylists";

        public const string StylistEmail = "@stylist.com";

        public const string BeautySalonEmail = "softuni-beautysalon@abv.bg";

        public const string SystemPasswordHashed = "AQAAAAEAACcQAAAAECrjCD23cQQ28Tyci+UMuaGrFMDUb/trG4E0RbJa4McRVfWFJ6c5UG4NpbXDB6K5rQ==";

        public const string DefaultUserProfilePicture = "https://res.cloudinary.com/dieu4mste/image/upload/v1601044226/v263-peera-ning-39-beauty_2.jpg_gfenuo.jpg";

        public const string BrandSeederPath = @"../../Data/BeautySalon.Data/Seeding/Data/Brands.json";

        public const string StylistSeederPath = @"../../Data/BeautySalon.Data/Seeding/Data/Stylists.json";

        public const string CategorySeederPath = @"../../Data/BeautySalon.Data/Seeding/Data/Categories.json";

        public const string JobTypeSeederPath = @"../../Data/BeautySalon.Data/Seeding/Data/JobTypes.json";

        public const string ProcedureSeederPath = @"../../Data/BeautySalon.Data/Seeding/Data/Procedures.json";

        public const string ProductSeederPath = @"../../Data/BeautySalon.Data/Seeding/Data/Products.json";

        public const string SkinProblemSeederPath = @"../../Data/BeautySalon.Data/Seeding/Data/SkinProblems.json";

        public const string SkinTypeSeederPath = @"../../Data/BeautySalon.Data/Seeding/Data/SkinTypes.json";

        public const string QuizQuestionAnswerSeederPath = @"../../Data/BeautySalon.Data/Seeding/Data/Quiz.json";

        public const string ArticleSeederPath = @"../../Data/BeautySalon.Data/Seeding/Data/Articles.json";

        public const string TypeCardSeederPath = @"../../Data/BeautySalon.Data/Seeding/Data/TypeCards.json";

        public const string CategorySkinName = "Skin Care";

        public const string CategoryNailsName = "Nails";

        public const string ProcedureHairCutName = "Haircut";

        public const string NameCriteria = "name";

        public const string PriceCriteria = "price";

        public const string RatingCriteria = "rating";

        public const int ProceduresPerPage = 12;

        public const int ArticlesPerPage = 6;

        public const int RecentArticlesCount = 5;

        public const string CategorySkinCareId = "c17ea54d-c0b7-49f2-9c05-8edadc59357d";

        public const string StartDropDownDefaultMessage = "Please select";

        public const int DefaultProductQuantity = 0;

        public const string DateTimeFormat = "{0:g}";

        public const string StatusProcessing = "Processing";

        public const string StatusDone = "Done";

        public const string StatusCancelledByStylist = "Cancelled";

        public const string StatusApproved = "Approved";

        public const int DefaultLastArticlesCount = 5;

        public const int StylistShortDescriptionLength = 50;

        public const int ProcedureNameShort = 15;

        public const int StylistArticleShortDescriptionLength = 150;

        public const int FavouriteProductShortDescriptionLength = 60;

        public const int ArticleShortDescriptionLength = 200;

        public const string YearNamePeriod = "year";

        public const string MonthNamePeriod = "month";

        public const int DaysOneYear = 365;

        public const int DaysOneMonth = 30;

        public const int DaysOneWeek = 7;

        public const int CacheDay = 1;

        public const int CacheMinutesRecentArticles = 30;
    }
}
