using System;
using System.Linq;
using System.Collections.Generic;

namespace csmonad
{
    using static Maybe<int>;
    using static UnitType;
    class Program
    {
        static void Main()
        {
            ListDemo();
            MaybeDemo();
            StringWriterDemo();
            ReaderDemo();
            StateDemo();
        }

        static void ListDemo()
        {
            Console.WriteLine("ListDemo:");

            var ns =
                from a in new int[] { 10, 20, 30 }
                from b in new int[] { 4, 5 }
                select a + b;


            ns.ToList().ForEach(Console.WriteLine);
            Console.WriteLine();
        }

        static void MaybeDemo()
        {
            Console.WriteLine("Maybe Demo:");

            Maybe<int> sum(Maybe<int> ma, Maybe<int> mb, Maybe<int> mc) =>
                from a in ma
                from b in mb
                from c in mc
                select a + b + c;

            var testCases = new List<(Maybe<int>, Maybe<int>, Maybe<int>)>{
                (Just(1), Just(2), Just(3)),
                (Nothing, Just(2), Just(3)),
                (Just(1), Nothing, Just(3)),
                (Just(1), Just(2), Nothing),
                (Nothing, Nothing, Nothing),
            };

            testCases.ForEach(testCase =>
            {
                var (ma, mb, mc) = testCase;
                Console.WriteLine($"{ma} + {mb} + {mc} = {sum(ma, mb, mc)}");
            });
            Console.WriteLine();
        }

        static void StringWriterDemo()
        {
            Console.WriteLine("StringWriter Demo:");
            StringWriter<int> SW(int i, String s) => new StringWriter<int>(i, s);

            var sw =
                from a in SW(3, "a is 3.\n")
                from b in SW(3 + 4, "b is a + 4.\n")
                from c in SW(a + b, "c is a + b.\n")
                select c;

            Console.WriteLine($"value: {sw.Value}");
            Console.WriteLine($"log:\n{sw.Log}");
            Console.WriteLine();
        }

        static void ReaderDemo()
        {
            Console.WriteLine("Reader Demo:");

            Reader<int, int> R(Func<int, int> f) => new Reader<int, int>(f);
            var r =
                from twice in R(x => x * 2)
                from l in R(x => x.ToString().Length)
                from plusHundred in R(x => x + 100)
                select $"twice: {twice}, length: {l}, * 100: {plusHundred}";

            Console.WriteLine(r.F(15));
            Console.WriteLine();
        }

        static void StateDemo()
        {
            State<Stack<int>, int> pop() =>
                new State<Stack<int>, int>(stack => stack.Pop());
            State<Stack<int>, UnitType> push(int i) =>
                new State<Stack<int>, UnitType>(stack => (new Cons<int>(i, stack), Unit));

            var f =
                from _1 in push(1)
                from _2 in push(2)
                from _3 in push(3)
                from _4 in push(4)
                from _5 in push(5)
                from a in pop()
                from b in pop()
                from c in pop()
                select a + b + c;

            var (state, result) = f.F(new Nil<int>());

            Console.WriteLine("state");
            state.ToList().ForEach(Console.WriteLine);
            Console.WriteLine($"result = {result}");
        }

    }

    class UnitType
    {
        private static UnitType instance = new UnitType();
        private UnitType() { }
        public static UnitType Unit = instance;

    }
}
