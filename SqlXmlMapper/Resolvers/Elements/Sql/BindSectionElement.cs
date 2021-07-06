using Microsoft.Extensions.DependencyInjection;
using System;

namespace SqlXmlMapper.Resolvers.Elements
{
    public class BindSectionElement : IInsqlSectionElement
    {
        private TypeCode valueType = TypeCode.Object;

        public string Name { get; }

        public string Value { get; }

        public string ValueType
        {
            get
            {
                return this.valueType.ToString();
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.valueType = TypeCode.Object; return;
                }

                if (!Enum.TryParse<TypeCode>(value, out this.valueType))
                {
                    throw new Exception($"insql bind section `valueType` not convert to `TypeCode`.");
                }
            }
        }

        public BindSectionElement(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.Name = name;
            this.Value = value;
        }

        public string Resolve(ResolveContext context)
        {
            var codeExecuter = context.ServiceProvider.GetRequiredService<IInsqlResolveScripter>();

            var executeResult = codeExecuter.Resolve(this.valueType, this.Value, context.Param);

            context.Param[this.Name] = executeResult;

            return string.Empty;
        }
    }
}
