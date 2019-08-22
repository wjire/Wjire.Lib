using System;
using System.Collections.Generic;

namespace Wjire.Console
{
    public class TestActionLink
    {
        private List<Func<Action, Action>> _funcs = new List<Func<Action, Action>>();

        public TestActionLink Add(Func<Action, Action> func)
        {
            _funcs.Add(func);
            return this;
        }

        public Action Build()
        {
            Action _action = () => System.Console.WriteLine("end");
            _funcs.Reverse();
            foreach (Func<Action, Action> func in _funcs)
            {
                _action = func(_action);
            }
            return _action;
        }
    }



    public class ActionApplication
    {
        private readonly List<Action> _actions = new List<Action>();

        public ActionApplication Add(Action action)
        {
            _actions.Add(action);
            return this;
        }
    }
}
