using AutoMapper;
using MasterServer.Application.Common.Models;
using MasterServer.Domain.Common;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Linq.Expressions;

namespace MasterServer.Application.Common.Extensions
{
    public static class UpsertDelDbExtensions
    {
        public static void Upsertdel<TEntity, TDto>(List<TEntity> entities, List<TDto> dtos, Func<TDto, int> getDtoKey, Func<TEntity, int> getEntityKey, Func<TDto, TEntity> onNew, Action<TDto, TEntity> onUpdate)
        {
            var removeItems = entities.Where(x => !dtos.Select(y => getDtoKey(y)).Contains(getEntityKey(x)));

            for (int i = removeItems.Count() - 1; i >= 0; i--)
            {
                entities.Remove(removeItems.ElementAt(i));
            }
            foreach (var updateDto in dtos)
            {
                var equals = entities.Where(x =>
                {
                    var entityKey = getEntityKey(x);
                    var dtoKey = getDtoKey(updateDto);
                    return entityKey == dtoKey;
                });


                var item = equals.FirstOrDefault();
                if (item != null)
                {
                    onUpdate(updateDto, item);
                }
                else
                {
                    item = onNew(updateDto);
                    entities.Add(item);
                }
            }
        }
    }
    public static class PaginationDbExtensions
    {
        public static async Task<PaginatedList<TEntity>> ToPaginationAsync<TEntity>(this IQueryable<TEntity> queryable, VuetifyPaginationParameter queryParameter, bool skipOrder = false, bool defaultOrderAsc = true) where TEntity : class
        {
            if (queryParameter.PaginationLimit == -1)
            {
                queryParameter.Full = true;
            }
            var count = await queryable.CountAsync();
            var queryList = queryable;
            if (!skipOrder)
            {
                if ((queryParameter.SortBy == null || queryParameter.SortBy.Length == 0) && typeof(TEntity).IsSubclassOf(typeof(BaseAuditableEntity)))
                {
                    if (defaultOrderAsc)
                    {
                        queryList = queryList.OrderBy(x => EF.Property<DateTime>(x, "CreatedAt"));
                    }
                    else
                    {
                        queryList = queryList.OrderByDescending(x => EF.Property<DateTime>(x, "CreatedAt"));
                    }

                }
                else if ((queryParameter.SortBy == null || queryParameter.SortBy.Length == 0) && typeof(TEntity).IsSubclassOf(typeof(IEntityIntKey)))
                {

                    if (defaultOrderAsc)
                    {
                        queryList = queryList.OrderBy(x => EF.Property<int>(x, "Id"));
                    }
                    else
                    {
                        queryList = queryList.OrderByDescending(x => EF.Property<int>(x, "Id"));
                    }
                }
                if (queryParameter.SortBy != null)
                {
                    int idxSort = 0;
                    foreach (var sortInfo in queryParameter.SortBy)
                    {
                        if (!string.IsNullOrEmpty(sortInfo.Key))
                        {
                            if (idxSort == 0)
                            {
                                //System.Reflection.PropertyInfo sortByProperty = typeof(TEntity).GetProperty();
                                if (!string.IsNullOrEmpty(sortInfo.Order) && sortInfo.Order.Equals("desc"))
                                {
                                    queryList = queryList.OrderByDescending(sortInfo.Key);
                                }
                                else
                                {
                                    queryList = queryList.OrderBy(sortInfo.Key);
                                }
                            }
                            else
                            {
                                //System.Reflection.PropertyInfo sortByProperty = typeof(TEntity).GetProperty();
                                if (!string.IsNullOrEmpty(sortInfo.Order) && sortInfo.Order.Equals("desc"))
                                {
                                    queryList = ((IOrderedQueryable<TEntity>)queryList).ThenOrderByDescending(sortInfo.Key);
                                }
                                else
                                {
                                    queryList = ((IOrderedQueryable<TEntity>)queryList).ThenOrderBy(sortInfo.Key);
                                }
                            }
                            idxSort++;
                        }

                    }
                }


            }
            queryList = queryParameter.Full ? queryList
                                           : queryList.Skip((queryParameter.PaginationPage - 1) * queryParameter.PaginationLimit).Take(queryParameter.PaginationLimit);
            List<TEntity> datas = await queryList.ToListAsync();
            var t = count == 0 ? 0 : ((decimal)count / (queryParameter.Full ? count : queryParameter.PaginationLimit));
            return new PaginatedList<TEntity>(datas, count, queryParameter.Full ? 1 : queryParameter.PaginationPage, queryParameter.Full ? count : queryParameter.PaginationLimit);
        }


