using System;
using System.Collections;
using System.Linq;

namespace SqlXmlMapper.Resolvers.Elements
{
   public class EachSectionElement : IInsqlSectionElement
   {
      public string Name { get; }

      public string Separator { get; set; }

      public string Open { get; set; }

      public string Close { get; set; }

      public string Prefix { get; set; }

      public string Suffix { get; set; }

      public EachSectionElement(string name)
      {
         if (string.IsNullOrWhiteSpace(name))
         {
            throw new ArgumentNullException(nameof(name));
         }

         this.Name = name;
      }

      public string Resolve(ResolveContext context)
      {
         if (!context.Param.TryGetValue(this.Name, out object value))
         {
            throw new Exception($"ResolveContext Param : `{this.Name}` is not exists!");
         }

         if (value == null)
         {
            throw new Exception($"ResolveContext Param : `{this.Name}` is null!");
         }

         var valueList = (value as IEnumerable)?.Cast<object>().ToList();

         if (valueList == null)
         {
            throw new Exception($"ResolveContext Param : `{this.Name}` is not IEnumerable!");
         }

         //add new params
         for (var i = 0; i < valueList.Count; i++)
         {
            context.Param.Add($"{this.Name}{i + 1}", valueList[i]);
         }

         //remove original parameters
         context.Param.Remove(this.Name);

         //return 
         var contentString = string.Join(this.Separator, valueList.Select((itemv, itemi) => $"{this.Prefix}{this.Name}{itemi + 1}{this.Suffix}"));

         return $"{this.Open}{contentString}{this.Close}";
      }
   }
}
