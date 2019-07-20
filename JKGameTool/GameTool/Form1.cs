using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

        internal GridManager GridManager { get => gridManager; set => gridManager = value; }

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

            // 점선의 점 간격
            pen.Width = 2.0f;

            // Point : Pixel = 72 : DPI
            // Point를 알고 있을 때 Pixel = (Point * DPI) / 72
            float dpi = graphics.DpiX;
            float pixelPerPoint = dpi / 72;
            int point = 20;
            int gridPixelInterval = (int)pixelPerPoint * point;

            int startX = 0;
            int startY = 0;
            int width = (int)((CreateButtonButton.Location.X - 10) / gridPixelInterval);
            int height = (int)(ClientSize.Height / gridPixelInterval);

            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            GridManager = new GridManager(gridPixelInterval);

            //세로 그리기
            for (int x = startX; x < width; ++x)
            {
                for (int y = startY; y < height; ++y)
                {
                    Grid grid = new Grid(x, y, x + 1, y + 1);
                    GridManager.AddGrid(grid);
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

            GridManager.DrawGrid(graphics, pen);
        }

        public void DrawButton(Graphics graphics, int startX, int startY, int endX, int endY)
        {
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

            int x = startX / gridManager.GridPixelInterval;
            int y = startY / gridManager.GridPixelInterval;
            int width = endX / gridManager.GridPixelInterval;
            int height = endY / gridManager.GridPixelInterval;

            Debug.WriteLine("x : {0}, y : {1}, width : {2}, height : {3}", x, y, width, height);

            Grid grid = new Grid(x, y, width, height);
            grid.DrawGrid(graphics, pen, gridManager.GridPixelInterval);
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
