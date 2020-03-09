using System;
using System.Collections.Generic;
using System.Text;

namespace Wjire.Log
{
    internal class TextLogInfo : BaseLogInfo
    {
        private readonly string _content;

        public TextLogInfo(string content, string relativePath)
        {
            _content = content;
            RelativePath = relativePath;
        }


        protected override string ToContent()
        {
            return _content;
        }
    }
}
