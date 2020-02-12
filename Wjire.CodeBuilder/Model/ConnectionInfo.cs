namespace Wjire.CodeBuilder.Model
{

    /// <summary>
    /// 数据库连接字符串信息
    /// </summary>
    public class ConnectionInfo
    {
        /// <summary>
        /// 数据库类型 : "sqlserver",",mysql"
        /// </summary>
        public string Type { get; set; } = "sqlserver";
        public string IP { get; set; }
        public string DbName { get; set; }
        public string User { get; set; }
        public string Pwd { get; set; }
    }
}
