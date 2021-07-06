using SqlXmlMapper.Resolvers;
using System;
using System.Collections.Generic;

namespace SqlXmlMapper.Providers
{
    public interface IInsqlDescriptorLoader
    {
        IDictionary<Type, InsqlDescriptor> Load();
    }
}
