using Dapper;
using KlarityLive.Application.Features.Base.Queries;
using KlarityLive.Application.Features.Base.Services;
using KlarityLive.Domain.Entities.Base;
using MediatR;
using System.Data;

namespace KlarityLive.Application.Features.Base.QueryHandlers
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
            var (sql, parameters) = _queryBuilder.BuildGetByIdQuery<TEntity>(request.Id);
            return await _connection.QueryFirstOrDefaultAsync<TEntity>(sql, parameters);
        }
    }
}
