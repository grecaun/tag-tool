using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool
{
    class Log
    {
        public static void D(String s)
        {
#if DEBUG
            Console.WriteLine(DateTime.Now.ToLongTimeString() + " LOGOUTPUT - d - " + s);
#endif
        }
    }
}
