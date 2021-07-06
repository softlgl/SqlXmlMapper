namespace SqlXmlMapper.Resolvers.Elements
{
    /// <summary>
    /// set 元素只会在至少有一个子元素的条件返回 SQL 子句的情况下才去插入“SET”子句。而且，若语句的结尾为“,”，set 元素也会将它们去除。
    /// </summary>
    public class SetSectionElement : TrimSectionElement
    {
        public SetSectionElement()
        {
            this.Prefix = "set ";
            this.SuffixOverrides = ",";
        }
    }
}
