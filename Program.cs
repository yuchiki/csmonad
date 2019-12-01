using System;
using System.Linq;
using System.Collections.Generic;

namespace csmonad
{
    using static Maybe<int>;
    class Program
    {
        static void Main()
        {
            ListDemo();
            MaybeDemo();
            StringWriterDemo();
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
    }

}