        public static async Task<PaginatedList<TEntity>> ToPaginationAsync<TEntity>(this IQueryable<TEntity> queryable, IPaginationParameter queryParameter, bool skipOrder = true) where TEntity : class
        {
            if (queryParameter.PaginationLimit == -1)
            {
                queryParameter.Full = true;
            }
            var count = await queryable.CountAsync();
            var queryList = queryable;
            if (!skipOrder)
            {
                if (typeof(TEntity).IsSubclassOf(typeof(BaseAuditableEntity)))
                {
                    queryList = queryList.OrderBy(x => EF.Property<DateTime>(x, "CreatedAt"));
                }
                if (!string.IsNullOrEmpty(queryParameter.SortBy))
                {
                    //System.Reflection.PropertyInfo sortByProperty = typeof(TEntity).GetProperty();
                    if (!string.IsNullOrEmpty(queryParameter.OrderBy) && queryParameter.OrderBy.Equals("descend"))
                    {
                        queryList = queryList.OrderByDescending(queryParameter.SortBy);
                    }
                    else
                    {
                        queryList = queryList.OrderBy(queryParameter.SortBy);
                    }
                }
            }
            queryList = queryParameter.Full ? queryList
                                           : queryList.Skip((queryParameter.PaginationPage - 1) * queryParameter.PaginationLimit).Take(queryParameter.PaginationLimit);
            List<TEntity> datas = await queryList.ToListAsync();
            var t = count == 0 ? 0 : ((decimal)count / (queryParameter.Full ? count : queryParameter.PaginationLimit));
            return new PaginatedList<TEntity>(datas, count, queryParameter.Full ? 1 : queryParameter.PaginationPage, queryParameter.Full ? count : queryParameter.PaginationLimit);
        }

        public static PaginatedList<TEntity> ToPaginationAsync<TEntity>(this IEnumerable<TEntity> queryable, IPaginationParameter queryParameter, bool skipOrder = true) where TEntity : class
        {
            if (queryParameter.PaginationLimit == -1)
            {
                queryParameter.Full = true;
            }
            var count = queryable.Count();
            var queryList = queryable;
            if (!skipOrder)
            {
                if (typeof(TEntity).IsSubclassOf(typeof(BaseAuditableEntity)))
                {
                    queryList = queryList.OrderBy(x => EF.Property<Instant>(x, "CreatedAt"));
                }
                if (!string.IsNullOrEmpty(queryParameter.SortBy))
                {
                    //System.Reflection.PropertyInfo sortByProperty = typeof(TEntity).GetProperty();
                    if (!string.IsNullOrEmpty(queryParameter.OrderBy) && queryParameter.OrderBy.Equals("descend"))
                    {
                        queryList = queryList.OrderByDescending(queryParameter.SortBy);
                    }
                    else
                    {
                        queryList = queryList.OrderBy(queryParameter.SortBy);
                    }
                }
            }

            queryList = queryParameter.Full ? queryList
                                           : queryList.Skip((queryParameter.PaginationPage - 1) * queryParameter.PaginationLimit).Take(queryParameter.PaginationLimit);
            List<TEntity> datas = queryList.ToList();
            var t = ((decimal)count / (queryParameter.Full ? count : queryParameter.PaginationLimit));
            return new PaginatedList<TEntity>(datas, count, queryParameter.Full ? 1 : queryParameter.PaginationPage, queryParameter.Full ? count : queryParameter.PaginationLimit);
        }

        public static async Task<PaginatedList<TEntityOut>> ToPaginationAsync<TEntity, TEntityOut>(this IQueryable<TEntity> queryable, IPaginationParameter queryParameter, Func<TEntity, TEntityOut> selectFunc, bool skipOrder = true) where TEntity : class
        {
            if (queryParameter.PaginationLimit == -1)
            {
                queryParameter.Full = true;
            }
            var count = await queryable.CountAsync();
            var queryList = queryable;
            if (!skipOrder)
            {
                if (typeof(TEntity).IsSubclassOf(typeof(BaseAuditableEntity)))
                {
                    queryList = queryList.OrderBy(x => EF.Property<Instant>(x, "CreatedAt"));
                }
                if (!string.IsNullOrEmpty(queryParameter.SortBy))
                {
                    //System.Reflection.PropertyInfo sortByProperty = typeof(TEntity).GetProperty();
                    if (!string.IsNullOrEmpty(queryParameter.OrderBy) && queryParameter.OrderBy.Equals("descend"))
                    {
                        queryList = queryList.OrderByDescending(queryParameter.SortBy);
                    }
                    else
                    {
                        queryList = queryList.OrderBy(queryParameter.SortBy);
                    }
                }
            }

            queryList = queryParameter.Full ? queryList :
                                               queryList.Skip((queryParameter.PaginationPage - 1) * queryParameter.PaginationLimit).Take(queryParameter.PaginationLimit);
            List<TEntityOut> datas = await queryList.Select(x => selectFunc(x)).ToListAsync();
            var t = ((decimal)count / (queryParameter.Full ? count : queryParameter.PaginationLimit));
            return new PaginatedList<TEntityOut>(datas, count, queryParameter.Full ? 1 : queryParameter.PaginationPage, queryParameter.Full ? count : queryParameter.PaginationLimit);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderBy(ToLambda<T>(propertyName));
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(ToLambda<T>(propertyName));
        }

        public static IOrderedQueryable<T> ThenOrderBy<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return source.ThenBy(ToLambda<T>(propertyName));
        }


        public static IOrderedQueryable<T> ThenOrderByDescending<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return source.ThenByDescending(ToLambda<T>(propertyName));
        }
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string propertyName)
        {
            return source.OrderBy(propertyName);
        }

        public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> source, string propertyName)
        {
            return source.OrderByDescending(propertyName);
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

        public static PaginatedList<V> MapPagination<U, V>(this IMapper mapper, PaginatedList<U> pagination) where U : class where V : class
        {
            return new PaginatedList<V>(mapper.Map<List<V>>(pagination.Items), pagination.TotalCount, pagination.PageNumber, pagination.PageSize);
        }


    }

}
