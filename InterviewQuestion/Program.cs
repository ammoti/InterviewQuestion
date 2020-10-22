using InterviewQuestion.Enums;
using InterviewQuestion.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace InterviewQuestion
{
    public class Program
    {
        static void Main()
        {
            // QueryElement Objects
            List<QueryElement> queryElements = QueryElementList.GetList();

            // QueryResultTable Objects
            List<QueryResultTable> queryResultTables = QueryResultTableList.GetList();

            // Start Timer
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ////////////////////////////////////
            /// Place your answer after this section
            ///////////////////////////////////           

            Hashtable hashtable = new Hashtable();
            for (int i = 0; i < queryElements.Count; i++)
            {
                var element = queryElements[i];
                var getProp = typeof(QueryResultTable).GetProperty("column" + element.Index);
                hashtable.Add(element.Index, getProp);

            }
            int value;
            var group = queryResultTables.GroupBy(x => x.perfdate);
            Parallel.ForEach(group, new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) * 1.0)) },
                   index =>
                   {
                       foreach (var item in queryElements)
                       {
                           PropertyInfo colPro = (PropertyInfo)hashtable[item.Index];
                           switch (item.Aggregate)
                           {
                               case AggregateType.sum:
                                   var total = index.Sum(s => int.TryParse(colPro?.GetValue(s).ToString(), out value) ? value : 0);
                                   colPro.SetValue(index.First(), total.ToString());
                                   break;

                               case AggregateType.avg:
                                   var avg = index.Average(s => int.TryParse(colPro?.GetValue(s).ToString(), out value) ? value : 0);
                                   colPro.SetValue(index.First(), avg.ToString());
                                   break;

                               case AggregateType.max:
                                   var max = index.Max(s => int.TryParse(colPro?.GetValue(s).ToString(), out value) ? value : 0);
                                   colPro.SetValue(index.First(), max.ToString());
                                   break;

                               case AggregateType.min:
                                   var min = index.Min(s => int.TryParse(colPro?.GetValue(s).ToString(), out value) ? value : 0);
                                   colPro.SetValue(index.First(), min.ToString());
                                   break;
                               default:
                                   break;
                           }
                       }
                   });
            ////////////////////////////////////
            /// Place your answer before this section
            ///////////////////////////////////

            // Stop Timer
            stopWatch.Stop();

            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            ////////////////////////////
            // Data Validation
            ////////////////////////////

            if (queryResultTables[0].column1 != "800")
            {
                Console.WriteLine("ERROR IN CALCULATION for column1!");
            }

            if (queryResultTables[0].column2 != "160")
            {
                Console.WriteLine("ERROR IN CALCULATION for column2!");
            }

            if (queryResultTables[0].column3 != "180")
            {
                Console.WriteLine("ERROR IN CALCULATION for column3!");
            }

            if (queryResultTables[0].column4 != "320")
            {
                Console.WriteLine("ERROR IN CALCULATION for column4!");
            }

            if (queryResultTables[0].column5 != "780")
            {
                Console.WriteLine("ERROR IN CALCULATION for column5!");
            }

            if (queryResultTables[0].column6 != "15")
            {
                Console.WriteLine("ERROR IN CALCULATION for column6!");
            }

            Console.WriteLine("Execution duration: " + elapsedTime);

            Thread.Sleep(10000);
        }
    }
}
