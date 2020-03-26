namespace Wjire.Common
{
    /// <summary>
    /// 分页类
    /// </summary>
    public class Paging
    {

        /// <summary>
        /// 页码,默认第一页
        /// </summary>
        public int PageIndex { get; set; } = 1;


        /// <summary>
        /// 页大小(默认10页)
        /// </summary>
        public int PageSize { get; set; } = 10;


        /// <summary>
        /// 总条数
        /// </summary>
        public int RowsCount { get; set; }


        /// <summary>
        /// 总页数
        /// </summary>
        private int _pageCount;


        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                _pageCount = (RowsCount % PageSize) == 0
                                     ? RowsCount / PageSize
                                     : (RowsCount / PageSize) + 1;
                return _pageCount;
            }

            set => _pageCount = value;
        }


        /// <summary>
        /// 是否分页,默认是
        /// </summary>
        public bool IsPaging { get; set; } = true;


        /// <summary>
        /// 开始索引
        /// </summary>
        public int StartRows
        {
            get
            {
                if (PageIndex <= 0)
                {
                    return 0;
                }

                return PageSize * (PageIndex - 1);
            }
        }
    }
}