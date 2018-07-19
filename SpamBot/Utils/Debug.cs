using System;
using System.Collections.Generic;
using System.Text;

namespace SpamBot.Utils
{
    static class Debug
    {
        internal static void Log(object o)
        {
            Console.WriteLine($"{DateTime.Now}:{o}");
        }
    }
}
