using KlarityLive.Domain.Core.Entities.Base;
using MediatR;

namespace KlarityLive.Application.Common.Queries
{
    public class GetByIdQuery<TEntity> : IRequest<TEntity>
     where TEntity : BaseEntity
    {
        public int Id { get; set; }
    }

}
