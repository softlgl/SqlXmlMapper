namespace SqlXmlMapper.Resolvers
{
    public interface IInsqlMapSectionElement
    {
        string Name { get; }

        string Property { get; }

        bool Identity { get; }

        bool Readonly { get; }

        InsqlMapElementType ElementType { get; }
    }

    public enum InsqlMapElementType
    {
        None,
        Key,
        Column
    }
}
