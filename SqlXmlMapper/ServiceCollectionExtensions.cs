using SqlXmlMapper.Resolvers;
using SqlXmlMapper.Resolvers.Matchers;
using SqlXmlMapper.Resolvers.Scripts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace SqlXmlMapper
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="mapperRoot">存放mapper的路径</param>
        /// <returns></returns>
        public static IServiceCollection AddSqlXmlMapper(this IServiceCollection services,params string[] mapperRoot)
        {
            if (!mapperRoot.Any())
            {
                throw new ArgumentNullException("mapper路径不能为空");
            }
            return services.AddOptions()
                .AddSingleton<IInsqlResolveScripter, ResolveScripter>()
                .AddSingleton<IConfigureOptions<ResolveScripterOptions>, ResolveScripterOptionsSetup>()
                .AddSingleton<IInsqlResolveMatcher, ResolveMatcher>()
                .AddSingleton(provider => new InsqlOptions(provider, mapperRoot));
        }
    }
}
