using StoreProject.Models;

namespace StoreProject.ViewModels.Stores
{
    public class StoreIndexViewModel
    {
        public string  Name { get; set; }
        public string Phone { get; set; }
        public string  Email { get; set; }

        public List<Store> Stores { get; set; }
    }
}
