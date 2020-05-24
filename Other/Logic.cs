using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Other
{
    class logic
    {
        int time = 0;
        internal Bot bot = null;
        cord target = null;
        internal Move move = null;
        internal Attack attack = null;
        internal Stats stats = null;
        internal logic(Bot b){
            bot = b;
            stats = new Stats(this);
            move = new Move(this);
            attack = new Attack(this);
        }
        public void SetMove(cord a)
        {

        }
        public void SetTarget()
        {

        }
        public void Do()
        {
            move.Do();
            attack.Do();
            stats.Do();
        }
    }
    class Move
    {
        logic logic = null;
        Stats stats = null;
        decimal s = 0;
        int i = 0;
        List<Timer> braking = new List<Timer>();
        List<Timer> acceleration = new List<Timer>();
        cord target = null;
        List<cord> cords = null;
        public void Do()
        {
            for (int i = 0; i < braking.Count; i++)
                if (braking[i].time)
                {
                    stats.i = 0;
                    EngineReverse();
                    braking.Remove(braking[i--]);
                }
            for (int i = 0; i < acceleration.Count; i++)
                if (acceleration[i].time)
                {
                    stats.i = 0;
                    EngineStart();
                    acceleration.Remove(acceleration[i--]);
                }
            if (stats.move == true)
            {
                if (stats.speed == stats.maxSpeed && stats.engine == Stats.Engine.acceleration) { EngineStop(); stats.i = 0; }
                if (stats.speed == 0 && cords.Count - i < 3) stats.position = target;
                if (
                    (target == null || stats.position == null)
                    ||(stats.position == target)
                )
                {
                    stats.i = 0;
                    StopMove();
                    return;
                }
                s += stats.speed / 1000;
                i = (int)Math.Truncate(s);
                if (i >= cords.Count) i = cords.Count - 1;
                stats.position = cords[i];
            }
        }
        public void SpeedCalculate()
        {
            int s = cords.Count;
            decimal accpath = ((stats.maxSpeed*stats.maxSpeed - stats.speed* stats.speed) / (2 * stats.acceleration))/1000;
            if (2 * accpath > s)
            {
                int time = (int)(((decimal)Math.Sqrt((double)(stats.speed*stats.speed+2*stats.acceleration*(s/2)))-stats.speed)/stats.acceleration);
                braking.Add(new Timer((int)(time)));
            }
            else
            {
                decimal acctime = (stats.maxSpeed-stats.speed)/stats.acceleration;
                accpath = (stats.maxSpeed*stats.maxSpeed-stats.speed*stats.speed) / (2 * stats.acceleration);
                decimal path = s*1000 - 2 * accpath;
                decimal noacceltime = path/stats.maxSpeed;
                double time = Math.Ceiling((double)(noacceltime + acctime));
                braking.Add(new Timer((int)time));
            }
        }
        public void SetMove(cord c)
        {
            cords = new List<cord>();
            cords.Add(stats.position);
            cords.AddRange(Map.Line(stats.position, c).ToList());
            cords.Add(c);
            target = c;
            SpeedCalculate();
            stats.move = true;
            EngineStart();
        }
        void StopMove()
        {
            EngineStop();
            stats.speed = 0;
            stats.move = false;
            target = null;
            cords = null;
            s = 0;
        }
        void EngineStart() => stats.engine = Stats.Engine.acceleration;
        void EngineStop() => stats.engine = Stats.Engine.offline;
        void EngineReverse() => stats.engine = Stats.Engine.breaking;
        internal Move(logic l) {
            logic = l;
            stats = logic.stats;
        }
    }
    internal class Attack
    {
        cord target = null;
        logic logic = null;
        internal Attack(logic l) => logic = l;
        internal void Do() { }
    }
    internal class Stats//parse from file
    {
        internal enum Engine { offline, acceleration, breaking };
        public int i = 0;
        logic logic = null;
        public ConsoleColor color = ConsoleColor.Blue;
        internal Stats(logic l) => logic = l;
        internal bool move = false;
        internal Map.Teams team = Map.Teams.friend;
        internal Engine engine = Engine.offline;
        internal decimal maxSpeed = 5000M;//cell per 1000 tick -->1s for 2 cell-->0.2 cell for 1 tick
        internal decimal acceleration = 500M;//per 1000 tick -->2s for max speed
        internal void Do()
        {
            i++;
            if (engine == Engine.acceleration)
            {
                speed += acceleration;
                move = true;
            }
            if (engine == Engine.breaking)
            {
                speed -= acceleration;
                move = true;
            }
            Console.Write(i);
            Console.Write(engine);
            Console.Write(speed);
        }
        internal cord position
        {
            get { return logic.bot.pos; }
            set { logic.bot.pos = value; }
        }
        internal decimal speed
        {
            get{ return logic.bot.speed; }
            set{
                if (value < 0) logic.bot.speed = 0;
                else
                if (value > maxSpeed) logic.bot.speed = maxSpeed;
                else logic.bot.speed = value;
            }
        }
    }
}
