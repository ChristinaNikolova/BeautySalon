namespace BeautySalon.Web.ViewModels.Administration.Cards.ViewModels
{
    using System.Collections.Generic;

    public class AllCardsViewModel
    {
        public IEnumerable<CardViewModel> ActiveCards { get; set; }

        public IEnumerable<CardViewModel> ExpiredCards { get; set; }
    }
}
