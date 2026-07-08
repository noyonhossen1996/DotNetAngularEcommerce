namespace ECommerceManagement.Application.DTOs.Products
{
    public class ProductQueryDto
    {
        public string? SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsActive { get; set; }

        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
