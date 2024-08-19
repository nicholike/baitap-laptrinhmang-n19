using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawLuckyWheel
{
    public partial class Form4 : Form
    {
        Timer fireworksTimer;
        FireWork[] fireworks;
        static Random rand = new Random();
        const int MaxFireWorks = 10;
        const int UpdateInterval = 30;

        public Form4()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            // Make the form transparent
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;
            this.TransparencyKey = Color.Black;

            // Set up the label to show the result in the center
 

            // Initialize fireworks
            fireworksTimer = new Timer();
            fireworksTimer.Tick += new EventHandler(Tick);
            fireworksTimer.Interval = UpdateInterval;
            fireworksTimer.Start();

            fireworks = new FireWork[MaxFireWorks];

            // Set form properties to make it look better
            this.StartPosition = FormStartPosition.CenterScreen; // Start in the center of the screen
            this.Size = new Size(800, 600); // Set the size of the form
        }

        // The timer tick method to update fireworks
        private void Tick(Object o, EventArgs e)
        {
            for (int i = 0; i < MaxFireWorks; ++i)
            {
                if (fireworks[i] != null)
                {
                    if (!fireworks[i].Update())
                    {
                        fireworks[i] = null;
                    }
                }
            }

            // Randomly create new fireworks
            if (rand.Next(10) == 0)
            {
                for (int i = 0; i < MaxFireWorks; ++i)
                {
                    if (fireworks[i] == null)
                    {
                        fireworks[i] = new FireWork(ClientRectangle.Width, ClientRectangle.Height);
                        break;
                    }
                }
            }

            Invalidate(); // Redraw the form
            Update();     // Update the form
        }

        // Override OnPaint to draw fireworks
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(Color.Transparent);

            foreach (FireWork fw in fireworks)
            {
                if (fw != null)
                {
                    fw.Paint(e.Graphics);
                }
            }
        }

        // Firework class to create a firework effect
        public class FireWork
        {
            private Point position;
            private List<Particle> particles;
            private Color color;
            private static Random rand = new Random();
            private int lifespan;

            public FireWork(int width, int height)
            {
                position = new Point(rand.Next(0, width), rand.Next(0, height / 2));
                particles = new List<Particle>();
                color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)); // Random color
                lifespan = 100; // Firework lifespan

                // Create particles
                for (int i = 0; i < 50; i++)
                {
                    particles.Add(new Particle(position, color));
                }
            }

            public bool Update()
            {
                lifespan--;
                foreach (var particle in particles)
                {
                    particle.Update();
                }
                return lifespan > 0;
            }

            public void Paint(Graphics g)
            {
                foreach (var particle in particles)
                {
                    particle.Paint(g);
                }
            }
        }

        // Particle class to represent each firework particle
        public class Particle
        {
            private PointF position;
            private PointF velocity;
            private Color color;
            private int size;
            private static Random rand = new Random();

            public Particle(Point origin, Color fireworkColor)
            {
                position = new PointF(origin.X, origin.Y);
                velocity = new PointF((float)(rand.NextDouble() - 0.5) * 5, (float)(rand.NextDouble() - 0.5) * 5);
                size = rand.Next(2, 6);
                color = Color.FromArgb(rand.Next(150, 256), fireworkColor); // Random transparency and color
            }

            public void Update()
            {
                position.X += velocity.X;
                position.Y += velocity.Y;
                velocity.Y += 0.1f; // Gravity effect
            }

            public void Paint(Graphics g)
            {
                using (Brush b = new SolidBrush(color))
                {
                    g.FillEllipse(b, position.X, position.Y, size, size); // Draw the particle
                }
            }
        }

    }
}
