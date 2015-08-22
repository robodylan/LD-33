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
        public Button(int x, int y, int width, int height,string text)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.clicked = false;
            this.text = text;
            this.visible = true;
        }
    }
}
