using System;
using System.Collections.Generic;

namespace SqlXmlMapper.Resolvers
{
    public interface IInsqlMapSection
    {
        Type Type { get; }

        string Table { get; }

        string Schema { get; }

        Dictionary<string, IInsqlMapSectionElement> Elements { get; }
    }
}
