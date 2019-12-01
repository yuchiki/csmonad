using System;

namespace csmonad
{
    abstract class Maybe<T>
    {
        public static Maybe<T> Nothing => Nothing<T>.Instance;
        public static Maybe<T> Just(T value) => new Just<T>(value);
    }
    sealed class Nothing<T> : Maybe<T>
    {
        private static Nothing<T> instance = new Nothing<T>();
        public static Nothing<T> Instance => instance;
        private Nothing() { }
        public override string ToString() => "Nothing";
    }
    sealed class Just<T> : Maybe<T>
    {
        public T Value { get; }
        public Just(T value) => Value = value;
        public void Deconstruct(out T value) => value = Value;
        public override string ToString() => $"Just({Value})";
    }

    static class MaybeExtension
    {
        public static Maybe<T> Return<T>(T value) => Maybe<T>.Just(value);
        public static Maybe<T2> Bind<T1, T2>(Maybe<T1> x, Func<T1, Maybe<T2>> f) => x switch
        {
            Nothing<T1> _ => Maybe<T2>.Nothing,
            Just<T1>(var v) => f(v)

        };

        public static Maybe<T2> Select<T1, T2>(this Maybe<T1> x, Func<T1, T2> f) =>
            QueryHelper.MakeSelect<T1, T2, Maybe<T1>, Maybe<T2>>(Return, Bind)(x, f);

        public static Maybe<T3> SelectMany<T1, T2, T3>(this Maybe<T1> x, Func<T1, Maybe<T2>> f, Func<T1, T2, T3> g) =>
            QueryHelper.MakeSelectMany<T1, T2, T3, Maybe<T1>, Maybe<T2>, Maybe<T3>>(Return, Bind, Bind)(x, f, g);
    }
}
