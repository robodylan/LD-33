using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD_33
{
    public class Target : Entity
    {
        int age;
        int fear;
        string name;
        bool isMale;
        Random rand;
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
            this.fear = rand.Next();
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
