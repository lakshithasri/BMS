namespace KlarityLive.Domain.Core.Entities.Base
{
    public class BaseEntity : ITimestamped
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
