using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace Wjire.Excel
{

    internal class Excel2003Handler : IExcelHandler
    {


        public MemoryStream CreateMemoryStream<T>(IEnumerable<T> sources)
        {
            HSSFWorkbook workbook = null;
            try
            {
                ColumnInfo[] cols = ColumnInfoContainer.GetColumnInfos(typeof(T));
                return NewMethod(sources, cols, out workbook);
            }
            finally
            {
                workbook?.Close();
            }
        }
        
    

        public MemoryStream CreateMemoryStream<T>(IEnumerable<T> sources, ICollection<string> exportFields)
        {
            HSSFWorkbook workbook = null;
            try
            {
                ColumnInfo[] cols = ColumnInfoContainer.GetColumnInfos(typeof(T), exportFields);
                return NewMethod(sources, cols, out workbook);
            }
            finally
            {
                workbook?.Close();
            }
        }



        public MemoryStream CreateMemoryStream<T>(IEnumerable<T> sources, Dictionary<string, string> exportFieldsWithName)
        {
            HSSFWorkbook workbook = null;
            try
            {
                Type type = typeof(T);
                ColumnInfo[] cols = ColumnInfoContainer.GetColumnInfos(type, exportFieldsWithName);
                return NewMethod(sources, cols, out workbook);
            }
            finally
            {
                workbook?.Close();
            }
        }




        public byte[] CreateBytes<T>(IEnumerable<T> sources)
        {
            using (MemoryStream ms = CreateMemoryStream(sources))
            {
                return ms.ToArray();
            }
        }


        public byte[] CreateBytes<T>(IEnumerable<T> sources, ICollection<string> exportFields)
        {
            using (MemoryStream ms = CreateMemoryStream(sources, exportFields))
            {
                return ms.ToArray();
            }
        }


        public byte[] CreateBytes<T>(IEnumerable<T> sources, Dictionary<string, string> exportFieldsWithName)
        {
            using (MemoryStream ms = CreateMemoryStream(sources, exportFieldsWithName))
            {
                return ms.ToArray();
            }
        }


        public void CreateFile<T>(IEnumerable<T> sources, string path)
        {
            byte[] bytes = CreateBytes(sources);
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }


        public void CreateFile<T>(IEnumerable<T> sources, ICollection<string> exportFields, string path)
        {
            byte[] bytes = CreateBytes(sources, exportFields);
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        public void CreateFile<T>(IEnumerable<T> sources, Dictionary<string, string> exportFieldsWithName, string path)
        {
            byte[] bytes = CreateBytes(sources, exportFieldsWithName);
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }



        /// <summary>
        /// 创建Sheet及列头
        /// </summary>
        private ISheet CreateSheetWithHeader(IWorkbook workbook, ColumnInfo[] cols, int sheetIndex)
        {
            ISheet sheet = workbook.CreateSheet("第 " + sheetIndex + " 页");
            //冻结首行首列
            sheet.CreateFreezePane(0, 1);

            IRow header = sheet.CreateRow(0);
            for (int i = 0; i < cols.Length; i++)
            {
                ICell cell = header.CreateCell(i);
                cell.SetCellValue(cols[i].DisplayName);
                //自适应宽度
                sheet.AutoSizeColumn(i);
            }
            return sheet;
        }


        /// <summary>
        /// 设置单元格值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cell"></param>
        private void SetCellValue(object value, ICell cell)
        {
            if (value == null)
            {
                cell.SetCellValue(string.Empty);
                return;
            }

            Type type = value.GetType();
            switch (type.Name)
            {
                case "DateTime":
                case "String":
                case "Boolean":
                    cell.SetCellValue(value.ToString());
                    break;
                case "Byte":
                case "Int16":
                case "Int32":
                case "Int64":
                case "Single":
                case "Double":
                case "Decimal":
                    cell.SetCellValue(Convert.ToDouble(value));
                    break;
                default:
                    cell.SetCellValue(string.Empty);
                    break;
            }
        }



        private MemoryStream NewMethod<T>(IEnumerable<T> sources, ColumnInfo[] cols, out HSSFWorkbook workbook)
        {
            workbook = new HSSFWorkbook();
            int sheetIndex = 1;
            ISheet sheet = CreateSheetWithHeader(workbook, cols, sheetIndex);

            int rowIndex = 1;
            foreach (T source in sources)
            {
                //03版 excel 一个 _sheet 最多 65535 行
                if (rowIndex == 65535)
                {
                    sheetIndex++;
                    sheet = CreateSheetWithHeader(workbook, cols, sheetIndex);
                    rowIndex = 1;
                }

                IRow dataRow = sheet.CreateRow(rowIndex);
                for (int i = 0; i < cols.Length; i++)
                {
                    ICell cell = dataRow.CreateCell(i);
                    object value = cols[i].PropertyInfo.GetValue(source, null);
                    SetCellValue(value, cell);
                }

                rowIndex++;
            }

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            return ms;
        }
    }
}
