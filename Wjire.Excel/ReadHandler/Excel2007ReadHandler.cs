using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace Wjire.Excel
{
    public class Excel2007ReadHandler : BaseReadHandler,IReadHandler
    {

        private readonly ExcelPackage _package;

        public Excel2007ReadHandler(string filePath)
        {
            FileInfo existingFile = new FileInfo(filePath);
            _package = new ExcelPackage(existingFile);
        }


        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _package?.Dispose();
        }


        /// <summary>
        /// Excel => List
        /// </summary>
        /// <param name="throwExceptionIfCellValueIsNull">当单元格无值时,是否抛出异常,默认 false</param>
        /// <param name="sheetIndex">读取第几张sheet,默认第1张</param>
        /// <returns></returns>
        public List<T> Read<T>(bool throwExceptionIfCellValueIsNull = false, int sheetIndex = 1) where T : class, new()
        {
            var columnMaps = GetColumnMaps(typeof(T));
            return Read<T>(columnMaps, throwExceptionIfCellValueIsNull, sheetIndex);
        }


        /// <summary>
        /// Excel => List
        /// </summary>
        /// <param name="columnMaps">需要读取的列及要转换成的对象字段名称</param>
        /// <param name="throwExceptionIfCellValueIsNull">当单元格无值时,是否抛出异常,默认 false</param>
        /// <param name="sheetIndex">读取第几张sheet,默认第1张</param>
        /// <returns></returns>
        public List<T> Read<T>(IDictionary<int, string> columnMaps, bool throwExceptionIfCellValueIsNull = false, int sheetIndex = 1) where T : class, new()
        {
            ExcelWorksheet sheet = _package.Workbook.Worksheets[sheetIndex - 1];
            return Read<T>(columnMaps, throwExceptionIfCellValueIsNull, sheet);
        }
      

        private List<T> Read<T>(IDictionary<int, string> columnMaps, bool throwExceptionIfCellValueIsNull, ExcelWorksheet sheet)
            where T : class, new()
        {
            List<T> result = new List<T>();
            Type type = typeof(T);
            for (int m = sheet.Dimension.Start.Row + 1, n = sheet.Dimension.End.Row; m <= n; m++)
            {
                T t = new T();
                foreach (KeyValuePair<int, string> column in columnMaps)
                {
                    object obj = sheet.GetValue(m, column.Key);
                    System.Reflection.PropertyInfo property = type.GetProperty(column.Value);
                    if (property == null)
                    {
                        throw new Exception($"{type.Name} 类没有 {column.Value} 属性");
                    }
                    switch (obj)
                    {
                        case null when throwExceptionIfCellValueIsNull:
                            throw new Exception($"未读取到第{m}行,{column.Key}列单元格的值");
                        case null:
                            continue;
                        default:
                            {
                                property.SetValue(t, ConvertHelper.Convert(obj, property.PropertyType));
                                break;
                            }
                    }
                }
                result.Add(t);
            }
            return result;
        }


        /// <summary>
        /// 获取一行的所有数据,并统一转换为 string ,因此可能会抛出异常 . rowIndex = 1 为第一行
        /// </summary>
        /// <param name="rowIndex">第 rowIndex 行</param>
        /// <param name="sheetIndex">第几张sheet</param>
        /// <returns></returns>
        public List<string> GetCells(int rowIndex, int sheetIndex = 1)
        {
            ExcelWorksheet sheet = _package.Workbook.Worksheets[sheetIndex - 1];
            List<string> columns = sheet.Cells[rowIndex, 1, rowIndex, sheet.Dimension.End.Column].Select(cell => cell.GetValue<string>()).ToList();
            return columns;
        }
    }
}
