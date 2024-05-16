using System.Diagnostics.Metrics;
using Сcs_Lab5_01.Objects;
using System.Reflection;
using System.Security.Cryptography;

namespace Сcs_Lab5_01
{
    public partial class Form1 : Form
    {
        MyRectangle myRect;
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        GoalCircle goalCircle;
        GoalCircle goalCircle2;

        public Form1()
        {
            InitializeComponent();

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

            player.OnGoalCircleOverlap += (gr) =>
            {
                objects.Remove(gr);
                goalCircle = null;
                enumerator();
                GoalCircle();
            };

            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);

            goalCircle = new GoalCircle(pbMain.Width / 10, pbMain.Height / 10, 0);
            goalCircle2 = new GoalCircle(pbMain.Width / 5 + 6, pbMain.Height / 5 + 10, 0);
            objects.Add(goalCircle);
            objects.Add(goalCircle2);

            objects.Add(marker);
            objects.Add(player);

            goalCircle.onDeath += (c) =>
            {
                Random rnd = new Random();
                c.X = rnd.Next(1, 500);
                c.Y = rnd.Next(1, 500);

            };
            goalCircle2.onDeath += (c) =>
            {
                Random rnd = new Random();
                c.X = rnd.Next(1, 500);
                c.Y = rnd.Next(1, 500);

            };
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

        private void pbMain_Click(object sender, EventArgs e)
        {

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

        private void GoalCircle()
        {
            Random rnd = new Random();
            int value = rnd.Next(1, 10);
            Random rnd2 = new Random();
            int value2 = rnd2.Next(1, 10);

            if (goalCircle == null)
            {
                goalCircle = new GoalCircle(pbMain.Width / value, pbMain.Height / value2, 0);
                objects.Add(goalCircle);
                goalCircle.onDeath += (c) =>
                {
                    Random rnd = new Random();
                    c.X = rnd.Next(1, 500);
                    c.Y = rnd.Next(1, 500);

                };
            }
        }

        int counter = 0;
        private void enumerator()
        {

            label1.Text = string.Empty;
            if (goalCircle == null)
            {
                counter += 1;
            }
            label1.Text = string.Format("Очки: ") + counter.ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
