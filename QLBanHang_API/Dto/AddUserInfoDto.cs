﻿namespace QLBanHang_API.Dto
{
    public class AddUserInfoDto
    {

        public Guid UserId { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? Gender { get; set; }
    }
}
