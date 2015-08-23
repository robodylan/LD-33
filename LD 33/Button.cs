using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD_33
{
    class Button
    {
        public int x;
        public int y;
        public int width;
        public int height;
        public bool clicked;
        public string text;
        public bool visible;
        public int trans;
        public Button(int x, int y, string text)
        {
            this.x = x;
            this.y = y;
            this.width = (int)(text.Length * 15f);
            this.height = 50;
            this.clicked = false;
            this.text = text;
            this.visible = true;
            this.trans = 255;
        }
    }
}
