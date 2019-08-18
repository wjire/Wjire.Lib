using System;
using System.Collections.Generic;
using Wjire.Excel.Interface;
using Wjire.Excel.Model;

namespace Wjire.Excel
{

    /// <summary>
    /// ExcelHandler工厂
    /// </summary>
    public static class ExcelHandlerFactory
    {

        private static readonly Dictionary<ExcelVersion, IExcelHandler> HandlerContainer =
            new Dictionary<ExcelVersion, IExcelHandler>();


        /// <summary>
        /// 创建ExcelHandler
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static IExcelHandler CreateHandler(ExcelVersion version)
        {
            if (HandlerContainer.TryGetValue(version, out IExcelHandler handler))
            {
                return handler;
            }

            switch (version)
            {
                case ExcelVersion.Excel2007:
                    handler = new Excel2007Handler();
                    break;
                case ExcelVersion.Excel2003:
                    handler = new Excel2003Handler();
                    break;
                default:
                    throw new ArgumentException("the excel version is invalid");
            }

            HandlerContainer.Add(version, handler);
            return handler;
        }
    }
}
