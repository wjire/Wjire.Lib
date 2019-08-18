using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;


namespace Wjire.Excel
{

    /// <summary>
    /// 数据源列信息容器
    /// </summary>
    internal static class ColumnInfoContainer
    {

        private static readonly Dictionary<Type, ColumnInfo[]> Container = new Dictionary<Type, ColumnInfo[]>();



        /// <summary>
        /// 获取需要导出的列信息
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="exportFields"></param>
        /// <returns></returns>
        internal static ColumnInfo[] GetColumnInfos(Type sourceType, HashSet<string> exportFields)
        {
            ColumnInfo[] cols = GetColumnInfos(sourceType);
            if (exportFields?.Count > 0)
            {
                cols = cols.Where(w => exportFields.Contains(w.PropertyInfo.Name)).ToArray();
            }
            return cols;
        }



        /// <summary>
        /// 获取数据源列信息
        /// </summary>
        /// <param name="sourceType">数据源类类型</param>
        /// <returns></returns>
        private static ColumnInfo[] GetColumnInfos(Type sourceType)
        {
            if (Container.TryGetValue(sourceType, out ColumnInfo[] infos))
            {
                return infos;
            }

            infos = sourceType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(propertyInfo => propertyInfo.GetCustomAttribute<DisplayNameAttribute>(true) != null)
                .Select(propertyInfo => new ColumnInfo
                {
                    PropertyInfo = propertyInfo,
                    DisplayName = propertyInfo.GetCustomAttribute<DisplayNameAttribute>().DisplayName
                }).ToArray();
            Container.Add(sourceType, infos);
            return infos;
        }
    }
}
