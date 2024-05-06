using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WinFormsApp5.Objects
{
    class Target : BaseObject
    {
        Random random;
        System.Timers.Timer timer;
        DateTime startTime;
        public Action<Target> OnTimerOverlap;

        public Target(float x, float y, float angle) : base(x, y, angle)
        {
            random = new Random();
            timer = new System.Timers.Timer(random.Next(5000, 20000));
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
            startTime = DateTime.Now;
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.GreenYellow), -15, -15, 30, 30);
            TimeSpan remainingTime = (startTime.AddMilliseconds(timer.Interval) - DateTime.Now);
            g.DrawString(
                (Math.Round(remainingTime.TotalSeconds)).ToString(),
                new Font("Verdana", 8),
                new SolidBrush(Color.Green),
                10, 10
            );
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-15, -15, 30, 30);
            return path;
        }
        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            timer.Interval = random.Next(5000, 20000);
            startTime = DateTime.Now;
            OnTimerOverlap?.Invoke(this);
        }

    }

}
