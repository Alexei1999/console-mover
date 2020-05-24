using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Other
{
    class Obj
    {
        public cord pos = null;
        public char c = ' ';
        public ConsoleColor color = ConsoleColor.Black;
        public Obj(char z, cord s, ConsoleColor clr = ConsoleColor.Black)
        {
            pos = new cord(s);
            c = z;
            color = clr;
        }
    }
    static class Map
    {
        public enum Teams { friend, enemy };
        public enum Type { attack, since, trade, support }
        static bool pause = false;
        public static List<Bot> bots = new List<Bot>();
        static public List<Timer> timers = new List<Timer>();
        static public List<Obj> obj = null;
        static Display map = new Display(new cord(0,0),60, 120);
        static Bot target = null;
        static cord lastCord = null;
        static bool lining = false;
        static bool botMove = false;
        public static void AddBot() { }
        public static void DeleBot() { }
        public static Bot IsShipThere(cord c)
        {
            foreach (Bot m in bots)
            {
                if (m.pos == c)
                    return m;
            }
            return null;
        }
        public static void SetPause() => pause = !pause;
        public static void UnsetPause() => pause = false;
        public static void Doing()
        {
            for (int i = 0; i < timers.Count; i++)
                if (timers[i].Tick())
                    i--;
            foreach (Bot b in bots)
                b.Do();
        }
        public static void ShowMap()
        {
            foreach (Bot n in bots)
            {
                Console.ForegroundColor = n.logic.stats.color;
                cord z = n.pos;
                Console.SetCursorPosition(z.x, z.y);
                Console.Write(n.c);
                //foreach (Obj o in n.ToObj())
                //    map.Insert(o);
            }
            //foreach (string str in map.ToString())
            //    Console.Write(str);
            Console.ResetColor();
        }
        public static void Step(cord c)
        {
            ShowMap();
            if (!pause) Doing();
            if (lining == true)
                Drow(Line(target.pos, c));
            lastCord = new cord(c);
        }
        public static void Stop() {
            if (botMove)
            {
                pause = false;
                target = null;
                lining = false;
            }
        }
        public static void Add(cord c)
        {
            new Bot(c);
        }
        public static void ShipMove(cord c)
        {
            if (target == null)
            {
                Bot t = IsShipThere(c);
                if (t != null)
                {
                    SetPause();
                    ShipMove(t);
                }
            }
            else
            {
                lining = false;
                target.Move(c);
                target = null;
                UnsetPause();
            }
        }
        public static void ShipMove(Bot t)
        {
            if (botMove == false)
            {
                target = t;
                lining = true;
            }
            else Stop();
        }
        public static cord[] Line(cord a, cord b)
        {
            if (a == b) return null;
            int dx = Math.Abs(b.x - a.x);
            int dy = Math.Abs(b.y - a.y);
            bool reversed = false;
            List<cord> cords = new List<cord>();
            if (dx >= dy)
            {
                if (a.x > b.x) cord.Swap(ref a, ref b);
                for (int i = a.x, j = a.y; i <= b.x; i++)
                {
                    cords.Add(new cord(i, j));
                    if (i != 0 && dy!=0 && j!=b.y && i % (dx / dy) == 0)
                    {
                        if (a.y > b.y)
                            j--;
                        else j++;
                        cords.Add(new cord(i, j));
                    }
                }
            }
            else
            {
                reversed = true;
                if (a.y > b.y) cord.Swap(ref a, ref b);
                for (int i = a.y, j = a.x; i <= b.y; i++)
                {
                    cords.Add(new cord(j, i));
                    if (i != 0 && dx!= 0 && j!=b.x && i % (dy / dx) == 0)
                    {
                        if (a.x > b.x)
                            j--;
                        else j++;
                        cords.Add(new cord(j, i));
                    }
                }
            }
            cords.RemoveAt(cords.Count-1);
            cords.RemoveAt(0);
            if (reversed) cords.Reverse();
            return cords.ToArray();
        }
        static void Drow(cord[] cords)
        {
            if (cords == null) return;
            for (int i = 0; i < cords.Length - 1; i++)
            {
                Console.SetCursorPosition(cords[i].x, cords[i].y);
                BitArray lrud = new BitArray(4);
                if (i != 0)
                {
                    if (cords[i - 1].x < cords[i].x) lrud[0] = true;
                    if (cords[i - 1].x > cords[i].x) lrud[1] = true;
                    if (cords[i - 1].y < cords[i].y) lrud[2] = true;
                    if (cords[i - 1].y > cords[i].y) lrud[3] = true;
                }
                if (cords[i + 1].x < cords[i].x) lrud[0] = true;
                if (cords[i + 1].x > cords[i].x) lrud[1] = true;
                if (cords[i + 1].y < cords[i].y) lrud[2] = true;
                if (cords[i + 1].y > cords[i].y) lrud[3] = true;
                Console.Write(AsciiLining(lrud));
            }
        }
        static char AsciiLining(BitArray lrud, int type = 0)
        {
            if (type == 0)
            {
                if (lrud[0] && lrud[2])
                    return '┘';
                if (lrud[0] && lrud[3])
                    return '┐';
                if (lrud[1] && lrud[2])
                    return '└';
                if (lrud[1] && lrud[3])
                    return '┌';
                if (lrud[0] && lrud[2] && lrud[1])
                    return '┴';
                if (lrud[0] && lrud[3] && lrud[1])
                    return '┬';
                if (lrud[0] && lrud[2] && lrud[3])
                    return '┤';
                if (lrud[1] && lrud[3] && lrud[2])
                    return '├';
                if (lrud[0] && lrud[3] && lrud[2] && lrud[1])
                    return '┼';
                if (lrud[2] || lrud[3])
                    return '│';
                if (lrud[0] || lrud[1])
                    return '─';
            }
            if (type == 1)
            {
                if (lrud[0] && lrud[2])
                    return '╛';
                if (lrud[0] && lrud[3])
                    return '╕';
                if (lrud[1] && lrud[2])
                    return '╘';
                if (lrud[1] && lrud[3])
                    return '╒';
                if (lrud[0] && lrud[2] && lrud[1])
                    return '╧';
                if (lrud[0] && lrud[3] && lrud[1])
                    return '╤';
                if (lrud[0] && lrud[2] && lrud[3])
                    return '╡';
                if (lrud[1] && lrud[3] && lrud[2])
                    return '╞';
                if (lrud[0] && lrud[3] && lrud[2] && lrud[1])
                    return '╪';
                if (lrud[2] || lrud[3])
                    return '│';
                if (lrud[0] || lrud[1])
                    return '═';
            }
            if (type == 2)
            {
                if (lrud[0] && lrud[2])
                    return '╜';
                if (lrud[0] && lrud[3])
                    return '╖';
                if (lrud[1] && lrud[2])
                    return '╙';
                if (lrud[1] && lrud[3])
                    return '╓';
                if (lrud[0] && lrud[2] && lrud[1])
                    return '╨';
                if (lrud[0] && lrud[3] && lrud[1])
                    return '╥';
                if (lrud[0] && lrud[2] && lrud[3])
                    return '╢';
                if (lrud[1] && lrud[3] && lrud[2])
                    return '╟';
                if (lrud[0] && lrud[3] && lrud[2] && lrud[1])
                    return '╫';
                if (lrud[2] || lrud[3])
                    return '║';
                if (lrud[0] || lrud[1])
                    return '─';
            }
            if (type == 3)
            {
                if (lrud[0] && lrud[2])
                    return '╝';
                if (lrud[0] && lrud[3])
                    return '╗';
                if (lrud[1] && lrud[2])
                    return '╚';
                if (lrud[1] && lrud[3])
                    return '╔';
                if (lrud[0] && lrud[2] && lrud[1])
                    return '╩';
                if (lrud[0] && lrud[3] && lrud[1])
                    return '╦';
                if (lrud[0] && lrud[2] && lrud[3])
                    return '╣';
                if (lrud[1] && lrud[3] && lrud[2])
                    return '╠';
                if (lrud[0] && lrud[3] && lrud[2] && lrud[1])
                    return '╬';
                if (lrud[2] || lrud[3])
                    return '║';
                if (lrud[0] || lrud[1])
                    return '═';
            }
            return '.';
        }
    }
    class Display
    {
        public cord pos = null;
        public char[][] arr = null;
        public Display(cord p, int a, int b)
        {
            pos = new cord(p);
            arr = new char[a][];
            for (int i = 0; i < a; i++)
                arr[i] = new char[b];
        }
        public void Insert(Obj j)
        {
            arr[j.pos.x][j.pos.y] = j.c;
        }
        new public string[] ToString()
        {
            List<string> str = new List<string>();
            for (int i = 0; i < arr.Length / arr.GetLength(0); i++)
                str.Add(new string(arr[i]));
            return str.ToArray();
        }
    }

    public class Timer
    {
        public bool Tick()//подружить с делегатом
        {
            if (c == t)
            {
                time = true;
                if (cycle) c = 0;
                else { Delete(); return true; }
            }
            else { time = false; }
            c++;
            return false;
        }
        int t;
        int c;
        bool cycle;
        public bool time;
        public Timer(int a, bool h = false)
        {
            t = a;
            c = 0;
            time = false;
            cycle = h;
            Map.timers.Add(this);
        }
        public void Delete() => Map.timers.Remove(this);
    }//Обычный таймер
}
