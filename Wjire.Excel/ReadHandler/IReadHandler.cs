using System;
using System.Collections.Generic;

namespace Wjire.Excel
{
    public interface IReadHandler : IDisposable
    {

        /// <summary>
        /// 获取一行的所有数据,并统一转换为 string ,因此可能会抛出异常 . rowIndex = 1 为第一行
        /// </summary>
        /// <param name="rowIndex">第 rowIndex 行</param>
        /// <param name="sheetIndex">第几张sheet,默认第一张</param>
        /// <returns></returns>
        List<string> GetCells(int rowIndex, int sheetIndex = 1);


        /// <summary>
        /// Excel => List
        /// </summary>
        /// <param name="throwExceptionIfCellValueIsNull">当单元格无值时,是否抛出异常,默认 false</param>
        /// <param name="sheetIndex">读取第几张sheet,默认第1张</param>
        /// <returns></returns>
        List<T> Read<T>(bool throwExceptionIfCellValueIsNull = false, int sheetIndex = 1) where T : class, new();


        /// <summary>
        /// Excel => List
        /// </summary>
        /// <param name="columnMaps">需要读取的列及要转换成的对象字段名称</param>
        /// <param name="throwExceptionIfCellValueIsNull">当单元格无值时,是否抛出异常,默认 false</param>
        /// <param name="sheetIndex">读取第几张sheet,默认第1张</param>
        /// <returns></returns>
        List<T> Read<T>(IDictionary<int, string> columnMaps, bool throwExceptionIfCellValueIsNull = false, int sheetIndex = 1)
            where T : class, new();
    }
}
