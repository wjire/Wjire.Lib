using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Wjire.Excel.Model;

namespace Wjire.Excel
{
    public static class ExcelHelper
    {

        /// <summary>
        /// 导出字节流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sources">数据源</param>
        /// <param name="exportFields">需要导出字段,可不传,则导出所有字段</param>
        /// <param name="version">excel版本,可不传,默认excel2007</param>
        /// <returns></returns>
        public static MemoryStream CreateMemoryStream<T>(IEnumerable<T> sources, HashSet<string> exportFields = null, ExcelVersion version = ExcelVersion.Excel2007)
        {
            CheckSources(sources);
            return ExcelHandlerFactory.CreateHandler(version).CreateMemoryStream(sources, exportFields);
        }


        /// <summary>
        /// 导出字节数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sources">数据源</param>
        /// <param name="exportFields">需要导出字段,可不传,则导出所有字段</param>
        /// <param name="version">excel版本,可不传,默认excel2007</param>
        /// <returns></returns>
        public static byte[] CreateBytes<T>(IEnumerable<T> sources, HashSet<string> exportFields = null, ExcelVersion version = ExcelVersion.Excel2007)
        {
            CheckSources(sources);
            return ExcelHandlerFactory.CreateHandler(version).CreateBytes(sources, exportFields);
        }


        /// <summary>
        /// 生成Excel文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sources">数据源</param>
        /// <param name="exportFields">需要导出字段,可不传,则导出所有字段</param>
        /// <param name="version">excel版本,可不传,默认excel2007</param>
        /// <returns></returns>
        public static void CreateFile<T>(IEnumerable<T> sources, string path, HashSet<string> exportFields = null, ExcelVersion version = ExcelVersion.Excel2007)
        {
            CheckSources(sources);
            ExcelHandlerFactory.CreateHandler(version).CreateFile(sources, exportFields, path);
        }



        private static void CheckSources<T>(IEnumerable<T> sources)
        {
            if (sources == null || sources.Any() == false)
            {
                throw new ArgumentNullException("the sources is null");
            }
        }
    }
}
