namespace PBL6.Dto
{
    public class ProductCategoryDto
    {
        public Guid ProductCategoryId { get; set; }
        public Guid ProductId { get; set; }
        public CategoryDto? Category { get; set; }
    }
}
