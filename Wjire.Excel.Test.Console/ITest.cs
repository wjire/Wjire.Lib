using System;
using System.Collections.Generic;
using System.Text;

namespace Wjire.Excel.Test.Console
{
    public interface ITest
    {

        /// <summary>
        /// 打印
        /// </summary>
        void Print();
    }


    public class Test : ITest
    {
        /// <summary>
        /// 打印
        /// </summary>
        public void Print()
        {
            throw new NotImplementedException();
        }
    }
}

