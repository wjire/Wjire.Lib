using System;
using System.Collections.Generic;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Wjire.Excel
{
    public class ExcelReadHandler
    {

        private readonly IWorkbook _workbook;

        public ExcelReadHandler(string fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                _workbook = CreateWorkbook(fileStream);
            }
        }


        public ExcelReadHandler(Stream stream)
        {
            _workbook = CreateWorkbook(stream);
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
        /// Excel中默认第一张Sheet导出到集合
        /// </summary>
        /// <param name="fields">Excel各个列，依次要转换成为的对象字段名称</param>
        /// <param name="sheetIndex">第几张sheet,默认第一张</param>
        /// <returns></returns>
        public List<T> Read<T>(string[] fields, int sheetIndex = 1) where T : class, new()
        {
            return ExportToList<T>(_workbook.GetSheetAt(sheetIndex - 1), fields);
        }


        /// <summary>
        /// Sheet中的数据转换为List集合,从第2行开始计算数据,第一行默认为标题.
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        private List<T> ExportToList<T>(ISheet sheet, string[] fields) where T : class, new()
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            //遍历每一行数据
            for (int i = sheet.FirstRowNum + 1, len = sheet.LastRowNum + 1; i < len; i++)
            {
                T t = new T();
                IRow row = sheet.GetRow(i);
                for (int j = 0, len2 = fields.Length; j < len2; j++)
                {

                    ICell cell = row.GetCell(j);
                    object cellValue = null;
                    try
                    {
                        switch (cell.CellType)
                        {
                            case CellType.String: //文本
                            case CellType.Formula:
                                cellValue = cell.StringCellValue;
                                break;
                            case CellType.Numeric: //数值
                                cellValue = cell.NumericCellValue.ToString();
                                break;
                            case CellType.Boolean: //bool
                                cellValue = cell.BooleanCellValue;
                                break;
                            case CellType.Blank: //空白
                                cellValue = "";
                                break;
                            default:
                                cellValue = "***Error Format***";
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        cellValue = "";
                        Console.WriteLine(e);
                    }
                    type.GetProperty(fields[j])?.SetValue(t, cellValue, null);

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
    }
}
