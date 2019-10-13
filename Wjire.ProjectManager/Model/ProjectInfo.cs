using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wjire.ProjectManager.Model
{
    public class ProjectInfo
    {

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }


        /// <summary>
        /// 项目文件夹
        /// </summary>
        public string ProjectDir { get; set; }


        /// <summary>
        /// 1:IIS 2:EXE
        /// </summary>
        public int ProjectType { get; set; }
    }
}
