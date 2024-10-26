namespace QLBanHang_API.Dto
{
    public class ReviewDto
    {
        public Guid ReviewId { get; set; }    // Primary Key

        public Guid ProductId { get; set; }   // Foreign Key

        public Guid UserId { get; set; }      // Foreign Key

        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public UserDto? User { get; set; }
    }
}
