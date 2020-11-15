namespace BeautySalon.Web.ViewModels.Articles.ViewModels
{
    using System.Collections.Generic;

    public class AllUsersFavouriteArticlesViewModel
    {
        public IEnumerable<UsersFavouriteArticlesViewModel> Articles { get; set; }
    }
}
