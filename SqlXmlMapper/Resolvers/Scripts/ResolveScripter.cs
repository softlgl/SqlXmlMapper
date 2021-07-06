using Jint;
using Jint.Native;
using Jint.Native.Object;
using Jint.Runtime.Interop;
using Jint.Runtime.References;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace SqlXmlMapper.Resolvers.Scripts
{
    public class ResolveScripter : IInsqlResolveScripter
    {
        private readonly Regex excludeRegex;
        private readonly Regex operatorRegex;
        private readonly ConcurrentDictionary<string, string> codeCaches;
        private readonly Dictionary<string, string> operatorMappings;

        private readonly IOptions<ResolveScripterOptions> options;
        private readonly ThreadLocal<Engine> localEngine;

        public ResolveScripter(IOptions<ResolveScripterOptions> options)
        {
            this.options = options;

            this.codeCaches = new ConcurrentDictionary<string, string>();

            this.operatorMappings = this.CreateOperatorMappings();

            this.excludeRegex = new Regex("(['\"]).*?[^\\\\]\\1");
            this.operatorRegex = new Regex($"\\s+({string.Join("|", this.operatorMappings.Keys)})\\s+");

            this.localEngine = new ThreadLocal<Engine>(this.CreateEngine);
        }

        public void Dispose()
        {
            this.localEngine.Dispose();
        }

        public object Resolve(TypeCode type, string code, IDictionary<string, object> param)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var optionsValue = this.options.Value;

            if (optionsValue.IsConvertOperator)
            {
                code = this.codeCaches.GetOrAdd(code, (key) => this.ConvertOperator(code));
            }

            var engine = localEngine.Value;

            foreach (var item in param)
            {
                if (item.Value is IDataParameter dataParameter)
                {
                    if (dataParameter == null)
                    {
                        engine.SetValue(item.Key, (object)null);
                    }
                    else if (dataParameter.Direction == ParameterDirection.Input || dataParameter.Direction == ParameterDirection.InputOutput)
                    {
                        engine.SetValue(item.Key, dataParameter.Value);
                    }
                }
                else
                {
                    engine.SetValue(item.Key, item.Value);
                }
            };

            engine.Execute(code);

            var value = engine.GetCompletionValue();

            var result = value.ToObject();

            foreach (var item in param)
            {
                engine.Global.Delete(item.Key, false);
            };

            if (result == null)
            {
                return null;
            }

            return Convert.ChangeType(result, type);
        }

        private Engine CreateEngine()
        {
            var optionsValue = this.options.Value;

            var dateTimeConverter = optionsValue.IsConvertDateTimeMin ? new ScriptDateTimeConverter() : null;

            var engine = new Engine(engineOptions =>
            {
                engineOptions.DebugMode(false);
                engineOptions.AllowDebuggerStatement(false);
                engineOptions.SetReferencesResolver(new NullPropagationReferenceResolver());

                if (optionsValue.IsConvertEnum)
                {
                    engineOptions.AddObjectConverter(ScriptEnumConverter.Instance);
                }
                if (optionsValue.IsConvertDateTimeMin)
                {
                    engineOptions.AddObjectConverter(dateTimeConverter);
                }
            });

            if (optionsValue.IsConvertDateTimeMin)
            {
                dateTimeConverter.SetEngine(engine);
            }

            return engine;
        }

        private Dictionary<string, string> CreateOperatorMappings()
        {
            var optionsValue = this.options.Value;

            var operatorMappings = new Dictionary<string, string>
            {
                { "and","&&" },
                { "or","||" },
                { "gt",">" },
                { "gte",">=" },
                { "lt","<" },
                { "lte","<=" },
                { "eq","==" },
                { "neq","!=" },
            };

            if (optionsValue.ExcludeOperators != null && optionsValue.ExcludeOperators.Length > 0)
            {
                foreach (var item in optionsValue.ExcludeOperators)
                {
                    if (operatorMappings.ContainsKey(item))
                    {
                        operatorMappings.Remove(item);
                    }
                }
            }

            return operatorMappings;
        }

        private string ConvertOperator(string code)
        {
            var excludeMatchs = excludeRegex.Matches(code);

            return this.operatorRegex.Replace(code, match =>
            {
                if (!match.Success)
                {
                    return match.Value;
                }

                var endIndex = match.Index + match.Length - 1;

                if (excludeMatchs.Cast<Match>().Any(cmatch =>
                {
                    var cendIndex = cmatch.Index + cmatch.Length - 1;

                    return match.Index > cmatch.Index && endIndex < cendIndex;
                }))
                {
                    return match.Value;
                }

                var operatorGroup = match.Groups.Count > 1 ? match.Groups[1] : null;

                if (operatorGroup == null || !operatorGroup.Success)
                {
                    return match.Value;
                }

                if (!this.operatorMappings.TryGetValue(operatorGroup.Value, out string operatorValue))
                {
                    return match.Value;
                }

                return $"{match.Value.Substring(0, operatorGroup.Index - match.Index)}" +
                    $"{operatorValue}" +
                    $"{match.Value.Substring(operatorGroup.Index - match.Index + operatorGroup.Length)}";
            });
        }
    }

    public class NullPropagationReferenceResolver : IReferenceResolver
    {
        public bool TryUnresolvableReference(Engine engine, Reference reference, out JsValue value)
        {
            value = reference.GetBase();
            return true;
        }

        public bool TryPropertyReference(Engine engine, Reference reference, ref JsValue value)
        {
            return value.IsNull() || value.IsUndefined();
        }

        public bool TryGetCallable(Engine engine, object reference, out JsValue value)
        {
            value = new ClrFunctionInstance(engine, (thisObj, values) => thisObj);
            return true;
        }

        public bool CheckCoercible(JsValue value)
        {
            return true;
        }
    }
}
