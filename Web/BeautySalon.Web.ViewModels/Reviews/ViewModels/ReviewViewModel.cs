namespace BeautySalon.Web.ViewModels.Reviews.ViewModels
{
    using System;

    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ReviewViewModel : IMapFrom<Review>
    {
        public string Content { get; set; }

        public int Points { get; set; }

        public DateTime Date { get; set; }

        public string FormattedDate
            => this.Date.ToString("dd.MM.yyyy hh:mm:ss");

        public string ClientPicture { get; set; }

        public string ClientUsername { get; set; }
    }
}
