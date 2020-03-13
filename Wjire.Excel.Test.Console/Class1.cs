using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;

namespace Wjire.Excel.Test.Console
{
    internal class Class1
    {
        public void Test()
        {
            DataTable tblDatas = new DataTable("Datas");
            DataColumn dc = null;

            dc = tblDatas.Columns.Add("ID", Type.GetType("System.Int32"));
            dc.AutoIncrement = true;//自动增加
            dc.AutoIncrementSeed = 1;//起始为1
            dc.AutoIncrementStep = 1;//步长为1
            dc.AllowDBNull = false;//

            dc = tblDatas.Columns.Add("产品", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("版本", Type.GetType("System.String"));
            dc = tblDatas.Columns.Add("描述", Type.GetType("System.String"));

            //DataRow newRow = tblDatas.NewRow();
            //newRow["产品"] = "大话西游";
            //newRow["版本"] = "2.0";
            //newRow["描述"] = "我很喜欢";
            //tblDatas.Rows.Add(newRow);

            //newRow = tblDatas.NewRow();
            //newRow["产品"] = "梦幻西游";
            //newRow["版本"] = "3.0";
            //newRow["描述"] = "比大话更幼稚";
            //tblDatas.Rows.Add(newRow);

            List<ExpandoObject> obj = GetDynamicListBydt(tblDatas);




            //foreach (var info in obj)
            //{
            //    foreach (var item in info)
            //    {
            //        System.Console.WriteLine(item.Key + ":" + item.Value);
            //    }
            //}

            //var path = @"C:\Users\Administrator\Desktop\1.xlsx";
            //ExcelWriteHelper.CreateFile(tblDatas, path);
        }



        /// <summary>
        /// 使用dynamic根据DataTable的列名自动添加属性并赋值
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns> 
        public static ExpandoObject GetDynamicClassBydt(DataTable dt)
        {
            ExpandoObject d = new ExpandoObject();

            //创建属性，并赋值。
            foreach (DataColumn cl in dt.Columns)
            {
                foreach (DataRow row in dt.Rows)
                {
                    d.TryAdd(cl.ColumnName, row[cl.ColumnName].ToString());
                }
            }
            return d;
        }


        /// <summary>
        /// 使用dynamic根据DataTable的列名自动添加属性并赋值
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns> 
        public static List<ExpandoObject> GetDynamicListBydt(DataTable dt)
        {
            List<ExpandoObject> list = new List<ExpandoObject>();

            foreach (DataRow row in dt.Rows)
            {
                ExpandoObject d = new ExpandoObject();
                //创建属性，并赋值。
                foreach (DataColumn cl in dt.Columns)
                {
                    d.TryAdd(cl.ColumnName, row[cl.ColumnName].ToString());
                }

                list.Add(d);
            }
            return list;
        }
    }
}
