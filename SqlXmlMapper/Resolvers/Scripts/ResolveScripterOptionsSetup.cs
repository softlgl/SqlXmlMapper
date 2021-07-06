using Microsoft.Extensions.Options;

namespace SqlXmlMapper.Resolvers.Scripts
{
    public class ResolveScripterOptionsSetup : IConfigureOptions<ResolveScripterOptions>
    {
        public void Configure(ResolveScripterOptions options)
        {
            options.IsConvertEnum = true;
            options.IsConvertOperator = true;
            options.IsConvertDateTimeMin = true;
        }
    }
}
