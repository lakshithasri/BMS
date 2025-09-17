using KlarityLive.Domain.Core.Entities.Base;
using MediatR;

namespace KlarityLive.Application.Common.Commands
{
    public class CreateCommand<TEntity> : IRequest<TEntity> where TEntity : BaseEntity
    {
        public TEntity Entity { get; set; }
    }
}
