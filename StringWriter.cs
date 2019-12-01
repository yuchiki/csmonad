using System;

namespace csmonad
{
    class StringWriter<T>
    {
        public T Value { get; }
        public string Log { get; }
        public StringWriter(T value, String log = "") => (Value, Log) = (value, log);
    }

    static class StringWriterExtensions
    {
        public static StringWriter<T> Return<T>(T value) => new StringWriter<T>(value);
        public static StringWriter<T2> Bind<T1, T2>(StringWriter<T1> sw, Func<T1, StringWriter<T2>> f)
        {
            var sw2 = f(sw.Value);
            return new StringWriter<T2>(sw2.Value, sw.Log + sw2.Log);
        }

        public static StringWriter<T2> Select<T1, T2>(this StringWriter<T1> x, Func<T1, T2> f) =>
            QueryHelper.MakeSelect<T1, T2, StringWriter<T1>, StringWriter<T2>>(Return, Bind)(x, f);

        public static StringWriter<T3> SelectMany<T1, T2, T3>(
            this StringWriter<T1> x,
            Func<T1, StringWriter<T2>> f,
            Func<T1, T2, T3> g
        ) => QueryHelper.MakeSelectMany<T1, T2, T3, StringWriter<T1>, StringWriter<T2>, StringWriter<T3>>(Return, Bind, Bind)(x, f, g);
    }
}
