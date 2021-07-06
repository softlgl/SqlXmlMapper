using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlXmlMapper.Resolvers.Sections
{
    public class SqlInsqlSection : IInsqlSection
    {
        public string Id { get; }

        public List<IInsqlSectionElement> Elements { get; }

        public virtual InsqlSectionType SectionType => InsqlSectionType.Sql;

        public SqlInsqlSection(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            this.Id = id;
            this.Elements = new List<IInsqlSectionElement>();
        }

        public virtual string Resolve(ResolveContext context)
        {
            var elementsResult = this.Elements.Select(element =>
            {
                return element.Resolve(context);
            });

            return string.Join(" ", elementsResult).Trim();
        }
    }

    public class SelectSqlSection : SqlInsqlSection
    {
        public SelectSqlSection(string id) : base(id)
        {
        }

        public override InsqlSectionType SectionType => InsqlSectionType.Select;
    }

    public class InsertSqlSection : SqlInsqlSection
    {
        public InsertSqlSection(string id) : base(id)
        {
        }

        public override InsqlSectionType SectionType => InsqlSectionType.Insert;
    }

    public class UpdateSqlSection : SqlInsqlSection
    {
        public UpdateSqlSection(string id) : base(id)
        {
        }

        public override InsqlSectionType SectionType => InsqlSectionType.Update;
    }

    public class DeleteSqlSection : SqlInsqlSection
    {
        public DeleteSqlSection(string id) : base(id)
        {
        }

        public override InsqlSectionType SectionType => InsqlSectionType.Delete;
    }
}
