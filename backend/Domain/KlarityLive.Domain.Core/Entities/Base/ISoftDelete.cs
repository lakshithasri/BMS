namespace KlarityLive.Domain.Core.Entities.Base
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
