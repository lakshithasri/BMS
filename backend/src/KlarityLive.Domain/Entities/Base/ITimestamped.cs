namespace KlarityLive.Domain.Entities.Base
{
    public interface ITimestamped
    {
        DateTime CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; }
    }
}
