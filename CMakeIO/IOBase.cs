using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;


namespace CMakeIO
{
    public class IOBase
    {
        public static Tuple<int, int, int> CMakeVer = new Tuple<int, int, int>(3, 3, 2);

        public IOBase()
        {

        }
    }

    public class IOWriter : IOBase
    {
        public IOWriter()
        {

        }
    }
}
