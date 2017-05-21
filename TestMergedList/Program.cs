using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMergedList
{
    class Program
    {
        private static List<TestClass> List1 = new List<TestClass>();
        private static List<TestClass> List2 = new List<TestClass>();
        private static List<TestClass> List3 = new List<TestClass>();
        private static List<TestClass> List4 = new List<TestClass>();

        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var task1 = Task.Run(() =>
            {
                List1 = GetList7();
            });

            var task2 = Task.Run(() =>
            {
                List2 = GetList30();
            });

            var task3 = Task.Run(() =>
            {
                List3 = GetList90();
            });

            var task4 = Task.Run(() =>
            {
                List4 = GetList180();
            });

            Task.WaitAll(task1, task2, task3, task4);

            var merge = GetMergedList();
            //var union = GetUnionLists().GroupBy(z => z.id).ToDictionary(x => x.Key).Select(y => y.Value).ToList();
            //var concat = GetConcatLists();
            
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }

        private static List<TestClass> GetList7()
        {
            var list1 = new List<TestClass>();

            for (int i = 0; i < 100000; i++)
            {
                list1.Add( new TestClass
                {
                    id = i,
                    name = $"Paolo{i}",
                    age = 34,
                    prop7 = 7
                });
            }

            list1.Add(new TestClass
            {
                id = 6,
                name = "pippo",
                age = 21,
                prop7 = 73
            });

            return list1;
        }

        private static List<TestClass> GetList30()
        {
            var list1 = new List<TestClass>();
            for (int i = 0; i < 100000; i++)
            {
                list1.Add(new TestClass
                {
                    id = i,
                    name = $"Paolo{i}",
                    age = 34,
                    prop90 = 90
                });
            }
            return list1;
        }

        private static List<TestClass> GetList90()
        {
            var list1 = new List<TestClass>();
            for (int i = 0; i < 100000; i++)
            {
                list1.Add(new TestClass
                {
                    id = i,
                    name = $"Paolo{i}",
                    age = 34,
                    prop30 = 30
                });
            }
            return list1;
        }

        private static List<TestClass> GetList180()
        {
            var list1 = new List<TestClass>();

            for (int i = 0; i < 1000000; i++)
            {
                list1.Add(new TestClass
                {
                    id = i,
                    name = $"Paolo{i}",
                    age = 34,
                    prop180 = 180
                });
            }

            return list1;
        }

        private static List<TestClass> GetMergedList()
        {
            Func<TestClass, TestClass, TestClass> mergeFunc = (p1, p2) => new TestClass
            {
                id = p1.id,
                name = p1.name == null ? p2.name : p1.name,
                age = p1.age == 0 ? p2.age : p1.age,
                prop7 = p1.prop7 == 0 ? p2.prop7  : p1.prop7,
                prop30 = p1.prop30 == 0 ? p2.prop30 : p1.prop30,
                prop90 = p1.prop90 == 0 ? p2.prop90 : p1.prop90,
                prop180 = p1.prop180 == 0 ? p2.prop180 : p1.prop180
            };

            var result = List1
                .Concat(List2)
                .Concat(List3)
                .Concat(List4)
                .GroupBy(x => x.id, (k, g) => g.Aggregate(mergeFunc));

            //var result = firstList
            //    .Concat(List4)
            //    .GroupBy(x => x.id, (k, g) => g.Aggregate(mergeFunc));

            return result.ToList();
        }


        //private void test()
        //{
        //    List<TestClass> lst = List1.Union(List2).Union(List3).Union(List4).ToLookup(x => x.id).Select(x => new TestClass()
        //    {
        //        id = x.Key,
        //        name = x.Select(y => y.name).Aggregate((z1, z2) => z1 ?? z2),
        //        age = x.Select(y => y.age).Aggregate((z1, z2) => z1 == 0 ? z2 : z1),
        //        prop7 = x.Select(y => y.prop7).Aggregate((z1, z2) => z1 == 0 ? z2 : z1),
        //        prop30 = x.Select(y => y.prop30).Aggregate((z1, z2) => z1 == 0 ? z2 : z1),
        //        prop90 = x.Select(y => y.prop90).Aggregate((z1, z2) => z1 == 0 ? z2 : z1),
        //        prop180 = x.Select(y => y.prop180).Aggregate((z1, z2) => z1 == 0 ? z2 : z1),
        //    }).ToList();
        //}

        private static List<TestClass> GetUnionLists()
        {
            return  List1.Union(List2).Union(List3).ToList();
        }

        private static List<TestClass> GetConcatLists()
        {
            return List1.Concat(List2).Concat(List3).ToList();
        }
    }
}
