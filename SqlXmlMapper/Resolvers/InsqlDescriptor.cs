using System;
using System.Collections.Generic;

namespace SqlXmlMapper.Resolvers
{
    public class InsqlDescriptor
    {
        public string TypeFullName { get;}

        public Dictionary<string, IInsqlSection> Sections { get; }

        public Dictionary<Type, IInsqlMapSection> Maps { get; }

        public InsqlDescriptor(string typeName)
        {
            if (typeName == null)
            {
                throw new Exception($"insql type : {typeName} not found !");
            }

            this.TypeFullName = typeName;
            this.Sections = new Dictionary<string, IInsqlSection>();
            this.Maps = new Dictionary<Type, IInsqlMapSection>();
        }
    }
}
