using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wjire.ProjectManager.WebApi.Model
{
    public class AppInfoView
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public int Status { get; set; }
    }
}
