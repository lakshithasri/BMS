using KlarityLive.Domain.Core.Entities.Base;

namespace KlarityLive.Domain.Core.Entities.BMS
{
    public class Building : BaseEntity, ISoftDelete
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Suburb { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool IsDeleted { get; set; }

        public List<Meter> Meters { get; set; }
    }
}
