﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheDesktopBear
{

    public partial class Bear : Form
    {
        private Point mousePoint;
        enum BearMove { FRONT, RIGHT, BACK, LEFT, STAND };

        private int move_num = -1; //이미지갱신을 위한 tick
        private int dir = (int)BearMove.FRONT; //방향

        public Bear()
        {
            InitializeComponent();
            MoveTimer.Interval = 500;
            MoveTimer.Start();

            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);

        }

        #region 캐릭터로 폼움직이기

        private void Character_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }
        // 마우스 클릭시 먼저 선언된 mousePoint변수에 현재 마우스 위치값이 들어갑니다.

        private void Character_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }
        #endregion

        #region 파일 드래그앤 드롭
        private Form isFormAlreadyOpen(Type FormType)
        {
            foreach(Form OpenForm in Application.OpenForms)
            {
                if (OpenForm.GetType() == FormType)
                    return OpenForm;
            }
            return null;
        }
        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            Form fs = Application.OpenForms["FileSender"];
            if (isFormAlreadyOpen(typeof(FileSender)) == null){
                fs = new FileSender();
                fs.Parent = this;
                fs.Show();
            }
            else
            {
                fs.BringToFront();
                fs.Activate();
            }

            //만약 FileSender가 열려있다면 - 파일Sender한테 파일넘겨주기

            //아니라면 파일을 먹는 모션(?)

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                //consoleeee.Text = file.ToString();
            }
        }
        #endregion

        #region 움직임
        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            Image now = Character.Image;
            Image[,] images = new Image[4, 4];
            int speed = 8;
            #region Front Image
            images[0, 0] = Image.FromFile("C:\\Users\\souvenir\\Desktop\\sprite\\Front1.png");
            images[0, 1] = Image.FromFile("C:\\Users\\souvenir\\Desktop\\sprite\\Front2.png");
            images[0, 2] = Image.FromFile("C:\\Users\\souvenir\\Desktop\\sprite\\Front3.png");
            images[0, 3] = Image.FromFile("C:\\Users\\souvenir\\Desktop\\sprite\\Front4.png");
            #endregion
            #region Right Image
            images[1, 0] = Image.FromFile("C:\\Users\\souvenir\\Desktop\\sprite\\Right1.png");
            images[1, 1] = Image.FromFile("C:\\Users\\souvenir\\Desktop\\sprite\\Right2.png");
            images[1, 2] = Image.FromFile("C:\\Users\\souvenir\\Desktop\\sprite\\Right3.png");
            images[1, 3] = Image.FromFile("C:\\Users\\souvenir\\Desktop\\sprite\\Right4.png");
            #endregion
            #region Back Image
            images[2, 0] = Image.FromFile("C:\\Users\\souvenir\\Desktop\\sprite\\Back1.png");
            images[2, 1] = Image.FromFile("C:\\Users\\souvenir\\Desktop\\sprite\\Back2.png");
            images[2, 2] = Image.FromFile("C:\\Users\\souvenir\\Desktop\\sprite\\Back3.png");
            images[2, 3] = Image.FromFile("C:\\Users\\souvenir\\Desktop\\sprite\\Back4.png");
            #endregion
            #region Left Image
            images[3, 0] = Image.FromFile("C:\\Users\\souvenir\\Desktop\\sprite\\Left1.png");
            images[3, 1] = Image.FromFile("C:\\Users\\souvenir\\Desktop\\sprite\\Left2.png");
            images[3, 2] = Image.FromFile("C:\\Users\\souvenir\\Desktop\\sprite\\Left3.png");
            images[3, 3] = Image.FromFile("C:\\Users\\souvenir\\Desktop\\sprite\\Left4.png");
            #endregion
            move_num++; move_num %= 4;

            switch (dir)
            {
                case (int)BearMove.FRONT:
                    Location = new Point(this.Location.X, this.Location.Y + speed);
                    break;
                case (int)BearMove.RIGHT:
                    Location = new Point(this.Location.X + speed, this.Location.Y);
                    break;
                case (int)BearMove.BACK:
                    Location = new Point(this.Location.X, this.Location.Y - speed);
                    break;
                case (int)BearMove.LEFT:
                    Location = new Point(this.Location.X - speed, this.Location.Y - speed);
                    break;
                default:
                    break;

            }
            if (dir > 3) return;
            switch (move_num)
            {
                case 0:
                    Character.Image = images[dir, 0];
                    break;
                case 1:
                    Character.Image = images[dir, 1];
                    break;
                case 2:
                    Character.Image = images[dir, 2];
                    break;
                case 3:
                    Character.Image = images[dir, 3];
                    break;
            }
        }
        #endregion

        #region 행동
        private void CharacterKeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                ExitTimer.Start();
            }
            else if (e.KeyCode == Keys.A)// 울음소리
            {
                Thread t = new Thread(new ThreadStart(Howling));
                t.Start();
            }
            else if (e.KeyCode == Keys.S)// 창 최소화
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }
        void Howling()
        {
            SoundPlayer wp = new SoundPlayer("../../resource/sound/growl.wav");
            wp.PlaySync();
        }

        private void CharacterKeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Escape:
                    ExitTimeDisplay.Text = (int.Parse(ExitTimeDisplay.Text) + 1).ToString();
                    break;
                default:
                    ExitTimeDisplay.Text = "0";
                    break;
            }
        }
        private void CharacterKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ExitTimer.Stop();
                ExitTimeDisplay.Text = "0";

                ExitTimer.Dispose();
            }
        }
        private void ExitTimeDisplay_TextChanged(object sender, EventArgs e)
        {
            if (ExitTimeDisplay.Text.Equals("10"))
            {
                this.Close();
            }

        }
        #endregion

        #region 임시
        private void 멈추기SToolStripMenuItem_Click(object sender, EventArgs e)    { dir = 4;}

        private void NewBearBtn_Click(object sender, EventArgs e)
        {
            Bear f = new Bear();
            f.Show();
        }

        private void 파일전송하기SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //해당 User에게 수락/거절 메세지 보내기
            //수락시 파일전송창 띄워주기
            FileSender filesender = new FileSender();
            filesender.Show();
            
        }
        #endregion
    }
}
