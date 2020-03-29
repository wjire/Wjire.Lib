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
        /// <param name="ignoreDisplayName">是否忽略 DisplayNameAttribute</param>
        /// <returns></returns>
        internal static ColumnInfo[] GetColumnInfos(Type sourceType, bool ignoreDisplayName = false)
        {
            //if (ColContainer.TryGetValue(sourceType, out ColumnInfo[] infos))
            //{
            //    return infos;
            //}
            IEnumerable<PropertyInfo> propertyInfos = sourceType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);
            ColumnInfo[] cols;
            if (ignoreDisplayName == false)
            {
                cols = propertyInfos
                    .Where(propertyInfo => propertyInfo.GetCustomAttribute<DisplayNameAttribute>(true) != null)
                    .Select(propertyInfo => new ColumnInfo
                    {
                        PropertyInfo = propertyInfo,
                        DisplayName = propertyInfo.GetCustomAttribute<DisplayNameAttribute>().DisplayName,
                    }).ToArray();
            }
            else
            {
                cols = propertyInfos
                    .Select(propertyInfo => new ColumnInfo
                    {
                        PropertyInfo = propertyInfo,
                        DisplayName = propertyInfo.Name,
                    }).ToArray();
            }
            return cols;
            //ColContainer.Add(sourceType, infos);
        }


        /// <summary>
        /// 获取需要导出的列信息
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="exportFields"></param>
        /// <param name="ignoreDisplayName"></param>
        /// <returns></returns>
        internal static ColumnInfo[] GetColumnInfos(Type sourceType, ICollection<string> exportFields, bool ignoreDisplayName = false)
        {
            IEnumerable<ColumnInfo> cols = GetColumnInfos(sourceType, ignoreDisplayName);
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
            ColumnInfo[] cols = GetColumnInfos(sourceType, exportFieldsWithName.Keys, true);
            //ColumnInfo[] newCols = cols.Select(s => new ColumnInfo
            //{
            //    PropertyInfo = s.PropertyInfo,
            //    DisplayName = exportFieldsWithName[s.PropertyInfo.Name],
            //}).ToArray();

            List<ColumnInfo> newCols = new List<ColumnInfo>(exportFieldsWithName.Count);
            foreach (KeyValuePair<string, string> keyValue in exportFieldsWithName)
            {
                ColumnInfo col = cols.First(f => f.DisplayName == keyValue.Key);
                newCols.Add(new ColumnInfo
                {
                    DisplayName = keyValue.Value,
                    PropertyInfo = col.PropertyInfo
                });
            }
            //ColumnInfo[] newCols = cols.Select(s => new ColumnInfo
            //{
            //    PropertyInfo = s.PropertyInfo,
            //    DisplayName = exportFieldsWithName[s.PropertyInfo.Name],
            //}).ToArray();
            return newCols.ToArray();
        }
    }
}
