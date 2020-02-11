namespace Wjire.CodeBuilder.Model
{
    /// <summary>
    /// 表信息
    /// </summary>
    public class TableInfo
    {

        /// <summary>
        /// 字段名
        /// </summary>
        public string ColumnName { get; set; }


        /// <summary>
        /// 字段注释
        /// </summary>
        public string ColumnDescription { get; set; }


        /// <summary>
        /// 字段类型
        /// </summary>
        public string ColumnType { get; set; }


        /// <summary>
        /// 字段长度
        /// </summary>
        public string ColumnLength { get; set; }


        /// <summary>
        /// 是否可为空 0:否 1:是
        /// </summary>
        public string IsNullable { get; set; }


        /// <summary>
        /// 是否是主键 0:否 1:是
        /// </summary>
        public string IsKey { get; set; }


        /// <summary>
        /// 是否自增 默认 IDENTITY(1,1)
        /// </summary>
        public string IsIncrement { get; set; }
    }
}
