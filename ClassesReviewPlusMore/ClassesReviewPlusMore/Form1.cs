using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GDIDrawer;

namespace ClassesReviewPlusMore
{   
    public partial class Form1 : Form
    {
        CDrawer canvas;             // The surface in which the Balls will be drawn
        List<Ball> balls;           // A collection objects for holding all our Ball objects
        Random rng = new Random();  // Used for generating random values when creationg the Ball objects
        public Form1()
        {
            InitializeComponent();

            // Initialize the fields not already initialized wehn declared above.
            canvas = new CDrawer();
            balls = new List<Ball>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            canvas.BBColour = Color.Beige;  // Set the background of the drawing window to Beige.
        }


        private void _btnStart_Click(object sender, EventArgs e)
        {
            // Generate 100 Ball objects with randomized values, and save them to the List for holding Ball objects.
            for (int i = 0; i < 100; ++i)
            {
                // While generating the Ball objects, it is possible that some random values as specified below
                // will cause exceptions to be thrown.  The messages generated are printed to the Output window.
                try
                {
                    // Create a Ball object
                    Ball temp = new Ball(rng.NextDouble() * 70 - 10,
                                            new Point(rng.Next(-50, 851), rng.Next(-50, 651)),
                                            Color.FromArgb(rng.Next(256), rng.Next(256), rng.Next(256)));

                    balls.Add(temp);    // Add the Ball object to the collection assuming no exception is thrown.

                    DrawBalls();        // Display all Ball objects to the CDrawer canvas window.
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);  // If an illegal random value is generated, print the resulting message to the 
                                                    // output window.
                }
            }
        }


        // Helper method for drawing each frame.
        private void DrawBalls()
        {
            canvas.Clear();
            foreach (Ball b in balls)
            {
                canvas.AddCenteredEllipse(b.Centre,(int)b.Radius * 2, (int)b.Radius * 2, b.Colour);
            }
        }

        // Timer for moving all Ball objects, then drawing the next frame with the Ball objects in their
        // new locations.
        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach(Ball b in balls)
            {
                b.Move();
            }

            DrawBalls();
        }

        // Immediately change the static (shared) ExtraVelocity of the Ball objects.  Note the interesting movement 
        // changing this value causes.
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Ball.ExtraVelocity = trackBar1.Value;
        }
    }
}
