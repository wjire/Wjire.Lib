using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using OfficeOpenXml;

namespace Wjire.Excel
{

    internal class Excel2007WriteHandler : IWriteHandler
    {

        public MemoryStream CreateMemoryStream<T>(IEnumerable<T> sources)
        {
            using (ExcelPackage pck = CreateExcelPackage(sources))
            {
                return pck.Stream as MemoryStream;
            }
        }


        public MemoryStream CreateMemoryStream<T>(IEnumerable<T> sources, ICollection<string> exportFields)
        {
            using (ExcelPackage pck = CreateExcelPackage(sources, exportFields))
            {
                return pck.Stream as MemoryStream;
            }
        }


        public MemoryStream CreateMemoryStream<T>(IEnumerable<T> sources, Dictionary<string, string> exportFieldsWithName)
        {
            using (ExcelPackage pck = CreateExcelPackage(sources, exportFieldsWithName))
            {
                return pck.Stream as MemoryStream;
            }
        }




        public byte[] CreateBytes<T>(IEnumerable<T> sources)
        {
            using (ExcelPackage pck = CreateExcelPackage(sources))
            {
                return pck.GetAsByteArray();
            }
        }


        public byte[] CreateBytes<T>(IEnumerable<T> sources, ICollection<string> exportFields)
        {
            using (ExcelPackage pck = CreateExcelPackage(sources, exportFields))
            {
                return pck.GetAsByteArray();
            }
        }


        public byte[] CreateBytes<T>(IEnumerable<T> sources, Dictionary<string, string> exportFieldsWithName)
        {
            using (ExcelPackage pck = CreateExcelPackage(sources, exportFieldsWithName))
            {
                return pck.GetAsByteArray();
            }
        }


        public void CreateFile<T>(IEnumerable<T> sources, string path)
        {
            using (ExcelPackage pck = CreateExcelPackage(sources, path))
            {
                pck.Save();
            }
        }


        public void CreateFile<T>(IEnumerable<T> sources, ICollection<string> exportFields, string path)
        {
            using (ExcelPackage pck = CreateExcelPackage(sources, exportFields, path))
            {
                pck.Save();
            }
        }


        public void CreateFile<T>(IEnumerable<T> sources, Dictionary<string, string> exportFieldsAndName, string path)
        {
            using (ExcelPackage pck = CreateExcelPackage(sources, exportFieldsAndName, path))
            {
                pck.Save();
            }
        }


        /// <summary>
        /// 创建ExcelPackage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sources"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private ExcelPackage CreateExcelPackage<T>(IEnumerable<T> sources, string path = null)
        {
            (ExcelPackage, ExcelWorksheet) ee = GetExcelWorksheet(path);
            DataTable dt = CreateDataTable(sources);
            ee.Item2.Cells["A1"].LoadFromDataTable(dt, true);
            return ee.Item1;
        }



        /// <summary>
        /// 创建ExcelPackage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sources"></param>
        /// <param name="exportFields"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private ExcelPackage CreateExcelPackage<T>(IEnumerable<T> sources, ICollection<string> exportFields, string path = null)
        {
            (ExcelPackage, ExcelWorksheet) ee = GetExcelWorksheet(path);
            DataTable dt = CreateDataTable(sources, exportFields);
            ee.Item2.Cells["A1"].LoadFromDataTable(dt, true);
            return ee.Item1;
        }



        /// <summary>
        /// 创建ExcelPackage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sources"></param>
        /// <param name="exportFieldsWithName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private ExcelPackage CreateExcelPackage<T>(IEnumerable<T> sources, Dictionary<string, string> exportFieldsWithName, string path = null)
        {
            (ExcelPackage, ExcelWorksheet) ee = GetExcelWorksheet(path);
            DataTable dt = CreateDataTable(sources, exportFieldsWithName);
            ee.Item2.Cells["A1"].LoadFromDataTable(dt, true);
            return ee.Item1;
        }



        private (ExcelPackage, ExcelWorksheet) GetExcelWorksheet(string path)
        {
            ExcelPackage pck = null;
            if (string.IsNullOrWhiteSpace(path) == false)
            {
                FileInfo fileInfo = new FileInfo(path);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
                pck = new ExcelPackage(fileInfo);
            }
            else
            {
                pck = new ExcelPackage();
            }
            ExcelWorksheet ws = CreateSheetOn(pck);
            return (pck, ws);
        }


        private ExcelWorksheet CreateSheetOn(ExcelPackage pck)
        {
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("sheet");
            ws.View.FreezePanes(2, 0);//冻结首行
            return ws;
        }



        /// <summary>
        /// 数据源 转 DataTable
        /// </summary>
        /// <typeparam name="T">源数据类型</typeparam>
        /// <param name="sources">源数据</param>
        /// <returns></returns>
        private DataTable CreateDataTable<T>(IEnumerable<T> sources)
        {
            Type type = typeof(T);
            ColumnInfo[] cols = ColumnInfoContainer.GetColumnInfos(type);
            DataTable dataTable = new DataTable();
            foreach (ColumnInfo col in cols)
            {
                dataTable.Columns.Add(col.DisplayName);
            }
            FillDataTable(dataTable, sources, cols);
            return dataTable;
        }



        /// <summary>
        /// 数据源 转 DataTable
        /// </summary>
        /// <typeparam name="T">源数据类型</typeparam>
        /// <param name="sources">源数据</param>
        /// <param name="exportFields"></param>
        /// <returns></returns>
        private DataTable CreateDataTable<T>(IEnumerable<T> sources, ICollection<string> exportFields)
        {
            Type type = typeof(T);
            ColumnInfo[] cols = ColumnInfoContainer.GetColumnInfos(type, exportFields);
            DataTable dataTable = new DataTable();
            foreach (ColumnInfo col in cols)
            {
                dataTable.Columns.Add(col.DisplayName);
            }
            FillDataTable(dataTable, sources, cols);
            return dataTable;
        }




        /// <summary>
        /// 数据源 转 DataTable
        /// </summary>
        /// <typeparam name="T">源数据类型</typeparam>
        /// <param name="sources">源数据</param>
        /// <param name="exportFieldsWithName"></param>
        /// <returns></returns>
        private DataTable CreateDataTable<T>(IEnumerable<T> sources, Dictionary<string, string> exportFieldsWithName)
        {
            Type type = typeof(T);
            ColumnInfo[] cols = ColumnInfoContainer.GetColumnInfos(type, exportFieldsWithName.Keys);
            DataTable dataTable = new DataTable();
            foreach (ColumnInfo col in cols)
            {
                dataTable.Columns.Add(exportFieldsWithName[col.PropertyInfo.Name]);
            }
            FillDataTable(dataTable, sources, cols);
            return dataTable;
        }



        private static void FillDataTable<T>(DataTable dataTable, IEnumerable<T> sources, ColumnInfo[] cols)
        {
            foreach (T obj in sources)
            {
                object[] objArray = new object[cols.Length];
                for (int index = 0; index < cols.Length; ++index)
                {
                    objArray[index] = cols[index].PropertyInfo.GetValue(obj, null);
                }

                dataTable.Rows.Add(objArray);
            }
        }
    }
}
