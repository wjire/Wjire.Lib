
using System.Collections.Generic;

namespace Wjire.Common
{

    /// <summary>
    /// 分页查询结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagingResult<T>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="data"></param>
        public PagingResult(Paging paging, IEnumerable<T> data)
        {
            Paging = paging;
            Data = data;
        }


        /// <summary>
        /// 分页信息
        /// </summary>
        public Paging Paging { get; set; }


        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerable<T> Data { get; set; }
    }
}
