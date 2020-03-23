using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Reflection;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Wjire.Excel
{
    public class ExcelReadHandler
    {

        private readonly IWorkbook _workbook;

        public ExcelReadHandler(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                _workbook = CreateWorkbook(fileStream);
            }
        }


        public ExcelReadHandler(Stream stream)
        {
            _workbook = CreateWorkbook(stream);
            stream.Dispose();
        }


        /// <summary>
        /// 创建工作簿对象
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private IWorkbook CreateWorkbook(Stream stream)
        {
            try
            {
                return new XSSFWorkbook(stream); //07
            }
            catch
            {
                return new HSSFWorkbook(stream); //03
            }
        }


        /// <summary>
        /// Excel => List
        /// </summary>
        /// <param name="fields">Excel各个列，依次要转换成为的对象字段名称</param>
        /// <param name="sheetIndex">读取第几张sheet,默认第1张</param>
        /// <param name="rowIndex">从第几行开始读取,默认第2行,第1行为标题</param>
        /// <returns></returns>
        public List<T> Read<T>(string[] fields, int sheetIndex = 1, int rowIndex = 2) where T : class, new()
        {
            return ReadFrom<T>(fields, _workbook.GetSheetAt(sheetIndex - 1), rowIndex - 1);
        }



        /// <summary>
        /// Excel => List
        /// </summary>
        /// <param name="useCustomOrder">是否使用自定义顺序</param>
        /// <param name="sheetIndex">读取第几张sheet,默认第1张</param>
        /// <param name="rowIndex">从第几行开始读取,默认第2行,第1行为标题</param>
        /// <returns></returns>
        public List<T> Read<T>(bool useCustomOrder = false, int sheetIndex = 1, int rowIndex = 2) where T : class, new()
        {
            Type type = typeof(T);
            Dictionary<int, string> columnMaps = useCustomOrder ? GetReadingColumnsByCustomOrder(type) : GetReadingColumns(type);
            return ReadFrom<T>(columnMaps, _workbook.GetSheetAt(sheetIndex - 1), rowIndex - 1);
        }


        /// <summary>
        /// Excel => List
        /// </summary>
        /// <param name="columnMaps">需要读取的列及要转换成为的对象字段名称</param>
        /// <param name="sheetIndex">读取第几张sheet,默认第1张</param>
        /// <param name="rowIndex">从第几行开始读取,默认第2行,第1行为标题</param>
        /// <returns></returns>
        public List<T> Read<T>(Dictionary<int, string> columnMaps, int sheetIndex = 1, int rowIndex = 2) where T : class, new()
        {
            return ReadFrom<T>(columnMaps, _workbook.GetSheetAt(sheetIndex - 1), rowIndex - 1);
        }



        private List<T> ReadFrom<T>(Dictionary<int, string> columnMaps, ISheet sheet, int rowIndex) where T : class, new()
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            var pros = type.GetProperties();
            //遍历每一行数据
            for (int i = rowIndex, len = sheet.LastRowNum + 1; i < len; i++)
            {
                T t = new T();
                IRow row = sheet.GetRow(i);
                foreach (KeyValuePair<int, string> column in columnMaps)
                {
                    ICell cell = row.GetCell(column.Key - 1);
                    var pro = type.GetProperty(column.Value);
                    object cellValue = ConvertCellValue(cell, pro.PropertyType);
                    pro.SetValue(t, cellValue);
                }
                list.Add(t);
            }
            return list;
        }


        private List<T> ReadFrom<T>(string[] fields, ISheet sheet, int rowIndex) where T : class, new()
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            //遍历每一行数据
            for (int i = rowIndex, len = sheet.LastRowNum + 1; i < len; i++)
            {
                T t = new T();
                IRow row = sheet.GetRow(i);
                for (int j = 0, len2 = fields.Length; j < len2; j++)
                {
                    ICell cell = row.GetCell(j);
                    var pro = type.GetProperty(fields[j]);
                    object cellValue = ConvertCellValue(cell, pro.PropertyType);
                    pro.SetValue(t, cellValue);
                }
                list.Add(t);
            }
            return list;
        }


        /// <summary>
        /// 默认获取第一个Sheet的第 rowIndex 行，第 columnIndex 列的值。起始均为为1
        /// </summary>
        /// <param name="rowIndex">行</param>
        /// <param name="columnIndex">列</param>
        /// <param name="sheetIndex">第几张sheet</param>
        /// <returns></returns>
        public string GetCellValue(int rowIndex, int columnIndex, int sheetIndex = 1)
        {
            ISheet sheet = _workbook.GetSheetAt(sheetIndex - 1);
            IRow row = sheet.GetRow(rowIndex - 1);
            return row.GetCell(columnIndex - 1).ToString();
        }


        /// <summary>
        /// 获取一行的所有数据,并统一转换为 string ,因此可能会抛出异常 . rowIndex = 1 为第一行
        /// </summary>
        /// <param name="rowIndex">第 rowIndex 行</param>
        /// <param name="sheetIndex">第几张sheet</param>
        /// <returns></returns>
        public string[] GetCells(int rowIndex, int sheetIndex = 1)
        {
            List<string> list = new List<string>();
            ISheet sheet = _workbook.GetSheetAt(sheetIndex - 1);
            IRow row = sheet.GetRow(rowIndex - 1);
            for (int i = 0, len = row.LastCellNum; i < len; i++)
            {
                list.Add(row.GetCell(i).StringCellValue);//这里没有考虑数据格式转换，会出现bug
            }
            return list.ToArray();
        }



        private object ConvertCellValue(ICell cell, Type type)
        {
            if (cell == null)
            {
                throw new ArgumentNullException("未读取到单元格数据,可能是有隐藏的单元格");
            }
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

                result = Convert(result, type);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"读取第{cell.RowIndex + 1}行,第{cell.ColumnIndex + 1}列时发生异常:" + ex);
            }
        }

        private object ConvertCellValue2(ICell cell)
        {
            if (cell == null)
            {
                throw new ArgumentNullException("未读取到单元格数据,可能是有隐藏的单元格");
            }

            try
            {
                switch (cell.CellType)
                {
                    case CellType.String: //文本
                    case CellType.Formula:
                        return cell.StringCellValue;
                    case CellType.Numeric: //数值
                        //return cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
                        return cell.NumericCellValue;
                    case CellType.Boolean: //bool
                        return cell.BooleanCellValue;
                    case CellType.Blank: //空白
                        return null;
                    default:
                        return "ERROR";
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"读取第{cell.RowIndex + 1}行,第{cell.ColumnIndex + 1}时发生异常:" + ex);
            }
        }



        private Dictionary<int, string> GetReadingColumnsByCustomOrder(Type type)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            foreach (PropertyInfo info in type.GetProperties())
            {
                DisplayAttribute displayAttribute = info.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute == null)
                {
                    throw new Exception($"{info.Name} 属性未定义 DisplayAttribute");
                }
                result.Add(displayAttribute.Order, info.Name);
            }
            return result;
        }


        private Dictionary<int, string> GetReadingColumns(Type type)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            PropertyInfo[] pros = type.GetProperties();
            for (int i = 0; i < pros.Length; i++)
            {
                result.Add(i + 1, pros[i].Name);
            }
            return result;
        }

        public object Convert(object value, Type conversionType)
        {
            Type underlyingType = Nullable.GetUnderlyingType(conversionType);
            if (underlyingType != null)
            {
                if (value == DBNull.Value)
                {
                    return null;
                }

                if (underlyingType.IsEnum)
                {
                    value = Enum.Parse(underlyingType, value.ToString());
                }

                return System.Convert.ChangeType(value, underlyingType);
            }
            if (conversionType.IsEnum)
            {
                return Enum.Parse(conversionType, value.ToString());
            }

            if (value==null && typeof(ValueType) .IsAssignableFrom(conversionType))
            {
                return Activator.CreateInstance(conversionType);
            }

            return System.Convert.ChangeType(value, conversionType);
        }
    }
}
