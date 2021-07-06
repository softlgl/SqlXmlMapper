using System;
using System.Collections.Generic;

namespace SqlXmlMapper.Resolvers.Sections
{
    public class MapInsqlSection : IInsqlMapSection
    {
        public Type Type { get; }

        public Dictionary<string, IInsqlMapSectionElement> Elements { get; }

        public string Table { get; set; }

        public string Schema { get; set; }

        public MapInsqlSection(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentNullException(nameof(typeName), $"insql map type is null!");
            }

            var type = Type.GetType(typeName);

            if (type == null)
            {
                throw new Exception($"insql map type : {typeName} not found !");
            }

            this.Type = type;
            this.Elements = new Dictionary<string, IInsqlMapSectionElement>();
        }
    }
}
