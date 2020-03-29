using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;


namespace Wjire.Excel
{

    /// <summary>
    /// 数据源列信息容器
    /// </summary>
    internal static class ColumnInfoContainer
    {
        private static readonly Dictionary<Type, PropertyInfo[]> PropertyInfoContainer = new Dictionary<Type, PropertyInfo[]>();
        private static readonly Dictionary<Type, ColumnInfo[]> ColumnInfosMap = new Dictionary<Type, ColumnInfo[]>();

        /// <summary>
        /// 获取数据源列信息
        /// </summary>
        /// <param name="sourceType">数据源类类型</param>
        /// <returns></returns>
        internal static ColumnInfo[] GetColumnInfos(Type sourceType)
        {
            if (ColumnInfosMap.TryGetValue(sourceType, out ColumnInfo[] columnInfos))
            {
                return columnInfos;
            }

            PropertyInfo[] properties = sourceType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfoContainer.Add(sourceType, properties);

            columnInfos = properties
                .Where(propertyInfo => propertyInfo.GetCustomAttribute<DisplayAttribute>(true) != null)
                .Select(propertyInfo =>
                {
                    DisplayAttribute displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                    ColumnInfo col = new ColumnInfo
                    {
                        PropertyInfo = propertyInfo,
                        DisplayName = string.IsNullOrWhiteSpace(displayAttribute.Name) ? propertyInfo.Name : displayAttribute.Name,
                        Order = displayAttribute.Order
                    };
                    return col;
                })
                .OrderBy(o => o.Order)
                .ToArray();
            ColumnInfosMap.Add(sourceType, columnInfos);
            return columnInfos;
        }


        /// <summary>
        /// 获取需要导出的列信息
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="exportFields"></param>
        /// <returns></returns>
        internal static ColumnInfo[] GetColumnInfos(Type sourceType, ICollection<string> exportFields)
        {
            IEnumerable<ColumnInfo> cols = GetColumnInfos(sourceType);
            if (exportFields?.Count > 0)
            {
                cols = cols.Where(w => exportFields.Contains(w.PropertyInfo.Name));
            }
            return cols.ToArray();
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
            foreach (ColumnInfo col in cols)
            {
                col.DisplayName = exportFieldsWithName[col.PropertyInfo.Name];
            }
            return cols;
        }
    }
}
