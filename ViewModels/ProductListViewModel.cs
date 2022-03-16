using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreProject.Models;

namespace StoreProject.ViewModels
{
    public class ProductListViewModel
    {

        public int? Page { get; set; } = 1;


        public string SearchString { get; set; }

        public int SelectedBrand { get; set; }
        public int SelectedCategory { get; set; }
        public short SelectedYear { get; set; }
        public List<Product> Products { get; set; }
        public IEnumerable<SelectListItem> Brands { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> ModelYears { get; set; }

        //*******************************************************

        public int PageNumber { get; set; } = 1;
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }


        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int Endpage { get; private set; }

        public ProductListViewModel()
        {

        }

        public ProductListViewModel(int totalItems, int page, int pageSize = 10)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            int currentPage = page;

            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            Endpage = endPage;


        }

    



}

}

