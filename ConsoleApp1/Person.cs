using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ConsoleApp1
{
    public class Person
    {

        [DisplayName("编号")]
        public int Id { get; set; }


        public string Name { get; set; }
    }
}
