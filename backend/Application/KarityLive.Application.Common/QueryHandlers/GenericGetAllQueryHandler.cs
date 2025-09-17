using Dapper;
using KlarityLive.Application.Common.Queries;
using KlarityLive.Application.Common.Services;
using KlarityLive.Domain.Core.Entities;
using KlarityLive.Domain.Core.Entities.Base;
using MediatR;
using System.Data;

namespace KlarityLive.Application.Common.QueryHandlers
{
    public class GenericGetAllQueryHandler<TEntity> : IRequestHandler<GetAllQuery<TEntity>, IEnumerable<TEntity>>
       where TEntity : BaseEntity
    {
        private readonly IDbConnection _connection;
        private readonly IQueryBuilder _queryBuilder;

        public GenericGetAllQueryHandler(IDbConnection connection, IQueryBuilder queryBuilder)
        {
            _connection = connection;
            _queryBuilder = queryBuilder;
        }

        public async Task<IEnumerable<TEntity>> Handle(GetAllQuery<TEntity> request, CancellationToken cancellationToken)
        {
            var entityName = typeof(TEntity).Name;
            var tableName = EntityTableMappings.Mappings[entityName];
            var (sql, parameters) = _queryBuilder.BuildGetAllQuery<TEntity>(request, tableName);
            return await _connection.QueryAsync<TEntity>(sql, parameters);
        }
    }
}
