namespace BeautySalon.Web.ViewModels.Articles.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public class SearchArticleCriteriaInputModel
    {
        [Required]
        public string CategoryId { get; set; }
    }
}
