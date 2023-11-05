namespace BeautySalon.Web.ViewModels.Categories.ViewModels
{
    using System.Collections.Generic;

    public class AllCategoriesViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public bool HasToAddSecondCriteria { get; set; }
    }
}
