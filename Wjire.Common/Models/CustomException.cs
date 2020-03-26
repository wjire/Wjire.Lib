using System;

namespace Wjire.Common
{
    /// <summary>
    /// 自定义异常
    /// </summary>
    public class CustomException : Exception
    {

        /// <summary>
        /// 
        /// </summary>
        public CustomException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public CustomException(string message) : base(message)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public CustomException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}