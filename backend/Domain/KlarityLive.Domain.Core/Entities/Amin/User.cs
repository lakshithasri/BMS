using KlarityLive.Domain.Core.Entities.Base;

namespace KlarityLive.Domain.Core.Entities.Amin
{
    public class User : BaseEntity, ISoftDelete
    {
        public int? TenantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsDeleted { get; set; }
    }
}
