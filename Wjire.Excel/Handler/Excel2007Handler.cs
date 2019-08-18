using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using OfficeOpenXml;
using Wjire.Excel.Interface;

namespace Wjire.Excel
{

    public class Excel2007Handler : IExcelHandler
    {

        public MemoryStream CreateMemoryStream<T>(IEnumerable<T> sources, HashSet<string> exportFields)
        {
            using (ExcelPackage pck = CreateExcelPackage(sources, exportFields))
            {
                return pck.Stream as MemoryStream;
            }
        }

        public byte[] CreateBytes<T>(IEnumerable<T> sources, HashSet<string> exportFields)
        {
            using (ExcelPackage pck = CreateExcelPackage(sources, exportFields))
            {
                return pck.GetAsByteArray();
            }
        }


        public void CreateFile<T>(IEnumerable<T> sources, HashSet<string> exportFields, string path)
        {
            using (ExcelPackage pck = CreateExcelPackage(sources, exportFields, path))
            {
                pck.Save();
            }
        }



        /// <summary>
        /// 导出ExcelPackage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sources"></param>
        /// <param name="exportFields"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private ExcelPackage CreateExcelPackage<T>(IEnumerable<T> sources, HashSet<string> exportFields, string path = null)
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

            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("sheet");
            ws.View.FreezePanes(2, 0);//冻结首行
            if (exportFields == null || exportFields.Count == 0)
            {
                ws.Cells["A1"].LoadFromCollection(sources, true);
            }
            else
            {
                //如果不是导出所有字段,则需要先转成DataTable.
                DataTable dt = ToDataTable(sources, exportFields);
                ws.Cells["A1"].LoadFromDataTable(dt, true);
            }
            return pck;
        }





        /// <summary>
        /// 数据源 转 DataTable
        /// </summary>
        /// <typeparam name="T">源数据类型</typeparam>
        /// <param name="sources">源数据</param>
        /// <param name="exportFields"></param>
        /// <returns></returns>
        public DataTable ToDataTable<T>(IEnumerable<T> sources, HashSet<string> exportFields)
        {
            Type type = typeof(T);
            ColumnInfo[] cols = ColumnInfoContainer.GetColumnInfos(type, exportFields);
            DataTable dataTable = new DataTable();
            foreach (ColumnInfo col in cols)
            {
                dataTable.Columns.Add(col.DisplayName);
            }
            foreach (T obj in sources)
            {
                object[] objArray = new object[cols.Length];
                for (int index = 0; index < cols.Length; ++index)
                {
                    objArray[index] = cols[index].PropertyInfo.GetValue(obj, null);
                }

                dataTable.Rows.Add(objArray);
            }
            return dataTable;
        }
    }
}
