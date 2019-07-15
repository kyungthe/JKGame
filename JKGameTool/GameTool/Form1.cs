using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTool
{
    public partial class Form1 : Form
    {
        // 점선의 점 간격
        private float dotInterval = 2.0f;
        // 격자 간격(Point 단위)
        private float gridInterval;
        private float pixelPerPoint;
        private int point = 20;
        private Pen pen;
        private State currentState;

        public Form1()
        {
            InitializeComponent();
            InitializePaintValues();
            currentState = new CreateUINoneState(this);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        private void InitializePaintValues()
        {
            Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
            Color color = ForeColor;
            pen = new Pen(color);

            pen.Width = dotInterval;

            // Point : Pixel = 72 : DPI
            // Point를 알고 있을 때 Pixel = (Point * DPI) / 72
            float dpi = graphics.DpiX;
            pixelPerPoint = dpi / 72;
            gridInterval = pixelPerPoint * point;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawGrid(e.Graphics);
            currentState.Draw(e.Graphics);
        }

        private void ChangeState(State state)
        {
            currentState.Exit();

            currentState = state;

            currentState.Enter();
        }

        private void CreateButtonButton_Click(object sender, EventArgs e)
        {
            ChangeState(new CreateUIButtonState(this));
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
            case Keys.Escape:
                    ChangeState(new CreateUINoneState(this));
                    break;
            default:
                    break;
            }
        }

        public void DrawGrid(Graphics graphics)
        {
            float startX = Width - ClientSize.Width;
            float startY = Height - ClientSize.Height;
            float width = CreateButtonButton.Location.X - 10;
            float height = ClientSize.Height;

            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            //세로 그리기
            for (float x = startX / gridInterval; x < width; x += gridInterval)
            {
                graphics.DrawLine(pen, x, 0, x, height);
            }

            //가로 그리기
            for (float y = startY / gridInterval; y < height; y += gridInterval)
            {
                graphics.DrawLine(pen, 0, y, width, y);
            }
        }

        public void DrawButton(Graphics graphics, int startX, int startY, int endX, int endY)
        {
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

            int width = endX - startX;
            int height = endY - startY;

            if ((width <= 0) || (height <= 0))
            {
                return;
            }
            
            graphics.DrawRectangle(pen, new Rectangle(startX, startY, width, height));
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            currentState.MouseDown(e.X, e.Y);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            currentState.MouseMove(e.X, e.Y);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            currentState.MouseUp();
        }
    }
}
