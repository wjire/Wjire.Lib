using System.IO;
using System.Text;
using Wjire.CodeBuilder.Model;
using Wjire.CodeBuilder.Utils;

namespace Wjire.CodeBuilder.FileService
{

    /// <summary>
    /// 文件创建基类 工厂模式+模板模式
    /// </summary>
    public abstract class BaseFactory
    {
        public void CreateFile(FormInfo formInfo)
        {
            string from = GetFromTemplateInfoPath(formInfo);
            string[] stringArray = File.ReadAllLines(from);
            string content = CreateContent(formInfo, stringArray);
            string to = GetToSavePath(formInfo);
            FileHelper.CreateFile(to, content, IsCover());
        }

        protected abstract string GetFromTemplateInfoPath(FormInfo formInfo);

        protected virtual string CreateContent(FormInfo formInfo, string[] stringArray)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string line in stringArray)
            {
                string item = ReplaceTemplateOfLine(line, formInfo);
                sb.AppendLine(item);
            }
            return sb.ToString();
        }

        protected abstract string ReplaceTemplateOfLine(string line, FormInfo formInfo);

        protected abstract string GetToSavePath(FormInfo formInfo);

        protected virtual bool IsCover()
        {
            return false;
        }
    }
}
