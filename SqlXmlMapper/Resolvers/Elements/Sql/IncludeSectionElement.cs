using System;

namespace SqlXmlMapper.Resolvers.Elements
{
    public class IncludeSectionElement : IInsqlSectionElement
    {
        public string RefId { get; }

        public IncludeSectionElement(string refid)
        {
            if (string.IsNullOrWhiteSpace(refid))
            {
                throw new ArgumentNullException(nameof(refid));
            }

            this.RefId = refid;
        }

        public string Resolve(ResolveContext context)
        {
            if (context.InsqlDescriptor.Sections.TryGetValue(this.RefId, out IInsqlSection insqlSection))
            {
                return insqlSection.Resolve(new ResolveContext
                {
                    ServiceProvider = context.ServiceProvider,
                    InsqlDescriptor = context.InsqlDescriptor,
                    InsqlSection = insqlSection,
                    Param = context.Param
                });
            }

            throw new ArgumentException($"insql section id : {this.RefId} not found !");
        }
    }
}
