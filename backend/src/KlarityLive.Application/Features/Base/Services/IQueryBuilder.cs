using KlarityLive.Application.Features.Base.Queries;
using KlarityLive.Domain.Entities.Base;

namespace KlarityLive.Application.Features.Base.Services
{
    public interface IQueryBuilder
    {
        (string sql, object parameters) BuildGetByIdQuery<TEntity>(int id) where TEntity : BaseEntity;
        (string sql, object parameters) BuildGetAllQuery<TEntity>(GetAllQuery<TEntity> query) where TEntity : BaseEntity;
        (string sql, object parameters) BuildCountQuery<TEntity>(Dictionary<string, object> filters) where TEntity : BaseEntity;
    }
}
