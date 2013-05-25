using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IssCalcCore.Tokens;
using OpenTK.Graphics.OpenGL;
using OpenTK.Math;
using OpenTK;
using IssCalcCore;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        bool loaded;
        static Font fnt = new Font("Arial", 12);
        OpenTK.Graphics.TextPrinter tx = new OpenTK.Graphics.TextPrinter();
        Random rnd = new Random();
        Graph3DData mg = new Graph3DData(100, 100);
        private Tuple<double, double> zoom = new Tuple<double, double>(1, 1); 
        private Tuple<double, double> offset = new Tuple<double, double>(0, 0);

        private bool NeedRecalc;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(System.Object sender, System.EventArgs e)
        {
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Multisample);

            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.ShadeModel(ShadingModel.Smooth);

            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Fastest);
            GL.Enable(EnableCap.PolygonSmooth);

            GL.Hint(HintTarget.PointSmoothHint, HintMode.Fastest);
            GL.Enable(EnableCap.PointSmooth);

            GL.Hint(HintTarget.LineSmoothHint, HintMode.Fastest);
            GL.Enable(EnableCap.LineSmooth);

            //GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.AlphaTest);

            ResizeAll();

            this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);
            this.MouseMove += new MouseEventHandler(Form1_MouseMove);
            this.KeyPress += new KeyPressEventHandler(Form1_KeyPress);
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            glControl1.MouseMove += new MouseEventHandler(Form1_MouseMove);
            glControl1.MouseWheel += new MouseEventHandler(Form1_MouseWheel);
            glControl1.KeyPress += new KeyPressEventHandler(Form1_KeyPress);
            glControl1.KeyDown += new KeyEventHandler(Form1_KeyDown);

            loaded = true;
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A) {
                angle-=3;
            }
            if (e.KeyCode == Keys.D) {
                angle+=3;
            }
            if (e.KeyCode == Keys.W) {
                angle2 += 3;
            }
            if (e.KeyCode == Keys.S) {
                angle2 -= 3;
            }
        }

        void Form1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if(e.KeyChar == 'r') {
                offset = new Tuple<double, double>(0, 0);
                zoom = new Tuple<double, double>(1, 1);
                angle = 0;
                angle2 = 0;
                NeedRecalc = true;
            }

            label4.Text = string.Format("xoff {0} yoff {1}", offset.Item1, offset.Item2);
            label3.Text = string.Format("xzoom {0} yzoom {1}", zoom.Item1, zoom.Item2);
        }

        private Point mlast;
        void Form1_MouseMove(object sender, MouseEventArgs e) {

            if (e.Button == MouseButtons.Left) {
                float an = MathHelper.DegreesToRadians((float) angle);
                float x = (mlast.X - e.X)/200f;
                float y = (mlast.Y - e.Y)/200f;
                offset = new Tuple<double, double>(offset.Item1 + x*Math.Cos(an) - y*Math.Sin(an),
                                                   offset.Item2 + x*Math.Sin(an) + y*Math.Cos(an));




                NeedRecalc = true;

                label4.Text = string.Format("xoff {0} yoff {1}", offset.Item1, offset.Item2);
            }
            mlast.Y = e.Y;
            mlast.X = e.X;
        }

        void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            offset = new Tuple<double, double>(offset.Item1 / zoom.Item1, offset.Item2 / zoom.Item2);
            if (e.Delta > 0) {
                zoom = new Tuple<double, double>(zoom.Item1 * 1.1, zoom.Item2 * 1.1);
            }
            if (e.Delta < 0) {
                zoom = new Tuple<double, double>(zoom.Item1 / 1.1, zoom.Item2 / 1.1);
            }

            offset = new Tuple<double, double>(offset.Item1 * zoom.Item1, offset.Item2 * zoom.Item2);
            NeedRecalc = true;
            label4.Text = string.Format("xoff {0} yoff {1}", offset.Item1, offset.Item2);
            label3.Text = string.Format("xzoom {0} yzoom {1}", zoom.Item1, zoom.Item2);
        }

        private void RecalcG3DData() {
            var a =
                ExpressionParser.TokenArrayRPN(
                    ExpressionParser.StringArrayToTokenArray(
                        ExpressionParser.ExpressionStringToStringArray(textBox1.Text)));

            for (int i = 0; i < mg.w; i++) {
                for (int j = 0; j < mg.h; j++) {
                    mg.data[i * mg.w + j] = ExpressionParser.Caclulate(a, ((double)i / mg.w + offset.Item1) / zoom.Item1 - 2.0, ((double)j / mg.h + offset.Item2) / zoom.Item2 - 2.0, 0, 0);
                }
            }
            NeedRecalc = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            ResizeAll();
        }

        private void ResizeAll()
        {
            glControl1.Width = (Width / 3) * 2 - 12;
            glControl1.Left = Width / 3 - 12;
            glControl1.Height = Height - 24 - 24 - 8;
            glControl1.Top = 12;

            SetupViewport();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {

        }

        private void SetupViewport()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();
            //GL.Ortho(0, w, 0, h, -1, 1);
            GL.Viewport(0, 0, w, h);
            double aspect_ratio = w / (double)h;

            OpenTK.Matrix4 perspective = OpenTK.Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);
        }

        private void DrawCube()
        {
            GL.Begin(BeginMode.Quads);

            GL.Color3(Color.Silver);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);

            GL.Color3(Color.Honeydew);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);

            GL.Color3(Color.Moccasin);

            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);

            GL.Color3(Color.IndianRed);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);

            GL.Color3(Color.PaleVioletRed);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);

            GL.Color3(Color.ForestGreen);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);

            GL.End();
        }


        double angle, angle2, rotation_speed = 0.5;
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded) {
                return;
            }

            if(NeedRecalc) {
                RecalcG3DData();
            }

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();
            //GL.Color3(Color.Yellow);
            //GL.Begin(BeginMode.Triangles);
            //GL.Vertex2(0, 20);
            //GL.Vertex2(100, 20);
            //GL.Vertex2(100, 50);
            //GL.End();

            OpenTK.Matrix4 lookat = OpenTK.Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            GL.Scale(2,2,2);

            //angle += rotation_speed; //* (float)e.Time;
            GL.Rotate(angle, 0.0f, 1.0f, 0.0f);
            GL.Rotate(angle2, 0.0f, 0.0f, 1.0f);

            DrawGrid(10, 10, 1, 1, Color.White);
            Draw3DGraph(mg, Draw3DMode.Trias);

            //DrawCube();
            //DrawStringInScreenCoord(tx, "SOme text", fnt, Color.White, 10, 10);

            glControl1.SwapBuffers();

        }

        private void DrawGrid(float counth, float countw, float w, float h, Color c)
        {


            GL.Begin(BeginMode.Lines);

            for (int i = 0; i <= counth; i++) {
                GL.Color3(c);
                GL.Vertex3(-1, 0, (i / countw) * 2 - 1);
                GL.Vertex3(1, 0, (i / countw) * 2 - 1);
            }

            for (int i = 0; i <= countw; i++) {
                GL.Color3(c);
                GL.Vertex3((i / countw) * 2 - 1, 0, -1);
                GL.Vertex3((i / countw) * 2 - 1, 0, 1);
            }

            GL.End();
        }

        enum Draw3DMode
        {
            Dots,
            Trias,
            Lines
        }

        private void Draw3DGraph(Graph3DData gr, Draw3DMode mode)
        {

            double maxgr = gr.data.Max();
            double mingr = gr.data.Min();

            if (maxgr - mingr == 0) maxgr = 0.01;

            if (mode == Draw3DMode.Trias) {
                GL.Begin(BeginMode.Triangles);

                for (int i = 0; i < gr.w - 1; i++) {
                    for (int j = 0; j < gr.h - 1; j++) {
                        var cc = 240 - ((gr.data[i*gr.w + j] - mingr)/(maxgr - mingr))*240.0;
                        if (cc < 0 || cc > 360) cc = 180;
                       // GL.Color3(ColorFromHSV(cc, 1, 1));
                        GL.Color3(gr.data[i * gr.w + j]/maxgr, 0, 1 - gr.data[i * gr.w + j]/maxgr);
                        GL.Vertex3((i / (double)gr.w) * 2 - 1, gr.data[i * gr.w + j] / 2.0, (j / (double)gr.h) * 2 - 1);
                        GL.Color3(gr.data[(i + 1) * gr.w + j] / maxgr, 0, 1 - gr.data[(i + 1) * gr.w + j] / maxgr);
                        GL.Vertex3(((i + 1) / (double)gr.w) * 2 - 1, gr.data[(i + 1) * gr.w + j] / 2.0, (j / (double)gr.h) * 2 - 1);
                        GL.Color3(gr.data[(i) * gr.w + j + 1] / maxgr, 0, 1 - gr.data[(i) * gr.w + j + 1] / maxgr);
                        GL.Vertex3(((i) / (double)gr.w) * 2 - 1, gr.data[(i) * gr.w + j + 1] / 2.0,
                                   ((j + 1) / (double)gr.h) * 2 - 1);

                        GL.Color3(gr.data[(i) * gr.w + j + 1] / maxgr, 0, 1 - gr.data[(i) * gr.w + j + 1] / maxgr);
                        GL.Vertex3(((i) / (double)gr.w) * 2 - 1, gr.data[(i) * gr.w + (j + 1)] / 2.0,
                                   ((j + 1) / (double)gr.h) * 2 - 1);
                        GL.Color3(gr.data[(i + 1) * gr.w + j] / maxgr, 0, 1 - gr.data[(i + 1) * gr.w + j] / maxgr);
                        GL.Vertex3(((i + 1) / (double)gr.w) * 2 - 1, gr.data[(i + 1) * gr.w + j] / 2.0, (j / (double)gr.h) * 2 - 1);
                        GL.Color3(gr.data[(i + 1) * gr.w + j + 1] / maxgr, 0, 1 - gr.data[(i + 1) * gr.w + j + 1] / maxgr);
                        GL.Vertex3(((i + 1) / (double)gr.w) * 2 - 1, gr.data[(i + 1) * gr.w + j + 1] / 2.0,
                                   ((j + 1) / (double)gr.h) * 2 - 1);
                    }
                }

                GL.End();
            } else if (mode == Draw3DMode.Dots) {
                GL.Begin(BeginMode.Points);

                for (int i = 0; i < gr.w - 1; i++) {
                    for (int j = 0; j < gr.h - 1; j++) {

                        GL.Color3(Color.Red);
                        GL.Vertex3((i / (double)gr.w) * 2 - 1, gr.data[i * gr.w + j] / 2.0, (j / (double)gr.h) * 2 - 1);
                    }
                }

                GL.End();
            } else if (mode == Draw3DMode.Lines) {
                GL.Begin(BeginMode.Lines);

                for (int i = 0; i < gr.w - 1; i++) {
                    for (int j = 0; j < gr.h - 1; j++) {

                        GL.Color3(Color.Red);
                        GL.Vertex3((i / (double)gr.w) * 2 - 1, gr.data[i * gr.w + j] / 2.0, (j / (double)gr.h) * 2 - 1);
                        GL.Vertex3((i / (double)gr.w) * 2 - 1, gr.data[i * gr.w + j + 1] / 2.0, ((j + 1) / (double)gr.h) * 2 - 1);
                    }
                }

                GL.End();
            }
        }

        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        private void DrawStringInScreenCoord(OpenTK.Graphics.TextPrinter printer, string text, Font font, Color c, float x, float y)
        {
            printer.Begin();

            GL.PushMatrix();
            GL.Translate(x, y, 0);

            printer.Print(text, font, c);

            GL.PopMatrix();

            printer.End();
        }

        private void DrawStringInWorldCoord(OpenTK.Graphics.TextPrinter printer, string text, Font font, Color c, float x, float y)
        {
            printer.Begin();

            //GL.PopMatrix();
            //GL.Translate(x, y, 0);
            // GL.MatrixMode(MatrixMode.Modelview);

            //  OpenTK.Matrix4 lookat = OpenTK.Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Rotate(angle, 0.0f, 1.0f, 0.0f);

            printer.Print(text, font, c);


            //GL.PushMatrix();

            printer.End();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            glControl1.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label2.Text = ExpressionParser.GetTokenizedRPNString(textBox1.Text);

            RecalcG3DData();
        }

        private void glControl1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            mg = new Graph3DData(trackBar1.Value, trackBar1.Value);
            NeedRecalc = true;
            label5.Text = string.Format("{0}x{1} {2} triangles", mg.w, mg.h, mg.w * mg.h * 2);
        }
    }
}
