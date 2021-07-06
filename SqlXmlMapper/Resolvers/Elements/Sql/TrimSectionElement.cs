using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlXmlMapper.Resolvers.Elements
{
    public class TrimSectionElement : IInsqlSectionElement
    {
        public string Prefix { get; set; }

        public string Suffix { get; set; }

        public string PrefixOverrides { get; set; }

        public string SuffixOverrides { get; set; }

        public List<IInsqlSectionElement> Children { get; }

        public TrimSectionElement()
        {
            this.Children = new List<IInsqlSectionElement>();
        }

        public string Resolve(ResolveContext context)
        {
            var childrenResult = this.Children.Select(children =>
            {
                return children.Resolve(context);
            });

            var resultString = string.Join(" ", childrenResult).Trim();

            //如果没有可用子元素，则返回空字符串
            if (string.IsNullOrWhiteSpace(resultString))
            {
                return string.Empty;
            }

            //判断是否存在前缀覆盖
            if (!string.IsNullOrWhiteSpace(this.PrefixOverrides))
            {
                var overrides = this.PrefixOverrides.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in overrides)
                {
                    if (string.IsNullOrWhiteSpace(item))
                    {
                        continue;
                    }

                    if (resultString.StartsWith(item, StringComparison.OrdinalIgnoreCase))
                    {
                        resultString = resultString.Substring(item.Length);

                        break;
                    }
                }
            }

            //判断是否存在后缀覆盖
            if (!string.IsNullOrWhiteSpace(this.SuffixOverrides))
            {
                var overrides = this.SuffixOverrides.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in overrides)
                {
                    if (string.IsNullOrWhiteSpace(item))
                    {
                        continue;
                    }

                    if (resultString.EndsWith(item, StringComparison.OrdinalIgnoreCase))
                    {
                        resultString = resultString.Substring(0, resultString.Length - item.Length);

                        break;
                    }
                }
            }

            return $"{this.Prefix}{resultString}{this.Suffix}";
        }
    }
}
