using SqlXmlMapper;
using SqlXmlMapper.Resolvers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace EntityFramework
{
    public static class DbContextExtensions
    {
        public static List<T> SqlQuery<NameSpace,T>(this DbContext dbContext, IServiceProvider serviceProvider, string sqlId, object parameters)
        {
            return SqlQuery<T>(dbContext, serviceProvider, typeof(NameSpace), sqlId,parameters);
        }

        public static List<T> SqlQuery<T>(this DbContext dbContext, IServiceProvider serviceProvider, Type nameSpace ,string sqlId, object parameters)
        {
            return SqlQuery<T>(dbContext, serviceProvider, nameSpace.FullName, sqlId, parameters);
        }

        public static List<T> SqlQuery<T>(this DbContext dbContext, IServiceProvider serviceProvider, string nameSpace, string sqlId, object parameters)
        {
            return dbContext.Database.SqlQuery<T>(serviceProvider, nameSpace, sqlId, parameters);
        }

        public static List<T> SqlQuery<NameSpace, T>(this Database database, IServiceProvider serviceProvider, string sqlId, object parameters)
        {
            return SqlQuery<T>(database, serviceProvider, typeof(NameSpace), sqlId, parameters);
        }

        public static List<T> SqlQuery<T>(this Database database, IServiceProvider serviceProvider, Type nameSpace, string sqlId, object parameters)
        {
            return SqlQuery<T>(database, serviceProvider, nameSpace.FullName, sqlId, parameters);
        }

        public static List<T> SqlQuery<T>(this Database database, IServiceProvider serviceProvider, string nameSpace, string sqlId, object parameters)
        {
            InsqlOptions insqlOptions = serviceProvider.GetService<InsqlOptions>();
            InsqlResolver insqlResolver = insqlOptions.GetInsqlResolver(nameSpace);
            var resolveResult = insqlResolver.Resolve(sqlId, parameters);
            var sqlParams = resolveResult.Param.Select(i => new MySqlParameter("@" + i.Key, i.Value)).ToArray();
            return database.SqlQuery<T>(resolveResult.Sql, sqlParams).ToListAsync()
                .GetAwaiter().GetResult();
        }
    }
}
