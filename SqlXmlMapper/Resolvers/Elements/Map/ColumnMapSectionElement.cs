using System;

namespace SqlXmlMapper.Resolvers.Elements
{
    public class ColumnMapSectionElement : IInsqlMapSectionElement
    {
        public string Name { get; }

        public string Property { get; }

        public bool Identity { get; set; }

        public bool Readonly { get; set; }

        public InsqlMapElementType ElementType => InsqlMapElementType.Column;

        public ColumnMapSectionElement(string name, string to)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrWhiteSpace(to))
            {
                throw new ArgumentNullException(nameof(to));
            }

            this.Name = name;
            this.Property = to;
        }
    }
}
