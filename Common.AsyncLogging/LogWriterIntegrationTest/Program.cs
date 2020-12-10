using System;
using System.Reflection;
using System.Threading;

namespace LogWriterIntegrationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Log Writer Repo Integration Tests");
            TestLogWriterRepo repo = new TestLogWriterRepo();
            repo.LoadExceptionOccurred += Repo_LoadExceptionOccurred;

            repo.LoadFromPath(Environment.CurrentDirectory);

            repo.LogDebug(typeof(Program), "Info");

            Thread.Sleep(500);


            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        }

        private static void Repo_LoadExceptionOccurred(object sender, EventArgs e)
        {
            Console.WriteLine("Exception");
        }
    }
}
