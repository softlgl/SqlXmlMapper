namespace SqlXmlMapper.Resolvers
{
    public interface IInsqlSection
    {
        string Id { get; }

        InsqlSectionType SectionType { get; }

        string Resolve(ResolveContext context);
    }

    public enum InsqlSectionType
    {
        None,
        Sql,
        Select,
        Insert,
        Update,
        Delete
    }
}
