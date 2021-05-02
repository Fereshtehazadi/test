using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadManager threadManager = new ThreadManager();
            threadManager.Run();
            Console.ReadLine();
        }
    }
}
