using System.ComponentModel.DataAnnotations;

namespace Wjire.Excel.Test.Console
{
    public class Person
    {
        [Display(Name = "编号", Order = 1)]
        public int Id { get; set; }

        public string Account { get; set; }

        public string CompanyName { get; set; }

        [Display(Name = "姓名", Order = 2)]
        public string Name { get; set; }

        [Display(Name = "年龄", Order = 3)]
        public int Age { get; set; }

        public string Address { get; set; }
    }
}
