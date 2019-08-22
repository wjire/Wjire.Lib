using System;

namespace Wjire.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TestActionLink link = new TestActionLink().Add(Action1).Add(Action2).Add(Action3).Add(Action4);
            Action action = link.Build();
            action();
            System.Console.ReadKey();
        }


        public static Action Action1(Action action) => () =>
        {
            System.Console.WriteLine("action1");
            action();
        };

        public static Action Action2(Action action) => () =>
        {
            System.Console.WriteLine("action2");
            action();
        };

        public static Action Action3(Action action) => () =>
        {
            System.Console.WriteLine("action3");
            action();
        };

        public static Action Action4(Action action) => () =>
        {
            System.Console.WriteLine("action4");
        };
    }
}
