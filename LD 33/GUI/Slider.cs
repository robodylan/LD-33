using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD_33
{
    class Slider
    {
        public int x { get; set; }
        public int y { get; set; } 
        public int percentFull { get; set; }

        public Slider(int x, int y, int percentFull)
        {
            this.x = x;
            this.y = y;
            this.percentFull = percentFull;
        }
    }
}
