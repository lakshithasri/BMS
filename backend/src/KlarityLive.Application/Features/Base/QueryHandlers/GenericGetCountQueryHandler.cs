using Dapper;
using KlarityLive.Application.Features.Base.Queries;
using KlarityLive.Application.Features.Base.Services;
using KlarityLive.Domain.Entities.Base;
using MediatR;
using System.Data;

namespace KlarityLive.Application.Features.Base.QueryHandlers
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
            var (sql, parameters) = _queryBuilder.BuildCountQuery<TEntity>(request.Filters);
            return await _connection.QueryFirstOrDefaultAsync<int>(sql, parameters);
        }
    }
}
