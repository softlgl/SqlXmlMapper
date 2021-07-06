using System.Collections.Generic;

namespace SqlXmlMapper.Resolvers
{
    public interface IInsqlResolveFilter
    {
        void OnResolving(InsqlDescriptor insqlDescriptor, string sqlId, IDictionary<string, object> sqlParam);

        void OnResolved(ResolveContext resolveContext, ResolveResult resolveResult);
    }
}
