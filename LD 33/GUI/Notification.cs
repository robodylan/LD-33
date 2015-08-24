using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD_33
{
    class Notification
    {
        public int x;
        public int y;
        public int trans;
        public string text;
        public Notification(int x, int y, string text)
        {
            this.x = x;
            this.y = y;
            this.trans = 255;
            this.text = text;
        }
    }
}
