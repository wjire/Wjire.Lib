using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Wjire.Excel
{
    public static class ExcelWriteHelper
    {


        /// <summary>
        /// 导出字节流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sources">数据源</param>
        /// <param name="version">excel版本,可不传,默认excel2007</param>
        /// <returns></returns>
        public static MemoryStream CreateMemoryStream<T>(IEnumerable<T> sources, ExcelVersion version = ExcelVersion.Excel2007)
        {
            CheckSources(sources);
            return WriteHandlerFactory.CreateHandler(version).CreateMemoryStream(sources);
        }


        /// <summary>
        /// 导出字节流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sources">数据源</param>
        /// <param name="exportFields">需要导出的字段,不可为空</param>
        /// <param name="version">excel版本,可不传,默认excel2007</param>
        /// <returns></returns>
        public static MemoryStream CreateMemoryStream<T>(IEnumerable<T> sources, ICollection<string> exportFields, ExcelVersion version = ExcelVersion.Excel2007)
        {
            CheckSources(sources);
            CheckExportFields(exportFields);
            return WriteHandlerFactory.CreateHandler(version).CreateMemoryStream(sources, exportFields);
        }


        /// <summary>
        /// 导出字节流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sources">数据源</param>
        /// <param name="exportFieldsWithName">需要导出的字段及自定义列名,不可为空</param>
        /// <param name="version">excel版本,可不传,默认excel2007</param>
        /// <returns></returns>
        public static MemoryStream CreateMemoryStream<T>(IEnumerable<T> sources, Dictionary<string, string> exportFieldsWithName, ExcelVersion version = ExcelVersion.Excel2007)
        {
            CheckSources(sources);
            CheckExportFieldsWithName(exportFieldsWithName);
            return WriteHandlerFactory.CreateHandler(version).CreateMemoryStream(sources, exportFieldsWithName);
        }


        /// <summary>
        /// 导出字节数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sources">数据源</param>
        /// <param name="version">excel版本,可不传,默认excel2007</param>
        /// <returns></returns>
        public static byte[] CreateBytes<T>(IEnumerable<T> sources, ExcelVersion version = ExcelVersion.Excel2007)
        {
            CheckSources(sources);
            return WriteHandlerFactory.CreateHandler(version).CreateBytes(sources);
        }


        /// <summary>
        /// 导出字节数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sources">数据源</param>
        /// <param name="exportFields">需要导出的字段,不可为空</param>
        /// <param name="version">excel版本,可不传,默认excel2007</param>
        /// <returns></returns>
        public static byte[] CreateBytes<T>(IEnumerable<T> sources, ICollection<string> exportFields, ExcelVersion version = ExcelVersion.Excel2007)
        {
            CheckSources(sources);
            CheckExportFields(exportFields);
            return WriteHandlerFactory.CreateHandler(version).CreateBytes(sources, exportFields);
        }


        /// <summary>
        /// 导出字节数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sources">数据源</param>
        /// <param name="exportFieldsWithName">需要导出的字段及自定义列名</param>
        /// <param name="version">excel版本,可不传,默认excel2007</param>
        /// <returns></returns>
        public static byte[] CreateBytes<T>(IEnumerable<T> sources, Dictionary<string, string> exportFieldsWithName, ExcelVersion version = ExcelVersion.Excel2007)
        {
            CheckSources(sources);
            CheckExportFieldsWithName(exportFieldsWithName);
            return WriteHandlerFactory.CreateHandler(version).CreateBytes(sources, exportFieldsWithName);
        }



        /// <summary>
        /// 生成Excel文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sources">数据源</param>
        /// <param name="path">文件路径</param>
        /// <param name="version">excel版本,可不传,默认excel2007</param>
        /// <returns></returns>
        public static void CreateFile<T>(IEnumerable<T> sources, string path, ExcelVersion version = ExcelVersion.Excel2007)
        {
            CheckSources(sources);
            CheckPath(path);
            WriteHandlerFactory.CreateHandler(version).CreateFile(sources, path);
        }



        /// <summary>
        /// 生成Excel文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sources">数据源</param>
        /// <param name="path">文件路径</param>
        /// <param name="exportFields">需要导出的字段,可不传,则导出所有字段</param>
        /// <param name="version">excel版本,可不传,默认excel2007</param>
        /// <returns></returns>
        public static void CreateFile<T>(IEnumerable<T> sources, string path, ICollection<string> exportFields, ExcelVersion version = ExcelVersion.Excel2007)
        {
            CheckSources(sources);
            CheckPath(path);
            CheckExportFields(exportFields);
            WriteHandlerFactory.CreateHandler(version).CreateFile(sources, exportFields, path);
        }



        /// <summary>
        /// 生成Excel文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sources">数据源</param>
        /// <param name="path">文件路径</param>
        /// <param name="exportFieldsWithName">需要导出的字段及自定义列名</param>
        /// <param name="version">excel版本,可不传,默认excel2007</param>
        /// <returns></returns>
        public static void CreateFile<T>(IEnumerable<T> sources, string path, Dictionary<string, string> exportFieldsWithName, ExcelVersion version = ExcelVersion.Excel2007)
        {
            CheckSources(sources);
            CheckPath(path);
            CheckExportFieldsWithName(exportFieldsWithName);
            WriteHandlerFactory.CreateHandler(version).CreateFile(sources, exportFieldsWithName, path);
        }


        /// <summary>
        /// 生成Excel文件,暂时只支持 Excel2007
        /// </summary>
        /// <param name="sources">数据源</param>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static void CreateFile(DataTable sources, string path)
        {
            CheckPath(path);
            WriteHandlerFactory.CreateHandler(ExcelVersion.Excel2007).CreateFile(sources, path);
        }


        /// <summary>
        /// 生成Excel文件,暂时只支持 Excel2007
        /// </summary>
        /// <param name="sources">数据源</param>
        /// <returns></returns>
        public static byte[] CreateBytes(DataTable sources)
        {
            return WriteHandlerFactory.CreateHandler(ExcelVersion.Excel2007).CreateBytes(sources);
        }



        private static void CheckSources<T>(IEnumerable<T> sources)
        {
            if (sources == null || sources.Any() == false)
            {
                throw new ArgumentNullException("the sources is null");
            }
        }

        private static void CheckExportFields(ICollection<string> exportFields)
        {
            if (exportFields == null || exportFields.Count == 0)
            {
                throw new ArgumentNullException("the exportFields is null");
            }
        }

        private static void CheckExportFieldsWithName(Dictionary<string, string> exportFieldsWithName)
        {
            if (exportFieldsWithName == null || exportFieldsWithName.Count == 0)
            {
                throw new ArgumentNullException("the exportFieldsWithName is null");
            }
        }

        private static void CheckPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("the path is null");
            }
        }
    }
}
