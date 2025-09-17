using KlarityLive.Domain.Core.Entities.Base;
using MediatR;

namespace KlarityLive.Application.Common.Queries
{
    public class GetAllQuery<TEntity> : IRequest<IEnumerable<TEntity>>
        where TEntity : BaseEntity
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string OrderBy { get; set; } = "Id";
        public bool Descending { get; set; } = false;
        public Dictionary<string, object> Filters { get; set; } = new();
    }
}
