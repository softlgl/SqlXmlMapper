using System;
using System.Data;
using System.Text.RegularExpressions;

namespace SqlXmlMapper.Resolvers.Elements
{
   public class TextSectionElement : IInsqlSectionElement
   {
      private static Regex rawRegex = new Regex(@"\$\{\s*(\w+)\s*\}");

      public string Text { get; }

      public TextSectionElement(string text)
      {
         this.Text = text;
      }

      public string Resolve(ResolveContext context)
      {
         if (string.IsNullOrWhiteSpace(this.Text))
         {
            return string.Empty;
         }

         //支持 ${} 原始值
         return rawRegex.Replace(this.Text.Trim(), (match) =>
         {
            if (!match.Success || match.Groups.Count < 2)
            {
               return match.Value;
            }

            var pname = match.Groups[1].Value;

            if (!context.Param.TryGetValue(pname, out object pvalue))
            {
               return match.Value;
            }

            if (pvalue == null)
            {
               return string.Empty;
            }

            if (pvalue is IDataParameter dataParameter)
            {
               return dataParameter.Value?.ToString();
            }

            return pvalue?.ToString();
         });
      }
   }
}
