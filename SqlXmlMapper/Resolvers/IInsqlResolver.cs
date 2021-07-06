using System.Collections.Generic;

namespace SqlXmlMapper.Resolvers
{
    public interface IInsqlResolver
    {
        ResolveResult Resolve(string sqlId, IDictionary<string, object> sqlParam);
    }
}
