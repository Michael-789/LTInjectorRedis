using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LTInjector
{
    public class Alert
    {
        int counter = 0;
        public void getAlert(string data)
        {
            Console.WriteLine("ALERT!! '{0}", data);
            Console.WriteLine("recieved {0} alerts ", ++counter);
        }
    }
}
