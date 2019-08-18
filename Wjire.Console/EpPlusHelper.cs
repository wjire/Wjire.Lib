using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Wjire.Console
{
    internal class EpPlusHelper
    {
        
        /// <summary>
        /// 使用EPPlus导出Excel(xlsx)
        /// </summary>
        /// <param name="sourceTable">数据源</param>
        /// <param name="path">xlsx文件名(不含后缀名)</param>
        public static byte[] ExportByEPPlus(DataTable sourceTable)
        {
            //var fileInfo = new FileInfo(path);
            //if (fileInfo.Exists)
            //{
            //    fileInfo.Delete();
            //    fileInfo = new FileInfo(path);
            //}
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                string sheetName = string.IsNullOrEmpty(sourceTable.TableName) ? "Sheet1" : sourceTable.TableName;
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetName);

                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromDataTable(sourceTable, true);

                ////Format the row
                //ExcelBorderStyle borderStyle = ExcelBorderStyle.Thin;
                //Color borderColor = Color.FromArgb(155, 155, 155);

                //using (ExcelRange rng = ws.Cells[1, 1, sourceTable.Rows.Count + 1, sourceTable.Columns.Count])
                //{
                //    //rng.Style.Font.Name = "宋体";
                //    //rng.Style.Font.Size = 10;
                //    //rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                //    //rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));

                //    //rng.Style.Border.Top.Style = borderStyle;
                //    //rng.Style.Border.Top.Color.SetColor(borderColor);

                //    //rng.Style.Border.Bottom.Style = borderStyle;
                //    //rng.Style.Border.Bottom.Color.SetColor(borderColor);

                //    //rng.Style.Border.Right.Style = borderStyle;
                //    //rng.Style.Border.Right.Color.SetColor(borderColor);

                //    //rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    //rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //}

                //Format the header row
                //using (ExcelRange rng = ws.Cells[1, 1, 1, sourceTable.Columns.Count])
                //{
                //    //rng.Style.Font.Bold = true;
                //    //rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    //rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(234, 241, 246));  //Set color to dark blue
                //    //rng.Style.Font.Color.SetColor(Color.FromArgb(51, 51, 51));
                //}

                pck.Save();
                return pck.GetAsByteArray();
            }
        }





        /// <summary>
        /// 使用EPPlus导出Excel(xlsx)
        /// </summary>
        /// <param name="sourceTable">数据源</param>
        /// <param name="path">xlsx文件名(不含后缀名)</param>
        public static byte[] ExportByCollection<T>(List<T> source)
        {
            //var fileInfo = new FileInfo(path);
            //if (fileInfo.Exists)
            //{
            //    fileInfo.Delete();
            //    fileInfo = new FileInfo(path);
            //}
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                string sheetName = "epplus";
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetName);

                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromCollection(source, true);

                //Format the row
                //ExcelBorderStyle borderStyle = ExcelBorderStyle.Thin;
                //Color borderColor = Color.FromArgb(155, 155, 155);

                //using (ExcelRange rng = ws.Cells[1, 1, source.Count + 1, sourceTable.Columns.Count])
                //{
                //    //rng.Style.Font.Name = "宋体";
                //    //rng.Style.Font.Size = 10;
                //    //rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                //    //rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));

                //    //rng.Style.Border.Top.Style = borderStyle;
                //    //rng.Style.Border.Top.Color.SetColor(borderColor);

                //    //rng.Style.Border.Bottom.Style = borderStyle;
                //    //rng.Style.Border.Bottom.Color.SetColor(borderColor);

                //    //rng.Style.Border.Right.Style = borderStyle;
                //    //rng.Style.Border.Right.Color.SetColor(borderColor);
                //}

                ////Format the header row
                //using (ExcelRange rng = ws.Cells[1, 1, 1, sourceTable.Columns.Count])
                //{
                //    //rng.Style.Font.Bold = true;
                //    //rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    //rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(234, 241, 246));  //Set color to dark blue
                //    //rng.Style.Font.Color.SetColor(Color.FromArgb(51, 51, 51));
                //}

                return pck.GetAsByteArray();
            }
        }
    }
}
