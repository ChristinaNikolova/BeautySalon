﻿namespace BeautySalon.Web.ViewModels.TypeCards.ViewModels
{
    using System.Collections.Generic;

    public class AllTypeCardsViewModel
    {
        public IEnumerable<TypeCardViewModel> TypeCards { get; set; }

        public bool HasCard { get; set; }

        public string SkinCareCategoryId { get; set; }

        public string NailsCategoryId { get; set; }

        public string HaircutsProcedureId { get; set; }
    }
}
