using SqlXmlMapper.Providers;
using SqlXmlMapper.Resolvers;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SqlXmlMapper
{
    public class InsqlOptions
    {
        private readonly ConcurrentDictionary<string, InsqlResolver> InsqlResolverDic = new ConcurrentDictionary<string, InsqlResolver>();

        public InsqlOptions(IServiceProvider serviceProvider,params string[] xmlMapperPath)
        {
            InitialMapper(serviceProvider, xmlMapperPath);
        }

        /// <summary>
        /// 初始化mapper
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="xmlMapperPath"></param>
        private void InitialMapper(IServiceProvider serviceProvider,params string[] xmlMapperPath)
        {
            List<FileInfo> fileInfos = new List<FileInfo>();
            foreach (var item in xmlMapperPath)
            {
                if (!Directory.Exists(item))
                {
                    throw new DirectoryNotFoundException($"路径{xmlMapperPath}不存在");
                }
                DirectoryInfo directoryInfo = new DirectoryInfo(item);
                fileInfos.AddRange(directoryInfo.GetFiles());
            }
            foreach (var fileInfo in fileInfos)
            {
                if (fileInfo.Extension != ".xml")
                {
                    continue;
                }

                InsqlResolver insqlResolver = GetInsqlResolver(serviceProvider, fileInfo);
                string fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                if (!fileName.StartsWith(insqlResolver.InsqlDescriptor.TypeFullName))
                {
                    throw new Exception($"文件名{fileName}需要以{insqlResolver.InsqlDescriptor.TypeFullName}开头");
                }
                InsqlResolver existsResolver = InsqlResolverDic.GetOrAdd(insqlResolver.InsqlDescriptor.TypeFullName, insqlResolver);
                if (existsResolver != insqlResolver)
                {
                    foreach (var section in insqlResolver.InsqlDescriptor.Sections)
                    {
                        if (existsResolver.InsqlDescriptor.Sections.ContainsKey(section.Key))
                        {
                            throw new Exception($"命名空间{insqlResolver.InsqlDescriptor.TypeFullName}已存在id=[{section.Key}]的节点");
                        }
                        existsResolver.InsqlDescriptor.Sections.Add(section.Key, section.Value);
                    }
                }

                IFileProvider fileProvider = new PhysicalFileProvider(fileInfo.Directory.FullName);
                ChangeToken.OnChange(() => fileProvider.Watch(fileInfo.Name), () => {
                    InsqlResolver changeInsqlResolver = GetInsqlResolver(serviceProvider, fileInfo);
                    InsqlResolverDic.AddOrUpdate(changeInsqlResolver.InsqlDescriptor.TypeFullName, changeInsqlResolver, (key, oldValue) => {
                        return changeInsqlResolver;
                    });
                });
            }
        }

        /// <summary>
        /// 获取InsqlResolver
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public InsqlResolver GetInsqlResolver<T>()
        {
            return GetInsqlResolver(typeof(T));
        }

        /// <summary>
        /// 获取InsqlResolver
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public InsqlResolver GetInsqlResolver(Type type)
        {
            return GetInsqlResolver(type.FullName);
        }

        /// <summary>
        /// 获取InsqlResolver
        /// </summary>
        /// <param name="typeName">命名空间</param>
        /// <returns></returns>
        public InsqlResolver GetInsqlResolver(string typeName)
        {
            InsqlResolverDic.TryGetValue(typeName, out InsqlResolver insqlResolver);
            return insqlResolver;
        }

        /// <summary>
        /// 获取ResolveResult
        /// </summary>
        /// <typeparam name="T">命名空间</typeparam>
        /// <param name="sqlId">sqlId</param>
        /// <returns></returns>
        public ResolveResult GetResolveResult<T>(string sqlId)
        {
            return GetResolveResult(typeof(T), sqlId);
        }

        /// <summary>
        /// 获取ResolveResult
        /// </summary>
        /// <param name="type">命名空间类型</param>
        /// <param name="sqlId">sqlId</param>
        /// <returns></returns>
        public ResolveResult GetResolveResult(Type type, string sqlId)
        {
            return GetInsqlResolver(type).Resolve(sqlId);
        }

        /// <summary>
        /// 获取ResolveResult
        /// </summary>
        /// <param name="typeName">命名空间</param>
        /// <param name="sqlId">sqlId</param>
        /// <returns></returns>
        public ResolveResult GetResolveResult(string typeName, string sqlId)
        {
            return GetInsqlResolver(typeName).Resolve(sqlId);
        }

        /// <summary>
        /// 获取ResolveResult
        /// </summary>
        /// <typeparam name="T">命名空间</typeparam>
        /// <param name="sqlId">sqlId</param>
        /// <returns></returns>
        public ResolveResult GetResolveResult<T>(string sqlId, object sqlParam)
        {
            return GetResolveResult(typeof(T), sqlId, sqlParam);
        }

        /// <summary>
        /// 获取ResolveResult
        /// </summary>
        /// <param name="type">命名空间类型</param>
        /// <param name="sqlId">sqlId</param>
        /// <returns></returns>
        public ResolveResult GetResolveResult(Type type, string sqlId, object sqlParam)
        {
            return GetInsqlResolver(type).Resolve(sqlId, sqlParam);
        }

        /// <summary>
        /// 获取ResolveResult
        /// </summary>
        /// <param name="typeName">命名空间</param>
        /// <param name="sqlId">sqlId</param>
        /// <returns></returns>
        public ResolveResult GetResolveResult(string typeName, string sqlId, object sqlParam)
        {
            return GetInsqlResolver(typeName).Resolve(sqlId, sqlParam);
        }

        private InsqlResolver GetInsqlResolver(IServiceProvider serviceProvider, FileInfo fileInfo)
        {
            var xmlStream = File.OpenRead(fileInfo.FullName);
            InsqlDescriptor insqlDescriptor = InsqlDescriptorXmlParser.Instance.ParseDescriptor(xmlStream, "");
            return new InsqlResolver(insqlDescriptor, serviceProvider);
        }
    }
}
