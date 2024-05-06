using System.Diagnostics.CodeAnalysis;
using WinFormsApp5.Objects;

namespace WinFormsApp5
{
    public partial class Form1 : Form
    {
        int score = 0;
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        Target target1;
        Target target2;

        public Form1()
        {
            InitializeComponent();
            scoreLable.Text = "Очки: " + score;

            target1 = new Target(randomNumber(), randomNumber(), 0);
            target1.OnTimerOverlap += Target_OnTimerOverlap;

            target2 = new Target(randomNumber(), randomNumber(), 0);
            target2.OnTimerOverlap += Target_OnTimerOverlap;

            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
            };
            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };
            player.OnTargetOverlap += (t) =>
            {
                score += 1;
                scoreLable.Text = "Очки: " + score;
                t.X = randomNumber();
                t.Y = randomNumber();
            };

            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);
            objects.Add(marker);
            objects.Add(player);
            objects.Add(target1);
            objects.Add(target2);
        }

        private void Target_OnTimerOverlap(Target target)
        {
            target.X = randomNumber();
            target.Y = randomNumber();
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(Color.White);
            updatePlayer();

            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }
            }

            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;
                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }

            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            player.X += player.vX;
            player.Y += player.vY;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker);
            }
            marker.X = e.X;
            marker.Y = e.Y;
        }

        private float randomNumber()
        {
            Random random = new Random();
            float value = random.Next(10, 490);
            return value;
        }
    }
}