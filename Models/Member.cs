namespace LibraryApi.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string CI { get; set; } = null!;         
        public string PhoneNumber { get; set; } = null!; 
        public DateTime RegistrationDate { get; set; }
    }
}