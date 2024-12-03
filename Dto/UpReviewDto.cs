namespace QLBanHang_API.Dto
{
    public class UpReviewDto
    {
        public Guid? ReviewId { get; set; }
        public Guid ProductId { get; set; }   // Foreign Key

        public Guid UserId { get; set; }      // Foreign Key

        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
