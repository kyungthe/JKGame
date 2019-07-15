using System.Drawing;
using System.Windows.Forms;

namespace GameTool
{
    abstract class State
    {
        protected Form1 parentForm = null;

        protected State(Form1 parentForm)
        {
            this.parentForm = parentForm;
        }

        public abstract void Enter();
        public abstract void MouseDown(int x, int y);
        public abstract void MouseMove(int x, int y);
        public abstract void MouseUp();
        public abstract void Draw(Graphics graphics);
        public abstract void Exit();
    }

    class CreateUINoneState : State
    {
        public CreateUINoneState(Form1 parentForm) : base(parentForm)
        {
        }

        public override void Enter()
        {
            parentForm.Cursor = Cursors.Default;
        }
        public override void MouseDown(int x, int y) { }
        public override void MouseMove(int x, int y) { }
        public override void MouseUp() { }
        public override void Draw(Graphics graphics) { }
        public override void Exit() { }
    }

    class CreateUIButtonState : State
    {
        private int startX = 0;
        private int startY = 0;
        private int endX = 0;
        private int endY = 0;
        private bool isClicked = false;

        public CreateUIButtonState(Form1 parentForm) : base(parentForm)
        {
        }

        public override void Enter()
        {
            parentForm.Cursor = Cursors.Cross;
        }

        public override void MouseDown(int x, int y)
        {
            startX = x;
            startY = y;
            isClicked = true;
        }

        public override void MouseMove(int x, int y)
        {
            if(isClicked)
            {
                endX = x;
                endY = y;
                parentForm.Invalidate();
            }
        }

        public override void MouseUp()
        {
            isClicked = false;
        }

        public override void Draw(Graphics graphics)
        {
            if (isClicked)
            {
                parentForm.DrawButton(graphics, startX, startY, endX, endY);
            }
        }

        public override void Exit() { }
    }
}
