using System;
using System.Collections.Generic;
using System.IO;
using Wjire.CodeBuilder.Model;
using Wjire.CodeBuilder.Utils;

namespace Wjire.CodeBuilder.FileService
{

    /// <summary>
    /// 实体创建,单独的逻辑,不继承任何类
    /// </summary>
    public class Cs_ModelFactory
    {

        /// <summary>
        /// 创建 Model 文件
        /// </summary>
        /// <param name="items"></param>
        /// <param name="formInfo"></param>
        /// <returns></returns>
        public void CreateFile(List<TableInfo> items, FormInfo formInfo)
        {
            string text = CreateCode(items, formInfo);
            string filePath = Path.Combine(formInfo.BasePath, $"{formInfo.NameSpaceName}.Model\\{formInfo.TableName}.cs");
            FileHelper.CreateFile(filePath, text);
        }


        /// <summary>
        /// 创建 Model 文本
        /// </summary>
        /// <param name="items"></param>
        /// <param name="formInfo"></param>
        /// <returns></returns>
        public string CreateCode(List<TableInfo> items, FormInfo formInfo)
        {
            MyStringBuilder fieldBuilder = new MyStringBuilder(256);
            foreach (TableInfo item in items)
            {
                string columnDescription = item.ColumnDescription;
                string keyString = null;
                string typeName = ChangeToCSharpType(item.ColumnType);
                string name = item.ColumnName;
                name = name.Substring(0, 1).ToUpper() + name.Substring(1);
                string isNullable = item.IsNullable == "1" && typeName != "string" ? "?" : "";
                if (string.IsNullOrWhiteSpace(columnDescription) && item.IsKey == "1")
                {
                    columnDescription = "主键";
                    keyString = "[Key]";
                }

                fieldBuilder.AppendLine();
                fieldBuilder.AppendLine(2, "/// <summary>");
                fieldBuilder.AppendLine(2, $"/// {columnDescription}");
                fieldBuilder.AppendLine(2, "/// </summary>");
                if (string.IsNullOrWhiteSpace(keyString) == false)
                {
                    fieldBuilder.AppendLine(2, keyString);
                }
                fieldBuilder.AppendLine(2, "public " + typeName + isNullable + " " + name + " { " + "get; set;" + " }");
                fieldBuilder.AppendLine();
            }

            string[] textArray = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib\\CsTemplate\\Model.txt"));
            MyStringBuilder sb = new MyStringBuilder(128);
            foreach (string text in textArray)
            {
                string item = text.Replace(TemplatePlaceholder.NameSpaceName, formInfo.NameSpaceName).Replace(TemplatePlaceholder.TableName, formInfo.TableName);
                if (item.Trim() == TemplatePlaceholder.FieldArea)
                {
                    string temp = item.Replace(item, fieldBuilder.ToString());
                    sb.AppendLine(temp);
                }
                else
                {
                    sb.AppendLine(item);
                }
            }

            return sb.ToString();
        }



        /// <summary>
        /// 数据库中与C#中的数据类型对照
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string ChangeToCSharpType(string type)
        {
            switch (type.ToLower())
            {
                case "int":
                    return "int";
                case "bigint":
                    return "long";
                case "bit":
                    return "bool";
                case "decimal":
                case "money":
                case "numeric":
                case "smallmoney":
                    return "decimal";
                case "float":
                    return "double";
                case "real":
                    return "Single";
                case "smallint":
                    return "short";
                case "tinyint":
                    return "byte";
                case "char":
                case "nchar":
                case "varchar":
                case "nvarchar":
                case "text":
                case "ntext":
                    return "string";
                case "binary":
                case "image":
                case "varbinary":
                    return "byte[]";
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                case "timestamp":
                    return "DateTime";
                case "uniqueidentifier":
                    return "Guid";
                case "Variant":
                    return "object";
                default:
                    return "string";
            }
        }
    }
}
