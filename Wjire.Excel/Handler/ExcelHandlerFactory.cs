using System;

namespace Wjire.Excel
{

    /// <summary>
    /// ExcelHandler工厂
    /// </summary>
    internal static class ExcelHandlerFactory
    {

        /// <summary>
        /// 创建ExcelHandler
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        internal static IExcelHandler CreateHandler(ExcelVersion version)
        {
            switch (version)
            {
                case ExcelVersion.Excel2007:
                    return new Excel2007Handler();
                case ExcelVersion.Excel2003:
                    return new Excel2003Handler();
                default:
                    throw new ArgumentException("the excel version is invalid");
            }
        }
    }
}
