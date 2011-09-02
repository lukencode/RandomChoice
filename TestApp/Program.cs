using System;
using RandomChoice;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            randomMethodCalls();
            weightedRandomMethodCalls();

            weightedGeneric();

            Console.ReadLine();
        }

        private static void weightedGeneric()
        {
            Console.WriteLine("-- Weighted Random Return Values --");

            //add actions to the selector (must add up to 1.0)
            var selector = RandomSelector<int>
                    .Add(0.7, 1)
                    .Add(0.2, 2)
                    .Add(0.1, 3);

            for (int i = 0; i < 5; i++)
            {
                var val = selector.Choose(); //randomly picks an action to run
                Console.WriteLine("chose " + val);
            }

            Console.WriteLine();
        }

        private static void weightedRandomMethodCalls()
        {
            Console.WriteLine("-- Weighted Random Method Calls --");

            //add actions to the selector (must add up to 1.0)
            var selector = RandomSelector
                    .Add(0.7, () => { Console.WriteLine("0.7 - probably do this"); })
                    .Add(0.2, () => { Console.WriteLine("0.2 - maybe do this"); })
                    .Add(0.1, () => { Console.WriteLine("0.1 - small chance of this"); });

            for (int i = 0; i < 5; i++)
            {
                selector.Choose(); //randomly picks an action to run
            }

            Console.WriteLine();
        }

        private static void randomMethodCalls()
        {
            Console.WriteLine("-- Random Method Calls --");

            //add actions to the selector
            var selector = RandomSelector
                            .Add(() => { Console.WriteLine("do first thing"); })
                            .Add(() => { Console.WriteLine("do other thing"); });

            for (int i = 0; i < 5; i++)
            {
                selector.Choose(); //randomly picks an action to run
            }

            Console.WriteLine();
        }
    }
}
