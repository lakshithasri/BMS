using KlarityLive.Domain.Entities.Base;
using MediatR;

namespace KlarityLive.Application.Features.Base.Commands
{
    public class CreateCommand<TEntity> : IRequest<TEntity> where TEntity : BaseEntity
    {
        public TEntity Entity { get; set; }
    }
}
