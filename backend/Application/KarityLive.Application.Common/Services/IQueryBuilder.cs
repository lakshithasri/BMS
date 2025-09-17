using KlarityLive.Application.Common.Queries;
using KlarityLive.Domain.Core.Entities.Base;

namespace KlarityLive.Application.Common.Services
{
    public interface IQueryBuilder
    {
        (string sql, object parameters) BuildGetByIdQuery<TEntity>(int id, string tableName) where TEntity : BaseEntity;
        (string sql, object parameters) BuildGetAllQuery<TEntity>(GetAllQuery<TEntity> query, string tableName) where TEntity : BaseEntity;
        (string sql, object parameters) BuildCountQuery<TEntity>(Dictionary<string, object> filters, string tableName) where TEntity : BaseEntity;
    }
}
