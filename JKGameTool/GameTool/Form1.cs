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
        private Pen pen;
        private State currentState;
        private GridManager gridManager;

        public Form1()
        {
            InitializeComponent();
            InitializeGrid();
            currentState = new CreateUINoneState(this);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        private void InitializeGrid()
        {
            Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
            Color color = ForeColor;
            pen = new Pen(color);

            pen.Width = 2.0f;

            // Point : Pixel = 72 : DPI
            // Point를 알고 있을 때 Pixel = (Point * DPI) / 72
            float dpi = graphics.DpiX;
            float pixelPerPoint = dpi / 72;
            int point = 20;
            float gridPixelInterval = pixelPerPoint * point;

            int startX = 0;
            int startY = 0;
            int width = (int)((CreateButtonButton.Location.X - 10) / gridPixelInterval);
            int height = (int)(ClientSize.Height / gridPixelInterval);

            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            gridManager = new GridManager();

            //세로 그리기
            for (int x = startX; x < width; ++x)
            {
                for (int y = startY; y < height; ++y)
                {
                    Grid grid = new Grid(x, y, gridPixelInterval);
                    gridManager.AddGrid(grid);
                }
            }
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
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            gridManager.DrawGrid(graphics, pen);
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
