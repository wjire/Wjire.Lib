﻿using Acb.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Wjire.Log.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //LogService.WriteException(new Exception("test"), "测试日志记录");
            //LogService.WriteCall("method");
            //LogService.WriteText("测试日志记录");

            //LogService.WriteExceptionAsync(new Exception("test"), "测试日志记录异步");
            //LogService.WriteCallAsync("method asc");
            //LogService.WriteTextAsync("测试日志记录异步");

            //var start = new DateTime(2020, 3, 16, 12, 53, 0);
            //var end = new DateTime(2020, 3, 18, 9, 2, 0);

            //var start1 = Convert.ToDateTime(start.ToShortDateString());
            //var end1 = Convert.ToDateTime(end.ToShortDateString());

            //Console.WriteLine(end1.Subtract(start1).Days);
            //Console.WriteLine((end1-start1).Days);

            //var s1 = "成都市新都区龙桥镇杏田路1号蓉?北尚城";
            //var s2 = "成都市新都区龙桥镇杏田路1号蓉垚北尚城";

            //string s1 = "哈?哈";
            //string s2 = "哈犇哈";
            //s1 = s1.Replace("?", "\\w?");
            //Regex regex = new Regex(s1, RegexOptions.None);
            //Console.WriteLine(regex.IsMatch(s2));//true

            var human = new Human
            {
                Id = 1,
                Children = new List<Human>
                  {
                      new Human
                      {
                          Id = 11,
                          Children = new List<Human>
                          {
                              new Human{Id=111}
                          }
                      },
                      new Human
                      {
                          Id = 12,
                          Children = new List<Human>
                          {
                              new Human{Id=222}
                          }
                      }
                  }
            };
            FindChild(human, 222);
        }


        static void FindChild(Human human, int id, out Human man)
        {
            if (human.Children == null || human.Children.Count == 0)
            {
                if (human.Id != id)
                {
                    man = null;
                }
                else
                {
                    man = human;
                }
                return;
            }

            foreach (var child in human.Children)
            {
                if (human.Id != id)
                {
                    FindChild(child, id, out var man1);
                }
            }
        }
    }


    public class Human
    {
        public int Id { get; set; }

        public List<Human> Children { get; set; }
    }

    public class Student : Human
    {
        public string Name { get; set; } = "wjire";
    }
}
