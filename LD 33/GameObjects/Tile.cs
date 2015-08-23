using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD_33
{
    class Tile
    {
        public int x { get; set; }
        public int y { get; set; }
        public int ID { get; set; }
        public bool hasPhysics { get; set; }
        public Tile(int x, int y, int ID, bool hasPhysics)
        {
            this.x = x;
            this.y = y;
            this.ID = ID;
            this.hasPhysics = hasPhysics;
        }
    }
}
