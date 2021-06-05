using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {


            //using (var enumerator = GenerateIntegers(2).GetEnumerator())
            //{
            //    while (enumerator.MoveNext())
            //    {
            //        Console.WriteLine(enumerator.Current);
            //    }
            //}

            using (var enumerator = GenerateIntegers1(2).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current == 1)
                    {
                        break;
                    }
                    Console.WriteLine(enumerator.Current);
                }
            }

            using (var enumerator = GenerateIntegers1(2).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current == 1)
                    {
                        break;
                    }
                    Console.WriteLine(enumerator.Current);
                }
            }

            //foreach (var i in GenerateIntegers(2))
            //{
            //    Console.WriteLine(i);
            //}

        }

        public static IEnumerable<int> GenerateIntegers1(int count)
        {
            InnerGenerateInteger ret = new InnerGenerateInteger(-2);
            ret.Count = count;
            return ret;
        }

        public static IEnumerable<int> GenerateIntegers(int count)
        {
            try
            {
                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine("Yielding {0}", i);
                    yield return i;
                    int doubled = i * 2;
                    Console.WriteLine("Yielding {0}", doubled);
                    yield return doubled;
                    //Console.WriteLine("ready next");
                    //yield return doubled;
                }
            }
            finally
            {
                Console.WriteLine("In finally block");
            }
        }

        private sealed class InnerGenerateInteger : IEnumerable<int>, IEnumerator<int>
        {
            public int _state;//当前状态
            private int _current;//当前值
            private readonly int _initialThreadId;//线程Id
            private int _count;//循环次数
            public int Count;//调用时传入的次数
            private int _i;//当前索引
            private int _doubleI;//局部变量

            int IEnumerator<int>.Current => _current;

            object IEnumerator.Current => _current;

            public InnerGenerateInteger(int state)
            {
                this._state = state;
                _initialThreadId = Environment.CurrentManagedThreadId;
            }

            void IDisposable.Dispose()
            {
                int num = _state;
                if (num == -3 || (uint)(num - 1) <= 1u)
                {
                    try
                    {
                    }
                    finally
                    {
                        Finally();
                    }
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (_state)
                    {
                        default:
                            return false;
                        case 0:
                            _state = -1;
                            _state = -3;
                            _i = 0;
                            break;
                        case 1:
                            _state = -3;
                            _doubleI = _i * 2;
                            Console.WriteLine("Yielding {0}", _doubleI);
                            _current = _doubleI;
                            _state = 2;
                            return true;
                        case 2:
                            _state = -3;
                            _i++;
                            break;
                    }
                    if (_i < _count)
                    {
                        Console.WriteLine("Yielding {0}", _i);
                        _current = _i;
                        _state = 1;
                        return true;
                    }
                    Finally();
                    return false;
                }
                catch
                {
                    ((IDisposable)this).Dispose();
                    throw;
                }
            }

            bool IEnumerator.MoveNext()
            {
                //ILSpy generated this explicit interface implementation from .override directive in MoveNext
                return this.MoveNext();
            }

            private void Finally()
            {
                _state = -1;
                Console.WriteLine("In finally block");
            }

            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            IEnumerator<int> IEnumerable<int>.GetEnumerator()
            {
                InnerGenerateInteger g;
                if (_state == -2 && _initialThreadId == Environment.CurrentManagedThreadId)
                {
                    _state = 0;
                    g = this;
                }
                else
                {
                    g = new InnerGenerateInteger(0);
                }
                _count = Count;
                return g;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable<int>)this).GetEnumerator();
            }
        }
    }
    class CapturedVariablesDemo
    {
        private string instanceField = "instance field";
        public Action<string> CreateAction(string methodParameter)
        {
            string methodLocal = "method local";
            string uncaptured = "uncaptured local";
            Action<string> action = lambdaParameter =>
            {
                string lambdaLocal = "lambda local";
                Console.WriteLine("Instance field: {0}", instanceField);
                Console.WriteLine("Method parameter: {0}", methodParameter);
                Console.WriteLine("Method local: {0}", methodLocal);
                Console.WriteLine("Lambda parameter: {0}", lambdaParameter);
                Console.WriteLine("Lambda local: {0}", lambdaLocal);
            };
            methodLocal = "modified method local";
            return action;
        }

    }

    public class Person
    {
        private readonly List<int> _ids;

        public Person(List<int> ids)
        {
            _ids = ids;
        }
    }
}
