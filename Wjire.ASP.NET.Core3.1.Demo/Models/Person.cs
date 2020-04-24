using System.ComponentModel.DataAnnotations;

namespace Wjire.ASP.NET.Core3._1.Demo.Models
{
    public class Person
    {

        public int Id { get; set; }

        //[StringLength(3, ErrorMessage = "姓名不能大于3个字符")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "地址不能为空")]
        public string Address { get; set; }

    }
}
