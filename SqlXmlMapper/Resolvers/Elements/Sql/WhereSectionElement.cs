namespace SqlXmlMapper.Resolvers.Elements
{
    /// <summary>
    /// where 元素只会在至少有一个子元素的条件返回 SQL 子句的情况下才去插入“WHERE”子句。而且，若语句的开头为“AND”或“OR”，where 元素也会将它们去除。
    /// </summary>
    public class WhereSectionElement : TrimSectionElement
    {
        public WhereSectionElement()
        {
            this.Prefix = "where ";
            this.PrefixOverrides = "and |or ";
        }
    }
}
