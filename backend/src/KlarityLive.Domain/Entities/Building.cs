using KlarityLive.Domain.Entities.Base;

namespace KlarityLive.Domain.Entities
{
    public record Building : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Suburb { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public virtual ICollection<Tenancy> Tenancies { get; set; }
        public virtual ICollection<Meter> Meters { get; set; }
    }
}
