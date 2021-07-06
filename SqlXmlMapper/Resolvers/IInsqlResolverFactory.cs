using System;

namespace SqlXmlMapper.Resolvers
{
    public interface IInsqlResolverFactory
    {
        IInsqlResolver CreateResolver(Type contextType);
    }
}
