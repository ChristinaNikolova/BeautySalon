namespace BeautySalon.Web.ViewModels.Chats.ViewModels
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public string AdminUsername { get; set; }

        public IEnumerable<ClientChatViewModel> Users { get; set; }
    }
}
