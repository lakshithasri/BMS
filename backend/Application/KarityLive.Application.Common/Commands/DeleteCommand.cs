using KlarityLive.Domain.Core.Entities.Base;
using MediatR;

namespace KlarityLive.Application.Common.Commands
{
    public class DeleteCommand<TEntity> : IRequest<bool>
        where TEntity : BaseEntity
    {
        public int Id { get; set; }
        public bool SoftDelete { get; set; } = true;
    }
}
