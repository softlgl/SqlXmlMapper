using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SqlXmlMapper.Resolvers.Elements
{
   public class IfNotEmptySectionElement : IInsqlSectionElement
   {
      public string Name { get; }

      public List<IInsqlSectionElement> Children { get; }

      public IfNotEmptySectionElement(string name)
      {
         if (string.IsNullOrWhiteSpace(name))
         {
            throw new ArgumentNullException(nameof(name));
         }

         this.Name = name;

         this.Children = new List<IInsqlSectionElement>();
      }

      public string Resolve(ResolveContext context)
      {
         if (!context.Param.TryGetValue(this.Name, out object value))
         {
            return string.Empty;
         }

         if (value == null)
         {
            return string.Empty;
         }

         if (value is IDataParameter dataParameter)
         {
            if (string.IsNullOrWhiteSpace((string)dataParameter.Value))
            {
               return string.Empty;
            }
         }
         else if (string.IsNullOrWhiteSpace((string)value))
         {
            return string.Empty;
         }

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

         return resultString;
      }
   }
}
