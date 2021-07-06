using System;
using System.Collections.Generic;

namespace SqlXmlMapper.Resolvers.Matchers
{
    public class ResolveMatcher : IInsqlResolveMatcher
    {
        public IInsqlSection Match(InsqlDescriptor insqlDescriptor, string sqlId, IDictionary<string, object> sqlParam)
        {
            if (string.IsNullOrWhiteSpace(sqlId))
            {
                throw new ArgumentNullException(nameof(sqlId));
            }

            if (insqlDescriptor.Sections.TryGetValue(sqlId, out IInsqlSection insqlSection))
            {
                return insqlSection;
            }

            var lastIndex = sqlId.LastIndexOf('.');

            if (lastIndex > -1)
            {
                if (insqlDescriptor.Sections.TryGetValue(sqlId.Substring(0, lastIndex), out insqlSection))
                {
                    return insqlSection;
                }
            }

            return null;
        }
    }
}
