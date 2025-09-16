using Dapper;
using KlarityLive.Application.Features.Base.Queries;
using KlarityLive.Application.Features.Base.Services;
using KlarityLive.Domain.Entities.Base;
using MediatR;
using System.Data;

namespace KlarityLive.Application.Features.Base.QueryHandlers
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
            var (sql, parameters) = _queryBuilder.BuildGetAllQuery<TEntity>(request);
            return await _connection.QueryAsync<TEntity>(sql, parameters);
        }
    }
}
