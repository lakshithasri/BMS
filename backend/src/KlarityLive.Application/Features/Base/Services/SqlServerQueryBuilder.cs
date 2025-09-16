using Dapper;
using KlarityLive.Application.Features.Base.Queries;
using KlarityLive.Domain.Entities.Base;

namespace KlarityLive.Application.Features.Base.Services
{
    public class SqlServerQueryBuilder : IQueryBuilder
    {
        public (string sql, object parameters) BuildGetByIdQuery<TEntity>(int id) where TEntity : BaseEntity
        {
            var tableName = GetTableName<TEntity>();
            var columns = GetColumnNames<TEntity>();

            var sql = $"SELECT {string.Join(", ", columns)} FROM {tableName} WHERE Id = @Id";
            var whereClause = BuildSoftDeleteFilter<TEntity>();
            if (!string.IsNullOrEmpty(whereClause))
                sql += $" AND {whereClause}";

            return (sql, new { Id = id });
        }

        public (string sql, object parameters) BuildGetAllQuery<TEntity>(GetAllQuery<TEntity> query) where TEntity : BaseEntity
        {
            var tableName = GetTableName<TEntity>();
            var columns = GetColumnNames<TEntity>();

            var sql = $"SELECT {string.Join(", ", columns)} FROM {tableName}";
            var parameters = new DynamicParameters();
            var whereConditions = new List<string>();

            // Add soft delete filter
            var softDeleteFilter = BuildSoftDeleteFilter<TEntity>();
            if (!string.IsNullOrEmpty(softDeleteFilter))
                whereConditions.Add(softDeleteFilter);

            // Add dynamic filters
            foreach (var filter in query.Filters)
            {
                whereConditions.Add($"{filter.Key} = @{filter.Key}");
                parameters.Add(filter.Key, filter.Value);
            }

            if (whereConditions.Any())
                sql += $" WHERE {string.Join(" AND ", whereConditions)}";

            // Add ordering
            var orderDirection = query.Descending ? "DESC" : "ASC";
            sql += $" ORDER BY {query.OrderBy} {orderDirection}";

            // Add pagination
            sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            parameters.Add("Offset", (query.Page - 1) * query.PageSize);
            parameters.Add("PageSize", query.PageSize);

            return (sql, parameters);
        }

        public (string sql, object parameters) BuildCountQuery<TEntity>(Dictionary<string, object> filters) where TEntity : BaseEntity
        {
            var tableName = GetTableName<TEntity>();
            var sql = $"SELECT COUNT(*) FROM {tableName}";
            var parameters = new DynamicParameters();
            var whereConditions = new List<string>();

            // Add soft delete filter
            var softDeleteFilter = BuildSoftDeleteFilter<TEntity>();
            if (!string.IsNullOrEmpty(softDeleteFilter))
                whereConditions.Add(softDeleteFilter);

            // Add dynamic filters
            foreach (var filter in filters)
            {
                whereConditions.Add($"{filter.Key} = @{filter.Key}");
                parameters.Add(filter.Key, filter.Value);
            }

            if (whereConditions.Any())
                sql += $" WHERE {string.Join(" AND ", whereConditions)}";

            return (sql, parameters);
        }

        private string GetTableName<TEntity>() where TEntity : class
        {
            // Simple pluralization - you might want to use a more sophisticated approach
            var entityName = typeof(TEntity).Name;
            return entityName.EndsWith("y") ? entityName.Substring(0, entityName.Length - 1) + "ies" : entityName + "s";
        }

        private IEnumerable<string> GetColumnNames<TEntity>() where TEntity : class
        {
            return typeof(TEntity).GetProperties()
                .Where(p => p.CanRead && IsSimpleType(p.PropertyType))
                .Select(p => p.Name);
        }

        private string BuildSoftDeleteFilter<TEntity>() where TEntity : class
        {
            return typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)) ? "IsDeleted = 0" : string.Empty;
        }

        private bool IsSimpleType(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
            return underlyingType.IsPrimitive ||
                   underlyingType.IsEnum ||
                   underlyingType == typeof(string) ||
                   underlyingType == typeof(decimal) ||
                   underlyingType == typeof(DateTime) ||
                   underlyingType == typeof(Guid);
        }
    }
}
