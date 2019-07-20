using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTool
{
    class Grid
    {
        private int startX;
        private int startY;
        private int endX;
        private int endY;

        public int X { get => startX; set => startX = value; }
        public int Y { get => startY; set => startY = value; }

        public Grid(int startX, int startY, int endX, int endY)
        {
            this.startX = startX;
            this.startY = startY;
            this.endX = endX;
            this.endY = endY;
        }

        public void DrawGrid(Graphics graphics, Pen pen, int pixelInterval)
        {
            float pixelStartX = startX * pixelInterval;
            float pixelStartY = startY * pixelInterval;
            float pixelEndX = endX * pixelInterval;
            float pixelEndY = endY * pixelInterval;

            graphics.DrawLine(pen, pixelStartX, pixelStartY, pixelEndX, pixelStartY);
            graphics.DrawLine(pen, pixelStartX, pixelStartY, pixelStartX, pixelEndY);
            graphics.DrawLine(pen, pixelStartX, pixelEndY, pixelEndX, pixelEndY);
            graphics.DrawLine(pen, pixelEndX, pixelStartY, pixelEndX, pixelEndY);
        }
    }

    class GridManager
    {
        private List<Grid> gridList;
        private int gridPixelInterval;

        public int GridPixelInterval { get => gridPixelInterval; set => gridPixelInterval = value; }

        public GridManager(int gridPixelInterval)
        {
            gridList = new List<Grid>();
            SetGridPixelInterval(gridPixelInterval);
        }

        public void AddGrid(Grid grid)
        {
            gridList.Add(grid);
        }

        public void DrawGrid(Graphics graphics, Pen pen)
        {
            foreach(Grid grid in gridList)
            {
                grid.DrawGrid(graphics, pen, gridPixelInterval);
            }
        }

        public void SetGridPixelInterval(int gridPixelInterval)
        {
            this.gridPixelInterval = gridPixelInterval;
        }

        public Grid FindGrid(int pixelX, int pixelY)
        {
            int x = pixelX / (int)gridPixelInterval;
            int y = pixelY / (int)gridPixelInterval;

            foreach(Grid grid in gridList)
            {
                if((grid.X == x) && (grid.Y == y))
                {
                    return grid;
                }
            }

            Debug.WriteLine("Grid를 찾을 수 없다! pixelX : {0}, pixelY : {1}, x : {2}, y : {3}", pixelX, pixelY, x, y);
            return null;
        }
    }
}
