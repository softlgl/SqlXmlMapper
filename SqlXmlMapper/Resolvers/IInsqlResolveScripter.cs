using System;
using System.Collections.Generic;

namespace SqlXmlMapper.Resolvers
{
    public interface IInsqlResolveScripter : IDisposable
    {
        object Resolve(TypeCode type, string code, IDictionary<string, object> param);
    }
}
