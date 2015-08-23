using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD_33
{
    public class Target : Entity
    {
        public int age;
        public int fear;
        public string name;
        public bool isMale;
        public int worth;
        public string desc;
        Random rand;
        string[] descList = {"Green shirt, Black hair, Light skin", "Black Shirt, Blonde hair, Tan skin", "Red shirt, White hair, Dark Skin", "Orange Shirt, Red baseball cap, Pale skin"};
        string[] maleNames = {"Ben","Sam","John","Tim","Will","James","Thomas","David","Richard", "George", "Daniel", "Michael", "Charles", "Paul", "Edward", "Brian", "Steven", "Kevin", "Jason", "Anthony" };
        string[] femaleNames = { "Mary", "Patricia", "Linda", "Barbara", "Elizabeth", "Jennifer", "Susan", "Maria", "Margaret", "Michelle", "Donna", "Carol", "Ruth", "Sharon", "Sarah", "Deborah", "Kim", "Karen" };
        public Target(int x, int y) :  base(x,y,32,32,63)
        {
            this.x = x;
            this.y = y;
            this.width = 32;
            this.height = 32;
            rand = new Random();
            this.age = rand.Next(6, 99);
            this.isMale = rand.Next(0, 2) == 1;
            this.fear = ((100 - age) / 10);
            if (this.isMale) fear--; 
            this.ID = 48 + rand.Next(0, 3);
            this.desc = descList[ID - 48];
            if(this.isMale)
            {
                this.name = maleNames[rand.Next(0, maleNames.Length )];
            }
            else
            {
                this.name = femaleNames[rand.Next(0, femaleNames.Length)];
            }
        }

    }
}
