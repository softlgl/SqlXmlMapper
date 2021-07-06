using System;

namespace SqlXmlMapper.Resolvers.Elements
{
    public class KeyMapSectionElement : IInsqlMapSectionElement
    {
        public string Name { get; }

        public string Property { get; }

        public bool Identity { get; set; }

        public bool Readonly { get; set; }

        public InsqlMapElementType ElementType => InsqlMapElementType.Key;

        public KeyMapSectionElement(string name, string property)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrWhiteSpace(property))
            {
                throw new ArgumentNullException(nameof(property));
            }

            this.Name = name;
            this.Property = property;
        }
    }
}
