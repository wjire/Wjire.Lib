using System;

namespace ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //int count = 1;

            //using (IASORMInitLogRepository repo = DbFactory.CreateIASORMInitLogRepositoryWrite())
            //{
            //    {
            //        ASORMInitLog log = new ASORMInitLog { AppID = 12, AppName = "wjire_test" };
            //        Stopwatch sw = new Stopwatch();
            //        sw.Start();
            //        for (int i = 0; i < count; i++)
            //        {
            //            ASORMInitLog model = repo.Query("select * from ASORMInitLog where id=@id", new { id = 9});
            //            Console.WriteLine(model.AppName);
            //        }

            //        sw.Stop();
            //        Console.WriteLine("测试一耗时 :" + sw.ElapsedMilliseconds + " ms");
            //    }
            //}


            string s = @"
                        using System.Collections.Generic;
                        namespace {0}
                        {     public List<T> QueryList<T>(string sql, object param) where T : class, new()
                                {
                                    ClearParameters();
                                    AddParameter(param);
                                    return ExecuteReader(sql).ToList<T>();
                                }
                                #endregion
                            }
                        }";


            string sql = @"
                                   SELECT
	                                    obj.name AS 表名,
	                                    col.colorder AS 序号,
	                                    col.name AS ColumnName,
	                                    ISNULL( ep.[value], '' ) AS ColumnDescription,
	                                    t.name AS ColumnType,
	                                    col.length AS 长度,
	                                    ISNULL( COLUMNPROPERTY( col.id, col.name, 'Scale' ), 0 ) AS 小数位数,
                                    CASE
		
		                                    WHEN COLUMNPROPERTY( col.id, col.name, 'IsIdentity' ) = 1 THEN
		                                    '√' ELSE '' 
	                                    END AS 标识,
                                    CASE
		
		                                    WHEN EXISTS (
		                                    SELECT
			                                    1 
		                                    FROM
			                                    dbo.sysindexes si
			                                    INNER JOIN dbo.sysindexkeys sik ON si.id = sik.id 
			                                    AND si.indid = sik.indid
			                                    INNER JOIN dbo.syscolumns sc ON sc.id = sik.id 
			                                    AND sc.colid = sik.colid
			                                    INNER JOIN dbo.sysobjects so ON so.name = si.name 
			                                    AND so.xtype = 'PK' 
		                                    WHERE
			                                    sc.id = col.id 
			                                    AND sc.colid = col.colid 
			                                    ) THEN
			                                    '1' ELSE '' 
		                                    END AS IsKey,
	                                    CASE
			
			                                    WHEN col.isnullable = 1 THEN
			                                    '1' ELSE '' 
		                                    END AS IsNullable,
		                                    ISNULL( comm.text, '' ) AS 默认值 
	                                    FROM
		                                    dbo.syscolumns col
		                                    LEFT JOIN dbo.systypes t ON col.xtype = t.xusertype
		                                    INNER JOIN dbo.sysobjects obj ON col.id = obj.id 
		                                    AND obj.xtype = 'U' 
		                                    AND obj.status >= 0
		                                    LEFT JOIN dbo.syscomments comm ON col.cdefault = comm.id
		                                    LEFT JOIN sys.extended_properties ep ON col.id = ep.major_id 
		                                    AND col.colid = ep.minor_id 
		                                    AND ep.name = 'MS_Description'
		                                    LEFT JOIN sys.extended_properties epTwo ON obj.id = epTwo.major_id 
		                                    AND epTwo.minor_id = 0 
		                                    AND epTwo.name = 'MS_Description' 
	                                    WHERE
		                                    obj.name = '{0}' --表名
		
                                    ORDER BY
	                                    col.colorder;
                                    ";
            s = string.Format(sql, "id");
            Console.WriteLine(s);
        }
    }
}
