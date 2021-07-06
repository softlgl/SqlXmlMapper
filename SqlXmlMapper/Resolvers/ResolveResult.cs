using System.Collections.Generic;

namespace SqlXmlMapper.Resolvers
{
    public class ResolveResult
    {
        public string Sql { get; set; }

        public IDictionary<string, object> Param { get; set; }
    }
}
