namespace BeautySalon.Web.ViewModels.SkinTypes.ViewModels
{
    using System.Collections.Generic;

    public class AllSkinTypesViewModel
    {
        public IEnumerable<SkinTypeViewModel> SkinTypes { get; set; }

        public bool HasToAddSecondCriteria { get; set; }
    }
}
