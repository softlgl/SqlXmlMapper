using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SqlXmlMapper.Resolvers;
using Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace SqlXmlMapper
{
    public static class DbConnectionExtension
    {
        /// <summary>
        /// xmlsql查询
        /// </summary>
        /// <typeparam name="NameSpace">命名空间和xml type对应</typeparam>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="dbConnection"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="sqlId">sql唯一标识</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static IEnumerable<T> Query<NameSpace,T>(this IDbConnection dbConnection,
            IServiceProvider serviceProvider,string sqlId, object param = null)
        {
            return dbConnection.Query<T>(serviceProvider, typeof(NameSpace).FullName, sqlId, param);
        }

        /// <summary>
        /// xmlsql查询
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="dbConnection"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="nameSpace">命名空间和xml type对应</param>
        /// <param name="sqlId">sql唯一标识</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(this IDbConnection dbConnection,
            IServiceProvider serviceProvider, Type nameSpace, string sqlId, object param = null)
        {
            return dbConnection.Query<T>(serviceProvider, nameSpace.FullName, sqlId, param);
        }

        /// <summary>
        /// xmlsql查询
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="dbConnection"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="nameSpace">命名空间和xml type对应</param>
        /// <param name="sqlId">sql唯一标识</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(this IDbConnection dbConnection, 
            IServiceProvider serviceProvider, string nameSpace, string sqlId, object param = null)
        {
            InsqlOptions insqlOptions = serviceProvider.GetService<InsqlOptions>();
            InsqlResolver insqlResolver = insqlOptions.GetInsqlResolver(nameSpace);
            var resolveResult = insqlResolver.Resolve(sqlId, param);
            return dbConnection.Query<T>(resolveResult.Sql, param);
        }
    }
}
