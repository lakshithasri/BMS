namespace KlarityLive.Domain.Core.Entities.Base
{
    public interface ITimestamped
    {
        DateTime CreatedOn { get; set; }
        DateTime UpdatedOn { get; set; }
    }
}
