namespace BeautySalon.Web.ViewModels.Administration.Cards.ViewModels
{
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using BeautySalon.Web.ViewModels.Users.ViewModels;

    public class CardViewModel : UserCardViewModel, IMapFrom<Card>
    {
        public string ClientUsername { get; set; }
    }
}
