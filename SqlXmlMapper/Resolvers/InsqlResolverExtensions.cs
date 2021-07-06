using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SqlXmlMapper.Resolvers
{
    public static partial class InsqlResolverExtensions
    {
        public static ResolveResult Resolve(this IInsqlResolver sqlResolver, string sqlId)
        {
            return sqlResolver.Resolve(sqlId, (IDictionary<string, object>)null);
        }

        public static ResolveResult Resolve(this IInsqlResolver sqlResolver, string sqlId, object sqlParam)
        {
            if (sqlParam == null)
            {
                return sqlResolver.Resolve(sqlId, (IDictionary<string, object>)null);
            }

            var sqlParamDictionary = sqlParam as IEnumerable<KeyValuePair<string, object>>;

            if (sqlParamDictionary == null)
            {
                sqlParamDictionary = sqlParam.GetType()
               .GetProperties(BindingFlags.Public | BindingFlags.Instance)
               .Select(propInfo => new KeyValuePair<string, object>(propInfo.Name, propInfo.GetValue(sqlParam, null)));
            }

            return sqlResolver.Resolve(sqlId, sqlParamDictionary.ToDictionary(item => item.Key, item => item.Value));
        }
    }
}
