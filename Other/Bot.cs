using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Other
{
    class Bot
    {
        logic logic = null;
        internal decimal speed = 0;
        public char c = 'w';
        public cord pos = new cord(0, 0);
        Map.Type type = Map.Type.support;
        Bot()
        {
            Map.bots.Add(this);
            logic = new logic(this);
        }
        public Bot(cord z, char a = 'w'):this()
        {
            pos = z;
            c = a;
        }
        ~Bot() =>Map.bots.Remove(this);
        public void Move(cord c)
        {
            logic.move.SetMove(c);
        }
        public void Do()
        {
            logic.Do();
        }
        public Obj[] ToObj()
        {
            return new Obj[] { new Obj(c, pos, logic.stats.color) };
        }
    }
}
