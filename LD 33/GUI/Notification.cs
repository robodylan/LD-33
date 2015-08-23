using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD_33
{
    class Notification
    {
        int x;
        int y;
        int trans;
        string text;
        public Notification(int x, int y, string text)
        {
            this.x = x;
            this.y = y;
            this.trans = 255;
            this.text = text;
        }
    }
}
