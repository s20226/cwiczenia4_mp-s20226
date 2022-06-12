using System;

namespace LinqTutorials
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Console.WriteLine("Task1");
            var res1 = LinqTasks.Task1();

            foreach (var item in res1)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Task2");
            var res2 = LinqTasks.Task2();

            foreach (var item in res2)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Task3");
            var res3 = LinqTasks.Task3();

           Console.WriteLine(res3);

            Console.WriteLine("Task4");
            var res4 = LinqTasks.Task4();

            foreach (var item in res4)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Task5");
            var res5 = LinqTasks.Task5();

            foreach (var item in res5)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Task6");
            var res6 = LinqTasks.Task6();

            foreach (var item in res6)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Task7");
            var res7 = LinqTasks.Task7();

            foreach (var item in res7)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Task8");
            var res8 = LinqTasks.Task8();
            Console.WriteLine(res8);
            

            Console.WriteLine("Task9");
            var res9 = LinqTasks.Task9();
            Console.WriteLine(res9);

            Console.WriteLine("Task10");
            var res10 = LinqTasks.Task10();
            foreach (var item in res10)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Task11");
            var res11 = LinqTasks.Task11();
            foreach (var item in res11)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Task12");
            var res12 = LinqTasks.Task12();
            foreach (var item in res12)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Task13");
            var res13 = LinqTasks.Task13(new int[] { 1, 1, 1, 1, 1, 1, 10, 1, 1, 1, 1 });
                Console.WriteLine(res13);
            

            Console.WriteLine("Task14");
            var res14 = LinqTasks.Task14();
            foreach (var item in res14)
            {
                Console.WriteLine(item);
            }



        }
    }
}
