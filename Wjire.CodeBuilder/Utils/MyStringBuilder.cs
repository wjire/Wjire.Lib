using System.Text;

namespace Wjire.CodeBuilder.Utils
{
    public class MyStringBuilder
    {
        private readonly StringBuilder _sb;

        public MyStringBuilder() : this(4)
        {

        }


        public MyStringBuilder(int capacity)
        {
            _sb = new StringBuilder(capacity);
        }

        public void AppendLine()
        {
            _sb.AppendLine();
        }

        public void AppendLine(string value)
        {
            _sb.AppendLine(value);
        }

        public void AppendLine(int num, string value)
        {
            for (int i = 0; i < num; i++)
            {
                _sb.Append("\t");
            }

            _sb.Append(value);
            _sb.Append("\r\n");
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}
