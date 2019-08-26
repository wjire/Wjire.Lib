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

        private static readonly Dictionary<Type, ColumnInfo[]> ColContainer = new Dictionary<Type, ColumnInfo[]>();


        /// <summary>
        /// 获取数据源列信息
        /// </summary>
        /// <param name="sourceType">数据源类类型</param>
        /// <returns></returns>
        internal static ColumnInfo[] GetColumnInfos(Type sourceType)
        {
            if (ColContainer.TryGetValue(sourceType, out ColumnInfo[] infos))
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
            ColContainer.Add(sourceType, infos);
            return infos;
        }


        /// <summary>
        /// 获取需要导出的列信息
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="exportFieldsWithName"></param>
        /// <returns></returns>
        internal static ColumnInfo[] GetColumnInfos(Type sourceType, Dictionary<string, string> exportFieldsWithName)
        {
            ColumnInfo[] cols = GetColumnInfos(sourceType, exportFieldsWithName.Keys);
            ColumnInfo[] newCols = cols.Select(s => new ColumnInfo
            {
                PropertyInfo = s.PropertyInfo,
                DisplayName = exportFieldsWithName[s.PropertyInfo.Name]
            }).ToArray();
            return newCols;
        }


        /// <summary>
        /// 获取需要导出的列信息
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="exportFields"></param>
        /// <returns></returns>
        internal static ColumnInfo[] GetColumnInfos(Type sourceType, ICollection<string> exportFields)
        {
            ColumnInfo[] cols = GetColumnInfos(sourceType);
            if (exportFields?.Count > 0)
            {
                cols = cols.Where(w => exportFields.Contains(w.PropertyInfo.Name)).ToArray();
            }
            return cols;
        }

    }
}
