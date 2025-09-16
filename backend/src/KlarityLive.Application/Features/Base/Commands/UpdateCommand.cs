using KlarityLive.Domain.Entities.Base;
using MediatR;

namespace KlarityLive.Application.Features.Base.Commands
{
    public class UpdateCommand<TEntity> : IRequest<TEntity>
    where TEntity : BaseEntity
    {
        public int Id { get; set; }
        public TEntity Entity { get; set; }
    }
}
