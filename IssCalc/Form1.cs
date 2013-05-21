using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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

            loaded = true;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            ResizeAll();
        }

        private void ResizeAll()
        {
            glControl1.Width = Width / 2 - 12;
            glControl1.Left = Width / 2 - 12;
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


        double angle, rotation_speed = 0.5;
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded) {
                return;
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

            angle += rotation_speed; //* (float)e.Time;
            GL.Rotate(angle, 0.0f, 1.0f, 0.0f);

            DrawGrid(10, 10, 1, 1, Color.White);

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
        }
    }
}
