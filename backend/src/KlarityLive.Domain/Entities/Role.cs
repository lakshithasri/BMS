using KlarityLive.Domain.Entities.Base;

namespace KlarityLive.Domain.Entities
{
    public record Role : BaseEntity
    {
        public string Name { get; set; }
    }
}
