using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public struct int2
    {
        public int X;
        public int Y;

        public override string ToString()
        {
            return $"{X}, {Y}";
        }
    }

    public struct Rule
    {
        public int B;
        public int S;
    }

}
