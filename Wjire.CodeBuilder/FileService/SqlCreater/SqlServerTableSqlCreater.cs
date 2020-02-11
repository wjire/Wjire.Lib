using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wjire.CodeBuilder;
using Wjire.CodeBuilder.Model;
using Wjire.CodeBuilder.Utils;

namespace FileService
{

    public class SqlServerTableSqlCreater : ITableSqlCreater
    {

        /// <summary>
        /// 生成 数据库创建表的sql语句
        /// </summary>
        /// <param name="path">excel路径</param>
        /// <param name="entityName">实体名称</param>
        /// <returns></returns>
        public string Create(string path, string entityName)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            LoadExcelHelper excel = new LoadExcelHelper(path);
            List<string> keys = new List<string>();
            List<TableInfo> tableList = excel.ExcelToList<TableInfo>(typeof(TableInfo).GetProperties().Select(s => s.Name).ToArray()).ToList();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"CREATE TABLE [dbo].[{entityName}] (");
            foreach (TableInfo table in tableList)
            {
                if (table.IsKey == "1")
                {
                    keys.Add("[" + table.ColumnName + "]");
                }
                sb.AppendLine($" [{table.ColumnName}] {table.ColumnType}{GetLength(table.ColumnLength)} {GetSortRule(table.ColumnType)} {GetNullable(table)},");
            }
            sb = sb.Remove(sb.Length - 3, 1);
            sb.AppendLine(")");
            sb.AppendLine("GO");
            sb.AppendLine();
            sb.AppendLine($"ALTER TABLE [dbo].[{entityName}] SET (LOCK_ESCALATION = TABLE)");
            sb.AppendLine("GO");

            foreach (TableInfo table in tableList)
            {
                sb.AppendLine($@"
    EXEC sp_addextendedproperty
    'MS_Description', N'{table.ColumnDescription}',
    'SCHEMA', N'dbo',
    'TABLE', N'{entityName}',
    'COLUMN', N'{table.ColumnName}'
    GO");
                sb.AppendLine();
            }

            if (keys.Count > 0)
            {
                sb.AppendLine($@"
ALTER TABLE [dbo].[{entityName}] ADD CONSTRAINT [PK_{entityName}] PRIMARY KEY CLUSTERED ({string.Join(",", keys)})
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO");
            }

            return sb.ToString();
        }


        private string GetSortRule(string type)
        {
            type = type.Trim().ToLower();
            if (type.Contains("char"))
            {
                return "COLLATE Chinese_PRC_CI_AS";
            }
            return null;
        }


        private string GetLength(string length)
        {
            return string.IsNullOrWhiteSpace(length) ? null : $"({length})";
        }

        private string GetNullable(TableInfo tableInfo)
        {
            if (tableInfo.IsKey == "1")
            {
                return tableInfo.IsIncrement == "1" ? "IDENTITY(1,1) NOT NULL" : "NOT NULL";
            }
            return tableInfo.IsNullable == "1" ? "Null" : "NOT NULL";
        }
    }
}
