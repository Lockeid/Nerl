using System;
using Nerl;

namespace NerlExample
{
    class Program
    {
        static void Main(string[] args)
        {
            using(NerlServer server = new NerlServer(8080)) {
                server.Start();
            }
        }
    }
}
