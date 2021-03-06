﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using TCBspline.Controller;

namespace TCBspline
{
    public partial class TcbSplineForm : Form
    {
        private MainController controller;

        public TcbSplineForm()
        {
            InitializeComponent();


            controller = new MainController(this.pictureBox);
            controller.OnDraw += logic_Draw;
            controller.OnUpdateImage += controller_UpdateImage;

            pictureBox.Paint += pictureBox_Paint;
            pictureBox.MouseClick += pictureBox_MouseClick;
            tensionBar.ValueChanged += bar_ValueChanged;
            continuityBar.ValueChanged += bar_ValueChanged;
            biasBar.ValueChanged += bar_ValueChanged;

            pictureBox.MouseDown += pictureBox_MouseDown;
            pictureBox.MouseMove += pictureBox_MouseMove;
            pictureBox.MouseUp += pictureBox_MouseUp;

            pictureBox.SizeChanged += pictureBox_SizeChanged;
        }

        void controller_UpdateImage(Bitmap obj)
        {
            pictureBox.Image = obj;
        }

        void pictureBox_SizeChanged(object sender, EventArgs e)
        {
            controller.InitPictureBox(pictureBox.Width, pictureBox.Height);
        }

        void logic_Draw()
        {
            Draw();
        }

        void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            controller.UnSelectPoint();
            pictureBox.Invalidate();
        }

        void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            controller.MoveSelectedPoint(e);
        }

        void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                controller.SelectOrAddNewPoint(e);
            }
        }

        void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            controller.PointsChanged(e);
        }

        void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) // delete
            {
                controller.DeletePoint(e);
            }
        }

        void bar_ValueChanged(object sender, EventArgs e)
        {
            controller.InitPictureBox(pictureBox.Width, pictureBox.Height);
            Draw();
        }

        private void Draw()
        {
            controller.Draw(tensionBar.Value, continuityBar.Value, biasBar.Value);
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            controller.ClearPoints();
        }

        private void undoBtn_Click(object sender, EventArgs e)
        {
            controller.Undo();
        }

        private void redoBtn_Click(object sender, EventArgs e)
        {
            controller.Redo();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.FileName = "points.xml";
            dlg.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                controller.Serialize(dlg.FileName);
            }
        }

        private void loadBtn_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.FileName = "points.xml";
            dlg.Filter = "Xml files (*.txt)|*.txt|All files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                controller.Deserialize(dlg.FileName);
            }

        }

    }
}
