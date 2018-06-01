using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math2D;

namespace System.CLIGraphics {
    class Program
    {
        static void Main(string[] args) {
            Triangle r = new Triangle(0,0,0,1,1,1);

            Console.WriteLine(r.ToString());
        }
    }
}
