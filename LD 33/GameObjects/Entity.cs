using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD_33
{
    public class Entity
    {
        public int x;
        public int y;
        public int width;
        public int height;
        public int ID;
        public enum Movement { Back, Forward, Right, Left}
        public Movement direction = Movement.Forward;
        public Entity(int x, int y, int width, int height,int ID)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.ID = ID;
        }
    }
}
