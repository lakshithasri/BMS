using KlarityLive.Domain.Entities.Base;
using MediatR;

namespace KlarityLive.Application.Features.Base.Queries
{
    public class GetByIdQuery<TEntity> : IRequest<TEntity>
     where TEntity : BaseEntity
    {
        public int Id { get; set; }
    }

}
