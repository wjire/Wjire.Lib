
һ.����Excel

Ĭ��ʹ�õ�������� Epplus ���� Excel2007 (Epplus ��֧�� Excel2003);
��ѡ��ʹ�� NPOI ���� Excel2003,�� NPOI �����������ʱ��ǳ����ڴ�,����.

���ֵ��÷�ʽ:
1.�����ֽ�
	byte[] bytes = ExcelHelper.CreateBytes(datas);
2.������
	MemoryStream = ExcelHelper.CreateMemoryStream(datas);
3.�����ļ�
	ExcelHelper.CreateFile(datas);


ֻ�м��� [DisplayName] ���Ե����Բ��п��ܵ���

        /// <summary>
        /// ����
        /// </summary>
        [DisplayName("���")]
        public long ID { get; set; }


������÷�ʽ��� ExcelHelper ��


��.��ȡExcel

    List<T> datas = new ExcelReadHandler(fileName).Read<T>(...);

    /// <summary>
    /// Excel��Ĭ�ϵ�һ��Sheet����������
    /// </summary>
    /// <param name="fields">Excel�����У�����Ҫת����Ϊ�Ķ����ֶ�����</param>
    /// <param name="sheetIndex">�ڼ���sheet,Ĭ�ϵ�һ��</param>
    /// <returns></returns>
    public List<T> Read<T>(string[] fields, int sheetIndex = 1) where T : class, new()

����
    
    CsvReadHelper.ReadByDefaultEncoding<T>(fileName)
    CsvReadHelper.ReadByUTF8Encoding<T>(fileName)
    CsvReadHelper.ReadByGB2312<T>(fileName)

