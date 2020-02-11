
一.导出Excel

默认使用第三方组件 Epplus 导出 Excel2007 (Epplus 不支持 Excel2003);
可选择使用 NPOI 导出 Excel2003,但 NPOI 在数据量大的时候非常耗内存,慎用.

三种调用方式:
1.返回字节
	byte[] bytes = ExcelHelper.CreateBytes(datas);
2.返回流
	MemoryStream = ExcelHelper.CreateMemoryStream(datas);
3.生成文件
	ExcelHelper.CreateFile(datas);


只有加了 [DisplayName] 特性的属性才有可能导出

        /// <summary>
        /// 主键
        /// </summary>
        [DisplayName("编号")]
        public long ID { get; set; }


具体调用方式详见 ExcelHelper 类


二.读取Excel

    List<T> datas = new ExcelReadHandler(fileName).Read<T>(...);

    /// <summary>
    /// Excel中默认第一张Sheet导出到集合
    /// </summary>
    /// <param name="fields">Excel各个列，依次要转换成为的对象字段名称</param>
    /// <param name="sheetIndex">第几张sheet,默认第一张</param>
    /// <returns></returns>
    public List<T> Read<T>(string[] fields, int sheetIndex = 1) where T : class, new()

或者
    
    CsvReadHelper.ReadByDefaultEncoding<T>(fileName)
    CsvReadHelper.ReadByUTF8Encoding<T>(fileName)
    CsvReadHelper.ReadByGB2312<T>(fileName)

