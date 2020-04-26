using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Wjire.Excel
{
    public class Excel2003ReadHandler : IReadHandler
    {
        private readonly IWorkbook _workbook;

        public Excel2003ReadHandler(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                _workbook = new HSSFWorkbook(fileStream); //03
            }
        }


        /// <summary>
        /// Excel => List
        /// </summary>
        /// <param name="columnMaps">需要读取的列及要转换成的对象字段名称</param>
        /// <param name="throwExceptionIfCellValueIsNull">当单元格无值时,是否抛出异常,默认true</param>
        /// <param name="sheetIndex">读取第几张sheet,默认第1张</param>
        /// <returns></returns>
        public List<T> Read<T>(Dictionary<int, string> columnMaps, bool throwExceptionIfCellValueIsNull = true, int sheetIndex = 1) where T : class, new()
        {
            ISheet sheet = _workbook.GetSheetAt(sheetIndex - 1);
            return Read<T>(columnMaps, throwExceptionIfCellValueIsNull, sheet);
        }

        private List<T> Read<T>(Dictionary<int, string> columnMaps, bool throwExceptionIfCellValueIsNull, ISheet sheet) where T : class, new()
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            //遍历每一行数据
            for (int i = 1, len = sheet.LastRowNum + 1; i < len; i++)
            {
                T t = new T();
                IRow row = sheet.GetRow(i);
                foreach (KeyValuePair<int, string> column in columnMaps)
                {
                    ICell cell = row.GetCell(column.Key - 1);
                    PropertyInfo property = type.GetProperty(column.Value);
                    if (property == null)
                    {
                        throw new Exception($"{type.Name} 类没有 {column.Value} 属性");
                    }
                    switch (cell)
                    {
                        case null when throwExceptionIfCellValueIsNull:
                            throw new Exception($"未读取到第{i}行,{column.Key}列单元格的值");
                        case null:
                            continue;
                        default:
                            {
                                object cellValue = ConvertCellValue(cell, property.PropertyType);
                                property.SetValue(t, cellValue);
                                break;
                            }
                    }
                }
                list.Add(t);
            }
            return list;
        }


        private object ConvertCellValue(ICell cell, Type type)
        {
            try
            {
                object result;
                switch (cell.CellType)
                {
                    case CellType.String: //文本
                    case CellType.Formula:
                        result = cell.StringCellValue;
                        break;
                    case CellType.Numeric: //数值
                        result = cell.NumericCellValue;
                        break;
                    case CellType.Boolean: //bool
                        result = cell.BooleanCellValue;
                        break;
                    case CellType.Blank: //空白
                        result = null;
                        break;
                    default:
                        result = "ERROR";
                        break;
                }

                result = ConvertHelper.Convert(result, type);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"读取第{cell.RowIndex + 1}行,{cell.ColumnIndex + 1}列时发生异常:" + ex);
            }
        }


        /// <summary>
        /// 获取一行的所有数据,并统一转换为 string ,因此可能会抛出异常 . rowIndex = 1 为第一行
        /// </summary>
        /// <param name="rowIndex">第 rowIndex 行</param>
        /// <param name="sheetIndex">第几张sheet</param>
        /// <returns></returns>
        public List<string> GetCells(int rowIndex, int sheetIndex = 1)
        {
            List<string> columns = new List<string>();
            ISheet sheet = _workbook.GetSheetAt(sheetIndex - 1);
            IRow row = sheet.GetRow(rowIndex - 1);
            for (int i = 0, len = row.LastCellNum; i < len; i++)
            {
                columns.Add(row.GetCell(i).StringCellValue);//这里没有考虑数据格式转换，会出现bug
            }
            return columns;
        }



        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _workbook?.Dispose();
        }
    }
}
