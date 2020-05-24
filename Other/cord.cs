using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Other
{
    class cord
    {
        int _x=0;
        int _y=0;
        public int x
        {
            get { return _x; }
            set {
                if (value < 0) _x = 0;
                else
                if (value > Console.WindowWidth) _x = Console.WindowWidth;
                else _x = value;
            }
        }
        public int y
        {
            get { return _y; }
            set
            {
                if (value < 0) _y = 0;
                else
                if (value > Console.WindowHeight) _y = Console.WindowHeight;
                else _y = value;
            }
        }
        public cord(int a, int b)
        {
            x = a; y = b;   
        }
        public cord(cord a)
        {
            x = a.x;
            y = a.y;
        }
        public static bool operator !=(cord a, cord b)
        {
            if (Equals(a,null) && Equals(b,null)) return false;
            if (Equals(a,null) || Equals(b,null)) return true;
            if ((a.x != b.x) || (a.y != b.y))
                return true;
            return false;
        }
        public static bool operator ==(cord a, cord b)
        {
            if (Equals(a, null) && Equals(b, null)) return true;
            if (Equals(a, null) || Equals(b, null)) return false;
            if ((a.x == b.x) && (a.y == b.y))
                return true;
            return false;
        }
        public static void Swap(ref cord a, ref cord b)
        {
            cord c = a;
            a = new cord(b);
            b = new cord(c);
        }
    }
}
