using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace Other
{
    public class Program
    {
        static void Press(ConsoleKeyInfo a, cord b)
        {
            if (a.Key == ConsoleKey.Spacebar)
                Map.SetPause();
            if (a.Key == ConsoleKey.N)
                Map.Add(b);
            if (a.Key == ConsoleKey.Enter)
                Map.ShipMove(b);
        }
        static void Main(string[] args)
        {
            Cursor.Position = new Point(0, 0);
            Console.SetCursorPosition(0, 0);
            ConsoleKeyInfo t = new ConsoleKeyInfo();
            Stopwatch watch = new Stopwatch();
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                watch.Start();
                Map.ShowMap();
                Console.SetCursorPosition(Cursor.Position.X / 16, Cursor.Position.Y / 37+1);
                cord c = new cord(Console.CursorLeft, Console.CursorTop);
                Map.Step(c);//подружить с событиями и делегатами
                Console.SetCursorPosition(Cursor.Position.X / 16, Cursor.Position.Y / 37);
                c = new cord(Console.CursorLeft, Console.CursorTop);
                if (Console.KeyAvailable)
                {
                    t = Console.ReadKey(true);
                    if (t.Key == ConsoleKey.Escape)
                        exit = true;
                    else
                        Press(t, c);
                }
                watch.Stop();
                Console.WriteLine(watch.Elapsed);
                if (watch.ElapsedMilliseconds < 200)
                    Thread.Sleep((int)(200 - watch.ElapsedMilliseconds));
                watch.Reset();
            }
        }
    }
}