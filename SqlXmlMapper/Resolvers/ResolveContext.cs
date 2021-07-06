using System;
using System.Collections.Generic;

namespace SqlXmlMapper.Resolvers
{
    public class ResolveContext
    {
        public IServiceProvider ServiceProvider { get; set; }

        public InsqlDescriptor InsqlDescriptor { get; set; }

        public IInsqlSection InsqlSection { get; set; }

        public IDictionary<string, object> Param { get; set; }
    }
}
