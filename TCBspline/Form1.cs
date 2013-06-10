using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace TCBspline
{
    public partial class Form1 : Form
    {
        Drawer drawer;
        PointsLogic logic;

        public Form1()
        {
            InitializeComponent();

            drawer = new Drawer(this.pictureBox);
            drawer.InitPictureBox();

            logic = new PointsLogic();
            logic.OnDraw += logic_Draw;

            pictureBox.Paint += pictureBox_Paint;
            pictureBox.MouseClick += pictureBox_MouseClick;
            tensionBar.ValueChanged += bar_ValueChanged;
            continuityBar.ValueChanged += bar_ValueChanged;
            biasBar.ValueChanged += bar_ValueChanged;

            pictureBox.MouseDown += pictureBox_MouseDown;
            pictureBox.MouseMove += pictureBox_MouseMove;
            pictureBox.MouseUp += pictureBox_MouseUp;
        }

        void logic_Draw()
        {
            Draw();
        }

        void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            logic.UnSelectPoint();
            pictureBox.Invalidate();
        }

        void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            logic.MoveSelectedPoint(drawer, e);
        }

        void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                logic.SelectOrAddNewPoint(drawer, e);
            }
        }

        void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            logic.PointsChanged(drawer, e);
            Draw();
        }

        void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) // delete
            {
                logic.DeletePoint(drawer, e);
            }
        }

        void bar_ValueChanged(object sender, EventArgs e)
        {
            drawer.InitPictureBox();
            Draw();
        }

        private void Draw()
        {
            drawer.Draw(logic.Points, tensionBar.Value, continuityBar.Value, biasBar.Value);
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            logic.ClearPoints(drawer);
        }

    }
}
