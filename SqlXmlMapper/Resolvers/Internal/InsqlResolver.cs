using System;
using System.Collections.Generic;

namespace SqlXmlMapper.Resolvers
{
    public class InsqlResolver : IInsqlResolver
    {
        public readonly InsqlDescriptor InsqlDescriptor;
        private readonly IServiceProvider serviceProvider;
        private readonly IInsqlResolveMatcher resolveMatcher;

        public InsqlResolver(InsqlDescriptor insqlDescriptor, IServiceProvider serviceProvider)
        {
            this.InsqlDescriptor = insqlDescriptor;
            this.serviceProvider = serviceProvider;
            this.resolveMatcher = serviceProvider.GetService(typeof(IInsqlResolveMatcher)) as IInsqlResolveMatcher;
        }

        public ResolveResult Resolve(string sqlId, IDictionary<string, object> sqlParam)
        {
            if (string.IsNullOrWhiteSpace(sqlId))
            {
                throw new ArgumentNullException(nameof(sqlId));
            }

            if (sqlParam == null)
            {
                sqlParam = new Dictionary<string, object>();
            }

            var insqlSection = this.resolveMatcher.Match(this.InsqlDescriptor, sqlId, sqlParam);

            if (insqlSection == null)
            {
                throw new Exception($"insql `{sqlId}` section not found!");
            }

            var resolveContext = new ResolveContext
            {
                ServiceProvider = this.serviceProvider,
                InsqlDescriptor = this.InsqlDescriptor,
                InsqlSection = insqlSection,
                Param = sqlParam,
            };

            var resolveResult = new ResolveResult
            {
                Sql = insqlSection.Resolve(resolveContext),
                Param = resolveContext.Param
            };

            return resolveResult;
        }
    }
}
