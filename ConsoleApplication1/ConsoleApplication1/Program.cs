

using System;

using System.Collections.Generic;

using System.Drawing;

using System.Linq;

using System.Reactive.Linq;

using System.Reactive.Subjects;

using System.Text;

using System.Threading.Tasks;

using Microsoft.Reactive.Testing;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ReactiveUI.Testing;



namespace ConsoleApplication1

{

    class Program

    {

        private enum Environment

        {

            Secure,

            Stage,

            Latest

        }



        private class CurrentVersion

        {

            private readonly Environment _environment;

            private readonly int _buildNumber;



            public CurrentVersion(Environment environment, Int32 buildNumber)

            {

                _environment = environment;

                _buildNumber = buildNumber;

            }



            public int BuildNumber

            {

                get { return _buildNumber; }

            }



            public Environment Environment

            {

                get { return _environment; }

            }



            public override string ToString()

            {

                return String.Format("{0}:{1}", Environment, BuildNumber);

            }

        }



        private static Random _random = new Random();



        private static IEnumerable<Int32> InfiniteStreamOfRandomNumbers()

        {

            while (true)

            {

                yield return _random.Next(1, 100);

            }

        }





        static void Main(string[] args)

        {

            var myNumbers = InfiniteStreamOfRandomNumbers()

                .Distinct()

                .Take(50)

                .OrderBy(i => i);



            Console.WriteLine(String.Join(",", myNumbers));

            Console.ReadKey();



            var scheduler = new TestScheduler();



            var versionEvents = scheduler.CreateHotObservable(

                scheduler.OnNextAt(1000, new CurrentVersion(Environment.Latest, 1)),

                scheduler.OnNextAt(2000, new CurrentVersion(Environment.Stage, 1)),

                scheduler.OnNextAt(3000, new CurrentVersion(Environment.Latest, 1)),

                scheduler.OnNextAt(4000, new CurrentVersion(Environment.Latest, 1)),

                scheduler.OnNextAt(5000, new CurrentVersion(Environment.Latest, 2)),

                scheduler.OnNextAt(6000, new CurrentVersion(Environment.Stage, 2)),

                scheduler.OnNextAt(7000, new CurrentVersion(Environment.Latest, 2)),

                scheduler.OnNextAt(8000, new CurrentVersion(Environment.Stage, 2)),

                scheduler.OnNextAt(9000, new CurrentVersion(Environment.Latest, 2)),

                scheduler.OnNextAt(10000, new CurrentVersion(Environment.Stage, 2)),

                scheduler.OnNextAt(11000, new CurrentVersion(Environment.Latest, 2)),

                scheduler.OnNextAt(12000, new CurrentVersion(Environment.Latest, 3)),

                scheduler.OnNextAt(13000, new CurrentVersion(Environment.Latest, 3)),

                scheduler.OnNextAt(14000, new CurrentVersion(Environment.Stage, 2)),

                scheduler.OnNextAt(15000, new CurrentVersion(Environment.Latest, 3)),

                scheduler.OnNextAt(15500, new CurrentVersion(Environment.Latest, 4)),

                scheduler.OnNextAt(16000, new CurrentVersion(Environment.Stage, 2))

            );



            var versionChangesOnLatest = DetectVersionChangeOnEnvironment(Environment.Latest, versionEvents);

            var versionChangesOnStage = DetectVersionChangeOnEnvironment(Environment.Stage, versionEvents);



            versionChangesOnStage.Subscribe(cv => Console.WriteLine(String.Format("Stage Version Changed to : '{0}'", cv)));

            versionChangesOnLatest.Subscribe(cv => Console.WriteLine(String.Format("Latest Version Changed to : '{0}'", cv)));





            Console.WriteLine(String.Format("The current time in the scheduler is {0}", scheduler.Now));

            Console.ReadKey();

            scheduler.AdvanceByMs(4000);



            Console.WriteLine(String.Format("The current time in the scheduler is {0}", scheduler.Now));

            Console.ReadKey();

            scheduler.AdvanceByMs(4000);



            Console.WriteLine(String.Format("The current time in the scheduler is {0}", scheduler.Now));

            Console.ReadKey();

            scheduler.AdvanceByMs(4000);



            Console.WriteLine(String.Format("The current time in the scheduler is {0}", scheduler.Now));

            Console.ReadKey();

            scheduler.AdvanceByMs(4000);



            Console.WriteLine(String.Format("The current time in the scheduler is {0}", scheduler.Now));

            Console.WriteLine("Done");

            Console.ReadKey();





        }



        private static IObservable<CurrentVersion> DetectVersionChangeOnEnvironment(Environment environment, IObservable<CurrentVersion> currentVersions)

        {

            return currentVersions

                .Where(cv => cv.Environment == environment)

                .DistinctUntilChanged(cv => cv.BuildNumber);

        }

    }

}