using KlarityLive.Domain.Entities.Base;
using MediatR;

namespace KlarityLive.Application.Features.Base.Queries
{
    public class GetBySpecificationQuery<TEntity> : IRequest<IEnumerable<TEntity>>
        where TEntity : BaseEntity
    {
        public string SqlQuery { get; set; }
        public object Parameters { get; set; }
    }
}
