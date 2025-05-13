﻿namespace LibraryApi.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime RegistrationDate { get; set; }
    }
}
