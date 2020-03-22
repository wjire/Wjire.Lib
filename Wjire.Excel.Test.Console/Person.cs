using System.ComponentModel.DataAnnotations;

namespace Wjire.Excel.Test.Console
{
    public class Person
    {
        [Display(Order = 1)]
        public string Account { get; set; }
        [Display(Order = 2)]
        public string Name { get; set; }
        [Display(Order = 7)]
        public string Role { get; set; }
        [Display(Order = 8)]
        public string Phone { get; set; }
    }
}
