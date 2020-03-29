using System.ComponentModel.DataAnnotations;

namespace Wjire.Excel.Test.Console
{
    public class Person
    {
        [Display(Name = "账号", Order = 3)]
        public string Account { get; set; }

        [Display(Name = "公司名称", Order = 2)]
        public string CompanyName { get; set; }

        [Display(Name = "姓名", Order = 1)]
        public string Name { get; set; }

        [Display(Order = 4)]
        public int Age { get; set; }

        public string Address { get; set; }
    }
}
