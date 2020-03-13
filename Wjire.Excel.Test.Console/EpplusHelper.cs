using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using OfficeOpenXml;

namespace Wjire.Excel.Test.Console
{
    /// <summary>
    /// 使用  EPPlus 第三方的组件读取Excel
    /// </summary>
    public class EPPlusHelper
    {
        private static string GetString(object obj)
        {
            try
            {
                return obj.ToString();
            }
            catch (Exception ex)
            {
                return "*********";
            }
        }

        /// <summary>
        ///将指定的Excel的文件转换成DataTable （Excel的第一个sheet）
        /// </summary>
        /// <param name="fullFielPath">文件的绝对路径</param>
        /// <returns></returns>
        public static DataTable WorksheetToTable(string fullFielPath)
        {
            try
            {
                FileInfo existingFile = new FileInfo(fullFielPath);

                ExcelPackage package = new ExcelPackage(existingFile);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//选定 指定页

                return WorksheetToTable(worksheet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 将worksheet转成datatable
        /// </summary>
        /// <param name="worksheet">待处理的worksheet</param>
        /// <returns>返回处理后的datatable</returns>
        public static DataTable WorksheetToTable(ExcelWorksheet worksheet)
        {
            //获取worksheet的行数
            int rows = worksheet.Dimension.End.Row;
            //获取worksheet的列数
            int cols = worksheet.Dimension.End.Column;

            DataTable dt = new DataTable(worksheet.Name);
            DataRow dr = null;
            for (int i = 1; i <= rows; i++)
            {
                if (i > 1)
                    dr = dt.Rows.Add();

                for (int j = 1; j <= cols; j++)
                {
                    //默认将第一行设置为datatable的标题
                    if (i == 1)
                        dt.Columns.Add(GetString(worksheet.Cells[i, j].Value));
                    //剩下的写入datatable
                    else
                        dr[j - 1] = GetString(worksheet.Cells[i, j].Value);
                }
            }
            return dt;
        }
    }

}
