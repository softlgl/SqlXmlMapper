using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlXmlMapper.Resolvers.Elements
{
    public class IfSectionElement : IInsqlSectionElement
    {
        public string Test { get; }

        public List<IInsqlSectionElement> Children { get; }

        public IfSectionElement(string test)
        {
            if (string.IsNullOrWhiteSpace(test))
            {
                throw new ArgumentNullException(nameof(test));
            }

            this.Test = test;

            this.Children = new List<IInsqlSectionElement>();
        }

        public string Resolve(ResolveContext context)
        {
            var codeExecuter = context.ServiceProvider.GetRequiredService<IInsqlResolveScripter>();

            var isTest = (bool)codeExecuter.Resolve(TypeCode.Boolean, this.Test, context.Param);

            if (!isTest)
            {
                return string.Empty;
            }

            //parse
            var childrenResult = this.Children.Select(children =>
            {
                return children.Resolve(context);
            });

            return string.Join(" ", childrenResult).Trim();
        }
    }
}
