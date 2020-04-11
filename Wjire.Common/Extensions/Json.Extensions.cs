using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Wjire.Common
{
    /// <summary>
    /// Newtonsoft.Json序列化
    /// </summary>
    public static partial class ObjectExtensions
    {

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings()
        {
            DateFormatString = "yyyy-MM-dd HH:mm:ss"
        };

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">源数据</param>
        /// <param name="props">传入的属性数组</param>
        /// <param name="retain">true:表示props是需要保留的字段(默认)  false：表示props是要排除的字段</param>
        /// <returns></returns>
        public static string ToJson(this object obj, string[] props = null, bool retain = true)
        {
            //settings.NullValueHandling = NullValueHandling.Ignore;
            //settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            if (props == null)
            {
                return JsonConvert.SerializeObject(obj, Settings);
            }

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                ContractResolver = new LimitPropsContractResolver(props, retain)
            };
            return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object ToObject(this string json)
        {
            return JsonConvert.DeserializeObject(json);
        }


        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToObject(this string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }


        private class LimitPropsContractResolver : DefaultContractResolver
        {
            private readonly string[] _props;

            private readonly bool _retain;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="props">传入的属性数组</param>
            /// <param name="retain">true:表示props是需要保留的字段  false：表示props是要排除的字段</param>
            public LimitPropsContractResolver(string[] props, bool retain = true)
            {
                //指定要序列化属性的清单
                _props = props;

                _retain = retain;
            }

            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);

                //只序列化清单列出的属性
                if (_retain)
                {
                    return list.Where(p => _props.Contains(p.PropertyName, StringComparer.OrdinalIgnoreCase)).ToList();
                }
                //过滤掉清单列出的属性
                else
                {
                    return list.Where(p => !_props.Contains(p.PropertyName, StringComparer.OrdinalIgnoreCase)).ToList();
                }
            }
        }
    }
}