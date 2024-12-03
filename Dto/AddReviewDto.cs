namespace QLBanHang_API.Dto
{
    public class AddReviewDto
    {
        public Guid ProductId { get; set; }   // Foreign Key

        public Guid UserId { get; set; }      // Foreign Key

        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
