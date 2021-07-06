namespace SqlXmlMapper.Resolvers.Scripts
{
    public class ResolveScripterOptions
    {
        public bool IsConvertEnum { get; set; }

        public bool IsConvertOperator { get; set; }

        public bool IsConvertDateTimeMin { get; set; }

        public string[] ExcludeOperators { get; set; }
    }
}
