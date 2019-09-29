namespace Wjire.CodeBuilder
{

    /// <summary>
    /// 数据库表创建语句 创建者
    /// </summary>
    public interface ITableSqlCreater
    {

        /// <summary>
        /// 生成 数据库创建表的sql语句
        /// </summary>
        /// <param name="path">excel路径</param>
        /// <param name="entityName">实体名称</param>
        /// <returns></returns>
        string Create(string path, string entityName);
    }
}
