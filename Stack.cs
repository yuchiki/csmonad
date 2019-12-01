using System;
using System.Collections.Generic;
using System.Linq;

namespace csmonad
{
    abstract class Stack<T>
    {
        public List<T> ToList()
        {
            var list = ToReversedList();
            list.Reverse();
            return list;
        }

        public (Stack<T>, T) Pop()
        {
            var cons = (Cons<T>)this;
            return (cons.Tail, cons.Head);
        }

        public abstract List<T> ToReversedList();
    }

    class Nil<T> : Stack<T>
    {
        public override List<T> ToReversedList() => new List<T>();
    }

    class Cons<T> : Stack<T>
    {
        public T Head { get; }
        public Stack<T> Tail { get; }
        public Cons(T head, Stack<T> tail) => (Head, Tail) = (head, tail);

        public override List<T> ToReversedList()
        {
            var list = Tail.ToReversedList();
            list.Add(Head);
            return list;
        }
    }
}
