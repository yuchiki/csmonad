using System;
using System.Linq;
using System.Collections.Generic;

namespace csmonad
{
    using static Maybe<int>;
    class Program
    {
        static void Main() => new List<Action> { MaybeDemo }.ForEach(f => f());

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
        }
    }

}
