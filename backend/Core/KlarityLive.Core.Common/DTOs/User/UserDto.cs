namespace KlarityLive.Core.Common.DTOs.User
{
    public class UserDto
    {
        public int? Id { get; set; }
        public int? TenantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsDeleted { get; set; }
    }
}
