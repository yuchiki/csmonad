using System;

namespace csmonad
{
    class Reader<TIn, TOut>
    {
        public Func<TIn, TOut> F { get; }
        public Reader(Func<TIn, TOut> f) => F = f;
    }

    static class ReaderExtensions
    {
        public static Reader<TIn, T> Return<TIn, T>(T value) => new Reader<TIn, T>(_ => value);
        public static Reader<TIn, T2> Bind<TIn, T1, T2>(Reader<TIn, T1> x, Func<T1, Reader<TIn, T2>> f) =>
            new Reader<TIn, T2>(y => f(x.F(y)).F(y));

        public static Reader<TIn, T2> Select<TIn, T1, T2>(this Reader<TIn, T1> x, Func<T1, T2> f) =>
            QueryHelper.MakeSelect<T1, T2, Reader<TIn, T1>, Reader<TIn, T2>>(Return<TIn, T2>, Bind)(x, f);

        public static Reader<TIn, T3> SelectMany<TIn, T1, T2, T3>(
            this Reader<TIn, T1> x,
            Func<T1, Reader<TIn, T2>> f,
            Func<T1, T2, T3> g
        ) => QueryHelper.MakeSelectMany<T1, T2, T3, Reader<TIn, T1>, Reader<TIn, T2>, Reader<TIn, T3>>(Return<TIn, T3>, Bind, Bind)(x, f, g);

    }
}
