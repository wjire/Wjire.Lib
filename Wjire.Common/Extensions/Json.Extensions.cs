﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Wjire.Common
{
    /// <summary>
    /// Newtonsoft.Json序列化
    /// </summary>
    public static class JsonExtension
    {

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">源数据</param>
        /// <param name="props">传入的属性数组</param>
        /// <param name="retain">true:表示props是需要保留的字段(默认)  false：表示props是要排除的字段</param>
        /// <returns></returns>
        public static string SerializeObject(this object obj, string[] props = null, bool retain = true)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };
            //settings.NullValueHandling = NullValueHandling.Ignore;
            //settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            if (props == null)
            {
                return JsonConvert.SerializeObject(obj, settings);
            }
            settings.ContractResolver = new LimitPropsContractResolver(props, retain);
            return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object DeserializeObject(this string json)
        {
            return JsonConvert.DeserializeObject(json);
        }


        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DeserializeObject(this string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }


        private class LimitPropsContractResolver : DefaultContractResolver
        {
            private readonly string[] props;

            private readonly bool retain;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="props">传入的属性数组</param>
            /// <param name="retain">true:表示props是需要保留的字段  false：表示props是要排除的字段</param>
            public LimitPropsContractResolver(string[] props, bool retain = true)
            {
                //指定要序列化属性的清单
                this.props = props;

                this.retain = retain;
            }

            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);

                //只序列化清单列出的属性
                if (retain)
                {
                    return list.Where(p => props.Contains(p.PropertyName, StringComparer.OrdinalIgnoreCase)).ToList();
                }
                //过滤掉清单列出的属性
                else
                {
                    return list.Where(p => !props.Contains(p.PropertyName, StringComparer.OrdinalIgnoreCase)).ToList();
                }
            }
        }
    }
}