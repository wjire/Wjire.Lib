using System;

namespace Wjire.Excel
{

    /// <summary>
    /// WriteHandler工厂
    /// </summary>
    internal static class WriteHandlerFactory
    {

        /// <summary>
        /// 创建ExcelHandler
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        internal static IWriteHandler CreateHandler(ExcelVersion version)
        {
            switch (version)
            {
                case ExcelVersion.Excel2007:
                    return new Excel2007WriteHandler();
                case ExcelVersion.Excel2003:
                    return new Write2003Handler();
                default:
                    throw new ArgumentException("the excel version is invalid");
            }
        }
    }
}
