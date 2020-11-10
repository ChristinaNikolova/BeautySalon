namespace BeautySalon.Web.ViewModels.Administration.Comments.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class CommentAdminstrationViewModel : IMapFrom<Comment>
    {
        public string Id { get; set; }

        public string ArticleId { get; set; }

        public string ArticleTitle { get; set; }

        public string ClientUsername { get; set; }

        public string Content { get; set; }
    }
}
