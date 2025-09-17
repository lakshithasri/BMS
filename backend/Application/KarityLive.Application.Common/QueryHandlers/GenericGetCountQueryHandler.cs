using Dapper;
using KlarityLive.Application.Common.Queries;
using KlarityLive.Application.Common.Services;
using KlarityLive.Domain.Core.Entities;
using KlarityLive.Domain.Core.Entities.Base;
using MediatR;
using System.Data;

namespace KlarityLive.Application.Common.QueryHandlers
{
    public class GenericGetCountQueryHandler<TEntity> : IRequestHandler<GetCountQuery<TEntity>, int>
        where TEntity : BaseEntity
    {
        private readonly IDbConnection _connection;
        private readonly IQueryBuilder _queryBuilder;

        public GenericGetCountQueryHandler(IDbConnection connection, IQueryBuilder queryBuilder)
        {
            _connection = connection;
            _queryBuilder = queryBuilder;
        }

        public async Task<int> Handle(GetCountQuery<TEntity> request, CancellationToken cancellationToken)
        {
            var entityName = typeof(TEntity).Name;
            var tableName = EntityTableMappings.Mappings[entityName];
            var (sql, parameters) = _queryBuilder.BuildCountQuery<TEntity>(request.Filters, tableName);
            return await _connection.QueryFirstOrDefaultAsync<int>(sql, parameters);
        }
    }
}
