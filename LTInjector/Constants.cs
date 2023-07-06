using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTInjector
{
    public class Constants
    {
        public static readonly int MSG_PER_SEC = 600;
        //EXCHAGES
        public static readonly string RAW_FLIGHTS_EXCHANGE = "raw_flights";
        public static readonly string ALERTS_EXCHANGE = "alerts";
        //RABBIT TYPES
        public static readonly string SENDER = "SENDER";
        public static readonly string RECIEVER = "RECIEVER";

         //FLIGHT COORDINATES
        public static double  XSTART = 0.0;
        public static double  YSTART = 0.0;
        public static double  XEND = 2.0;
        public static double  YEND = 2.0;

        public static readonly int TRACKPERIOD = 10;


    }

}
