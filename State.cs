using System;

namespace csmonad
{
    class State<TState, T>
    {
        public Func<TState, (TState, T)> F { get; }
        public State(Func<TState, (TState, T)> f) => F = f;
    }

    static class StateExtensions
    {
        public static State<TState, T> Return<TState, T>(T value) => new State<TState, T>(s => (s, value));
        public static State<TState, T2> Bind<TState, T1, T2>(State<TState, T1> x, Func<T1, State<TState, T2>> f) =>
            new State<TState, T2>(st =>
            {
                var (newSt, v) = x.F(st);
                return f(v).F(newSt);
            });


        public static State<TState, T2> Select<TState, T1, T2>(this State<TState, T1> x, Func<T1, T2> f) =>
            QueryHelper.MakeSelect<T1, T2, State<TState, T1>, State<TState, T2>>(Return<TState, T2>, Bind)(x, f);

        public static State<TState, T3> SelectMany<TState, T1, T2, T3>(
            this State<TState, T1> x,
            Func<T1, State<TState, T2>> f,
            Func<T1, T2, T3> g
        ) => QueryHelper.MakeSelectMany<T1, T2, T3, State<TState, T1>, State<TState, T2>, State<TState, T3>>(Return<TState, T3>, Bind, Bind)(x, f, g);
    }
}
