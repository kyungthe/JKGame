using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTool
{
    class Grid
    {
        private float x;
        private float y;
        private float interval;

        public Grid(float x, float y, float interval)
        {
            this.x = x;
            this.y = y;
            this.interval = interval;
        }

        public void DrawGrid(Graphics graphics, Pen pen)
        {
            float pixelX = x * interval;
            float pixelY = y * interval;

            graphics.DrawLine(pen, pixelX,              pixelY,             pixelX + interval,  pixelY);
            graphics.DrawLine(pen, pixelX,              pixelY,             pixelX,             pixelY + interval);
            graphics.DrawLine(pen, pixelX + interval,   pixelY,             pixelX + interval,  pixelY + interval);
            graphics.DrawLine(pen, pixelX,              pixelY + interval,  pixelX + interval,  pixelY + interval);
        }
    }

    class GridManager
    {
        private List<Grid> gridList;

        public GridManager()
        {
            gridList = new List<Grid>();
        }

        public void AddGrid(Grid grid)
        {
            gridList.Add(grid);
        }

        public void DrawGrid(Graphics graphics, Pen pen)
        {
            foreach(Grid grid in gridList)
            {
                grid.DrawGrid(graphics, pen);
            }
        }
    }
}
