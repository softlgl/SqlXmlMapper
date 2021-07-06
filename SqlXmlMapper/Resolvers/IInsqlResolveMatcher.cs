using System.Collections.Generic;

namespace SqlXmlMapper.Resolvers
{
    public interface IInsqlResolveMatcher
    {
        IInsqlSection Match(InsqlDescriptor insqlDescriptor, string sqlId, IDictionary<string, object> sqlParam);
    }
}
