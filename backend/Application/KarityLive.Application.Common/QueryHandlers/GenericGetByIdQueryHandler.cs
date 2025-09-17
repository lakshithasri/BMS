using Dapper;
using KlarityLive.Application.Common.Queries;
using KlarityLive.Application.Common.Services;
using KlarityLive.Domain.Core.Entities;
using KlarityLive.Domain.Core.Entities.Base;
using MediatR;
using System.Data;

namespace KlarityLive.Application.Common.QueryHandlers
{
    public class GenericGetByIdQueryHandler<TEntity> : IRequestHandler<GetByIdQuery<TEntity>, TEntity>
    where TEntity : BaseEntity
    {
        private readonly IDbConnection _connection;
        private readonly IQueryBuilder _queryBuilder;

        public GenericGetByIdQueryHandler(IDbConnection connection, IQueryBuilder queryBuilder)
        {
            _connection = connection;
            _queryBuilder = queryBuilder;
        }

        public async Task<TEntity> Handle(GetByIdQuery<TEntity> request, CancellationToken cancellationToken)
        {
            var entityName = typeof(TEntity).Name;
            var tableName = EntityTableMappings.Mappings[entityName];
            var (sql, parameters) = _queryBuilder.BuildGetByIdQuery<TEntity>(request.Id, tableName);
            return await _connection.QueryFirstOrDefaultAsync<TEntity>(sql, parameters);
        }
    }
}
