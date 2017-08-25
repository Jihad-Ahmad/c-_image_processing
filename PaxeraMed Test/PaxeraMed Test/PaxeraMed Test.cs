using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace PaxeraMed_Test
{
    public partial class PaxeraMedTestForm : Form
    {
        //Graphics processing members 
        private Graphics graphicsPictureBox;
        private Graphics graphicsBitmap;
        private Color backgroundColor;
        private Color drawColor;
        private Bitmap bitmap;
        private SolidBrush solidColorBrush;
        private Pen pen;
        private bool free_hand;
        private bool lines;
        private bool commit_line;
        private int start_pointX;
        private int start_pointY;

        //file management member
        private String filename;

        //image processing settings
        BitmapProcessor bitmapProcessor;
        private bool shouldPaint;
        private bool shouldDrag;
        private float zoom_factor;
        private float zoomout_factor;
        private int deltaX;
        private int deltaY;
        private int posX;
        private int posY;

        //User Friendly GUI members
        private UndoRedoFeature undoRedo;

        //Constructor
        public PaxeraMedTestForm()
        {
            InitializeComponent();
            this.filename = "./Untitled.bmp";
            solidColorBrush =
             new SolidBrush(Color.White);
            pen = new Pen(solidColorBrush);
            pen.Width = 5;
            this.graphicsPictureBox = this.pictureBox1.CreateGraphics();
            this.shouldPaint = false;
            this.shouldDrag = false;
            this.backgroundColor = Color.FromArgb(100,100,100);
            this.drawColor = Color.Aqua;
            this.posX = 0;
            this.posY = 0;
            this.deltaX = 0;
            this.deltaY = 0;
            this.zoom_factor = 1.1f;
            this.zoomout_factor = 0.9f;
            this.bitmap = new Bitmap(100,100);
            this.graphicsBitmap = Graphics.FromImage(this.bitmap);
            this.graphicsBitmap.FillRectangle(this.solidColorBrush, 0f, 0f, this.bitmap.Width, this.bitmap.Height);
            this.free_hand = true;
            this.lines = false;
            this.commit_line = false; 
            this.start_pointX=0;
            this.start_pointY=0;

            this.bitmapProcessor = new BitmapProcessor();
            this.undoRedo = new UndoRedoFeature();
        }

        private void createNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Create_Form cf = new Create_Form();
            DialogResult dr = cf.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.filename = cf.filename;
                this.bitmap = new Bitmap(cf.getWidth(), cf.getHeight());
                graphicsBitmap = Graphics.FromImage(this.bitmap);
                graphicsBitmap.FillRectangle(this.solidColorBrush, 0f, 0f, this.bitmap.Width, this.bitmap.Height);
                File.Create(this.filename);

                this.posX = 0;
                this.posY = 0;
                this.graphicsPictureBox.Clear(this.backgroundColor);
                this.graphicsBitmap.DrawImage(this.bitmap, this.posX, this.posY);
                this.printInformation();
            }
            this.undoRedo = new UndoRedoFeature();
            this.undoRedo.AddToArray(this.bitmap);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.shouldPaint = true;
                this.shouldDrag = false;
                this.start_pointX = e.X - this.posX;
                this.start_pointY = e.Y - this.posY;
            }
            if (e.Button == MouseButtons.Middle)
            {
                this.shouldPaint = false;
                this.shouldDrag = false;
                this.deltaY = e.Y - this.deltaY;
            }
            if (e.Button == MouseButtons.Left)
            {
                this.shouldPaint = false;
                this.shouldDrag = true;
                this.deltaX = e.X - this.posX;
                this.deltaY = e.Y - this.posY;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.shouldPaint  && this.free_hand)
            {
                if (e.X >= this.posX && e.X < (this.posX + this.bitmap.Width)
                    && e.Y >= this.posY && e.Y < (this.posY + this.bitmap.Height))
                {                    
                    this.graphicsBitmap.FillEllipse(new SolidBrush(this.drawColor), e.X - this.posX, e.Y - this.posY, 10, 10);
                    this.graphicsPictureBox.Clear(this.backgroundColor);
                    this.graphicsPictureBox.DrawImage(this.bitmap, this.posX, this.posY);
                    this.undoRedo.AddToArray(this.bitmap);
                }
            }
            if (this.shouldPaint && this.lines)
            {
                this.commit_line = true;
            }
            if (this.shouldDrag)
            {
                this.graphicsPictureBox.Clear(this.backgroundColor);
                this.posX = e.X - this.deltaX;
                this.posY = e.Y - this.deltaY;
                this.graphicsPictureBox.DrawImage((Image)bitmap, this.posX, this.posY); //e.X,e.Y);
            }
            
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = DefaultCursor;
            this.shouldPaint = false;
            this.shouldDrag = false;
            this.deltaX = 0;
            this.deltaY = 0;
            if (this.commit_line)
            {
                if (e.X >= this.posX && e.X < (this.posX + this.bitmap.Width)
                    && e.Y >= this.posY && e.Y < (this.posY + this.bitmap.Height))
                {
                    this.pen = new Pen(new SolidBrush(this.drawColor));
                    this.pen.Width = 7;
                    this.graphicsBitmap.DrawLine(this.pen, this.start_pointX, this.start_pointY, e.X - this.posX, e.Y - this.posY);
                    this.graphicsPictureBox.Clear(this.backgroundColor);
                    this.graphicsPictureBox.DrawImage(this.bitmap, this.posX, this.posY);
                    this.undoRedo.AddToArray(this.bitmap);
                }
                commit_line = false;
            }             
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphicsObject = e.Graphics;
            graphicsObject.Clear(this.backgroundColor);
            graphicsObject.DrawImage(this.bitmap, this.posX, this.posY);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
           Form f=  this.FindForm();
           f.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = this.openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.filename = this.openFileDialog1.FileName;
                Image image = Image.FromFile(this.filename);
                bitmap = new Bitmap(image);
                this.graphicsBitmap = Graphics.FromImage(this.bitmap);
                this.graphicsPictureBox.Clear(this.backgroundColor);
                this.posX = 0;
                this.posY = 0;
                this.graphicsPictureBox.DrawImage(image, this.posX, this.posY);
              
            }
            if (result == DialogResult.Cancel)
            {
                return;
            }
            this.printInformation();
            this.undoRedo = new UndoRedoFeature();
            this.undoRedo.AddToArray(this.bitmap);

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bitmap != null)
                {
                    if (File.Exists(this.filename))
                    {
                        //  FileStream fs = File.OpenWrite(this.filename);
                        this.bitmap.Save(this.filename);
                        // fs.Close();

                    }
                    else
                    {
                        //fire save as
                        saveAsToolStripMenuItem_Click(sender, e);
                    }
                }
                this.pictureBox1.Refresh();
            }
            catch (Exception exp)
            {
                lbl_info.Text = exp.Message;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.ShowDialog();
            this.printInformation();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            this.filename = this.saveFileDialog1.FileName;
            bitmap.Save(this.filename);
        }

        private void printInformation()
        {
            this.Text = "PaxeraMed Test      "+this.filename;
            this.lbl_info.Text = "filename:" + this.filename + "\n";
            this.lbl_info.Text += "width of created bitmap:" + this.bitmap.Width + "\n, height of bmp:" + this.bitmap.Height + "\n";
    
        }

        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Bitmap b = new Bitmap(this.bitmap.Width, this.bitmap.Height);
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color temp = bitmap.GetPixel(i, j);//.ToString();//.Width;;
                    int R = 255 - temp.R;
                    int G = 255 - temp.G;
                    int B = 255 - temp.B;
                    b.SetPixel(i, j, Color.FromArgb(R, G, B));
                }

            }

            bitmap = new Bitmap((Image)b);
            this.graphicsBitmap = Graphics.FromImage(this.bitmap);
            this.undoRedo.AddToArray(this.bitmap);
            sw.Stop();
            lbl_info.Text = "Nested Loops Invert took time:"+sw.ElapsedMilliseconds+"ms";
            this.graphicsPictureBox.Clear(this.backgroundColor);
            this.graphicsPictureBox.DrawImage(this.bitmap, this.posX, this.posY);
            this.pictureBox1.Refresh();
        }

        private void grayScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color temp = bitmap.GetPixel(i, j);//.ToString();//.Width;;
                    double R = 0.21 * temp.R;
                    double G = 0.72 * temp.G;
                    double B = 0.07 * temp.B;
                    int avg = (int)R + (int)G + (int)B;
                    bitmap.SetPixel(i, j, Color.FromArgb(avg, avg, avg));
                }

            }
            this.bitmap = new Bitmap((Image)this.bitmap, this.bitmap.Width, this.bitmap.Height);
            this.graphicsBitmap = Graphics.FromImage((Image)this.bitmap);
            this.undoRedo.AddToArray(this.bitmap);
            sw.Stop();
            lbl_info.Text = "Nested Gray Scale took time: "+sw.ElapsedMilliseconds+"ms";
            this.graphicsPictureBox.Clear(this.backgroundColor);
            this.graphicsPictureBox.DrawImage(this.bitmap, this.posX, this.posY);
        }

        private void stretchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            this.posX = 0;
            this.posY = 0;

            //could be done correctly by 1 line
            /*
           this.bitmap = new Bitmap((Image)bitmap, new Size(this.pictureBox1.Width, this.pictureBox1.Height));
            this.graphicsBitmap = Graphics.FromImage((Image)this.bitmap);
            */

            //detailed image stretching processing 
            float stretch_factorX = this.pictureBox1.Width*100/this.bitmap.Width;
            float stretch_factorY = this.pictureBox1.Height*100/this.bitmap.Height;

            Bitmap oldBitmap = (Bitmap)this.bitmap.Clone();
            this.bitmap = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            this.graphicsBitmap = Graphics.FromImage((Image)this.bitmap);

            for (int i = 0; i < oldBitmap.Width; i++)
            {
                for (int j = 0; j < oldBitmap.Height; j++)
                {
                    Color temp = oldBitmap.GetPixel(i, j);
                    for (int k = (int)((i) * stretch_factorX/100); k < ((int)((i + 1) * stretch_factorX/100)); k++)
                    {
                        for (int l = (int)((j) * stretch_factorY/100); l < ((int)((j + 1) * stretch_factorY/100)); l++)
                        {
                            this.bitmap.SetPixel(k, l, temp);
                        }
                    }
                }
            }
            this.undoRedo.AddToArray(this.bitmap);
            sw.Stop();
            lbl_info.Text = "Nested Loops Stretch took time: "+sw.ElapsedMilliseconds+"ms";
            this.graphicsPictureBox.Clear(this.backgroundColor);
            this.graphicsPictureBox.DrawImage(this.bitmap, 0, 0);
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            this.posX = this.posX - (int)((this.bitmap.Width * zoom_factor - this.bitmap.Width) / 2);
            this.posY = this.posY - (int)((this.bitmap.Height * zoom_factor - this.bitmap.Height) / 2);

            //could be done by correctly by 1 line
            /*
            this.bitmap = new Bitmap(this.bitmap,new Size((int)(this.bitmap.Width * zoom_factor),(int)(this.bitmap.Height * zoom_factor)));
             * this.graphicsBitmap = Graphics.FromImage((Image)this.bitmap);
            */

            //detailed image zooming processing 
            Bitmap oldBitmap = (Bitmap)this.bitmap.Clone();
            this.bitmap = new Bitmap((int)(this.bitmap.Width * zoom_factor), (int)(this.bitmap.Height * zoom_factor));
            this.graphicsBitmap = Graphics.FromImage((Image)this.bitmap);

            for (int i = 0; i < oldBitmap.Width; i++)
            {
                for (int j = 0; j < oldBitmap.Height; j++)
                {
                    Color temp = oldBitmap.GetPixel(i, j);
                    for (int k = (int)((i) * zoom_factor); k < ((int)((i + 1) * zoom_factor)); k++)
                    {
                        for (int l = (int)((j) * zoom_factor); l < ((int)((j + 1) * zoom_factor)); l++)
                        {
                            this.bitmap.SetPixel(k, l, temp);
                        }
                    }
                }
            }
            this.undoRedo.AddToArray(this.bitmap);

            sw.Stop();
            lbl_info.Text = "Nested Loops Zoom In took time:" + sw.ElapsedMilliseconds + "ms";
            this.graphicsPictureBox.Clear(this.backgroundColor);
            this.graphicsPictureBox.DrawImage(this.bitmap, this.posX, this.posY);
            
        }

    
    
        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            this.posX = this.posX - (int)((this.bitmap.Width * zoomout_factor - this.bitmap.Width) / 2);
            this.posY = this.posY - (int)((this.bitmap.Height * zoomout_factor - this.bitmap.Height) / 2);

            //could be done by correctly by 1 line
            /*
            this.bitmap = new Bitmap(this.bitmap, new Size((int)(this.bitmap.Width * zoomout_factor), (int)(this.bitmap.Height * zoomout_factor))); 
             * this.graphicsBitmap = Graphics.FromImage((Image)this.bitmap);
             */

            //detailed image zooming processing 
            Bitmap oldBitmap = (Bitmap)this.bitmap.Clone();
            this.bitmap = new Bitmap((int)(this.bitmap.Width * zoomout_factor), (int)(this.bitmap.Height * zoomout_factor));
            this.graphicsBitmap = Graphics.FromImage((Image)this.bitmap);
            for (int i = 0; i < oldBitmap.Width; i++)
            {
                for (int j = 0; j < oldBitmap.Height; j++)
                {
                    Color temp = oldBitmap.GetPixel(i, j);
                    for (int k = (int)((i) * zoomout_factor); k < ((int)((i + 1) * zoomout_factor)); k++)
                    {
                        for (int l = (int)((j) * zoomout_factor); l < ((int)((j + 1) * zoomout_factor)); l++)
                        {
                            this.bitmap.SetPixel(k, l, temp);
                        }
                    }
                }
            }


            this.undoRedo.AddToArray(this.bitmap);
            sw.Stop();
            lbl_info.Text = "Nested Zoom Out took time: "+sw.ElapsedMilliseconds+"ms";
            this.graphicsPictureBox.Clear(this.backgroundColor);
            this.graphicsPictureBox.DrawImage(this.bitmap, this.posX, this.posY);
            //    Bitmap zoomedIn = new Bitmap(this.bitmap.Width * this.zoom_factor, this.bitmap.Height * this.zoom_factor);
            
        }

        private void fitImageIntoBoundary(Bitmap b, int posx, int posy, int bw, int bh)
        {
            try
            {
                int aspect_ratio = (b.Width * 100 / b.Height);
                int box_aspect = (bw * 100 / bh);

                int new_width; int new_height;
                if (box_aspect > aspect_ratio)
                {
                    //aspect_ratio = (int)(this.bitmap.Height / this.bitmap.Width);
                    new_height = bh;
                    new_width = (int)((new_height * aspect_ratio) / 100);
                    posx = ((int)((bw - new_width) / 2))+posx;
                    this.lbl_info.Text += "\n box aspect > aspect ratio";
                }
                else
                {
                    new_width = bw;
                    new_height = (int)100 * bw / aspect_ratio;
                    posy = ((int)((bh - new_height) / 2))+posy;
                    this.lbl_info.Text += "\n  aspect ratio > box aspect";
                }

                float stretch_factorX = new_width * 100 / b.Width;
                float stretch_factorY = new_height * 100 / b.Height;


                Bitmap oldBitmap = (Bitmap)b.Clone();
                b = new Bitmap(bw,bh);

                for (int i = 0; i < oldBitmap.Width; i++)
                {
                    for (int j = 0; j < oldBitmap.Height; j++)
                    {
                        Color temp = oldBitmap.GetPixel(i, j);
                        for (int k = (int)((i) * stretch_factorX / 100); k < ((int)((i + 1) * stretch_factorX / 100)); k++)
                        {
                            for (int l = (int)((j) * stretch_factorY / 100); l < ((int)((j + 1) * stretch_factorY / 100)); l++)
                            {
                                b.SetPixel(k, l, temp);
                            }
                        }
                    }
                }

            }
            catch (Exception exp)
            {
                this.lbl_info.Text += "\n" + exp.Message;
            }
        }

        private void fitCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                int aspect_ratio = (this.bitmap.Width * 100 / this.bitmap.Height);
                int box_aspect = (this.pictureBox1.Width * 100 / this.pictureBox1.Height);

                int new_width; int new_height;
                if (box_aspect > aspect_ratio)
                {
                    new_height = this.pictureBox1.Height;
                    new_width = (int)((new_height*aspect_ratio)/100);
                    this.posY = 0;
                    this.posX = (int)((this.pictureBox1.Width-new_width) / 2);
                    this.lbl_info.Text += "\n box aspect > aspect ratio";
                }
                else
                {
                    new_width = this.pictureBox1.Width;
                    new_height = (int)100 * this.pictureBox1.Width / aspect_ratio;
                    this.posX = 0;
                    this.posY = (int)((this.pictureBox1.Height-new_height) / 2);
                    this.lbl_info.Text += "\n  aspect ratio > box aspect";
                }

                float stretch_factorX = new_width * 100 / this.bitmap.Width;
                float stretch_factorY = new_height * 100 / this.bitmap.Height;


                Bitmap oldBitmap = new Bitmap(this.bitmap);
                this.bitmap = new Bitmap(new_width,new_height);
                this.graphicsBitmap = Graphics.FromImage((Image)this.bitmap);


                for (int i = 0; i < oldBitmap.Width; i++)
                {
                    for (int j = 0; j < oldBitmap.Height; j++)
                    {
                        Color temp = oldBitmap.GetPixel(i, j);
                        for (int k = (int)((i) * stretch_factorX / 100); k < ((int)((i + 1) * stretch_factorX / 100)); k++)
                        {
                            for (int l = (int)((j) * stretch_factorY / 100); l < ((int)((j + 1) * stretch_factorY / 100)); l++)
                            {
                                this.bitmap.SetPixel(k, l, temp);
                            }
                        }
                    }
                }
                this.undoRedo.AddToArray(this.bitmap);
                sw.Stop();
                lbl_info.Text = "Nested Fit and Center"+sw.ElapsedMilliseconds+"ms";
                this.graphicsPictureBox.Clear(this.backgroundColor);
                this.graphicsPictureBox.DrawImage(this.bitmap, this.posX,this.posY);

            }
            catch (Exception exp)
            {
                this.lbl_info.Text += "\n"+exp.Message;
            }
            this.graphicsBitmap = Graphics.FromImage((Image)this.bitmap);
            this.graphicsPictureBox.Clear(this.backgroundColor);
            this.graphicsPictureBox.DrawImage(this.bitmap,this.posX,this.posY);
        }

        private void freeHandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.free_hand = true;
            this.lines = false;
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.drawColor = this.colorDialog1.Color;
            }
        }

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.free_hand = false;
            this.lines = true;
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.drawColor = this.colorDialog1.Color;
            }
        }

        private void thresholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int t = 0;
            Threshold_Choose tc = new Threshold_Choose();
            DialogResult dr = tc.ShowDialog();
            if (dr == DialogResult.OK)
            {
                t = tc.chosen_threshold;

                this.grayScaleToolStripMenuItem_Click(sender, e);

                int w = this.bitmap.Width;
                int h = this.bitmap.Height;
                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < h; j++)
                    {

                        if (this.bitmap.GetPixel(i, j).B <= t)
                        {
                            this.bitmap.SetPixel(i, j, Color.Black);
                        }
                        else
                        {
                            this.bitmap.SetPixel(i, j, Color.White);
                        }
                    }
                }
                this.undoRedo.AddToArray(this.bitmap);

                this.graphicsPictureBox.Clear(this.backgroundColor);
                this.graphicsPictureBox.DrawImage(this.bitmap, this.posX, this.posY);
            }
        }

        private void edgeDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.grayScaleToolStripMenuItem_Click(sender, e);
            Bitmap oldBitmap = new Bitmap((Image)this.bitmap);//.Clone();

            try
            {
                int[][] kernel = new int[3][];//{{,,},{,,},{,,}};

                kernel[0] = new int[] { -1, -1, -1 };
                kernel[1] = new int[] { -1, 9, -1 };
                kernel[2] = new int[] { -1, -1, -1 };
                //for every pixel
                for (int i = 1; i < this.bitmap.Width - 1; i++)
                {
                    for (int j = 1; j < this.bitmap.Height - 1; j++)
                    {
                        int sum = 0;
                        for (int kx = -1; kx <= 1; kx++)
                        {
                            for (int ky = -1; ky <= 1; ky++)
                            {
                                //Calculate the adjecent pixel for this kernel point
                                int px = i + kx;
                                int py = j + ky;

                                //image in gray scale, so R = G = B = gray degree
                                Color temp = oldBitmap.GetPixel(px, py);
                                int val = temp.B;
                                sum += kernel[kx+1][ky+1]* val;
                            }
                            
                        }
                        if (sum < 0) { sum = 0; }
                        if (sum > 255) { sum = 255; };
                        this.bitmap.SetPixel(i, j, Color.FromArgb(sum, sum, sum));
                    }
                }
            }
            catch (Exception exp)
            {
                lbl_info.Text += exp.Message;
            }
            this.undoRedo.AddToArray(this.bitmap);

            this.graphicsPictureBox.Clear(this.backgroundColor);
            this.graphicsPictureBox.DrawImage(this.bitmap, this.posX, this.posY);
        }


        private void segmentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.grayScaleToolStripMenuItem_Click(sender, e);

            //variable declarations
            int[] histogram = new int[256];
            int threshold = 0;
            double var_max = 0;
            int sum = 0;
            int sumB = 0;
            int q1 = 0;
            int q2 = 0;
            double Muo1 = 0;
            double Muo2 = 0;
            int N = this.bitmap.Width * this.bitmap.Height;

            try
            {
                //initialization
                for (int i = 0; i < 256; i++)
                { histogram[i] = 0; }

                //calculating insity frequencies
                for (int x = 0; x < this.bitmap.Width; x++)
                {
                    for (int y = 0; y < this.bitmap.Height; y++)
                    {
                        histogram[this.bitmap.GetPixel(x, y).B]++;
                    }
                }

                //choosing appropriate threshold
                for (int i = 0; i < 256; i++)
                { sum += i * histogram[i]; }

                for (int t = 0; t < 256; t++)
                {
                    q1 += histogram[t];
                    if (q1 == 0) { continue; }
                    q2 = N - q1;

                    sumB += t * histogram[t];
                    if (q1 == 0) { q1 = 1; }
                    if (q2 == 0) { q2 = 1; }
                    Muo1 = sumB / q1;
                    Muo2 = (sum - sumB) / q2;

                    double segma_sqr = q1 * q2 * (Muo1 - Muo2) * (Muo1 - Muo2);

                    if (segma_sqr > var_max)
                    {
                        threshold = t;
                        var_max = segma_sqr;
                    }
                }

                //performing segmentation to two classes
                for (int x = 0; x < this.bitmap.Width; x++)
                {
                    for (int y = 0; y < this.bitmap.Height; y++)
                    {
                        if (this.bitmap.GetPixel(x, y).B > threshold)
                        {
                            this.bitmap.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        }
                        else
                        {
                            this.bitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                this.lbl_info.Text = exp.Message;
            }

            this.undoRedo.AddToArray(this.bitmap);

            this.graphicsPictureBox.Clear(this.backgroundColor);
            this.graphicsPictureBox.DrawImage(this.bitmap, this.posX, this.posY);
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int sumB = 0;
            int q1 = 0;
            int q2 = 0;
            double Muo1 = 0;
            double Muo2 = 0;
            double segma_sqr = 0;
            int N = this.bitmap.Width * this.bitmap.Height;
            
            this.grayScaleToolStripMenuItem_Click(sender,e);
            try
            {
                int[] histogram = new int[256];

                //initialization
                for (int i = 0; i < 256; i++)
                { histogram[i] = 0; }

                Bitmap medicalDataExtraction = new Bitmap(this.bitmap.Width+512,this.bitmap.Height);
                Graphics g = Graphics.FromImage(medicalDataExtraction);
                g.Clear(Color.White);

                //copy bitmap to the left portion of new image
                for (int x = 0; x < this.bitmap.Width; x++)
                {
                    for (int y = 0; y < this.bitmap.Height; y++)
                    {
                        int c = this.bitmap.GetPixel(x, y).B;
                       histogram[c]++;
                        Color temp = Color.FromArgb(c,c,c);
                       medicalDataExtraction.SetPixel(x,y,temp);
                    }
                }

                int font_dist = (int)(medicalDataExtraction.Height / 3);
                int graph_height = medicalDataExtraction.Height - font_dist;
                int sum = 0;int sum2=0;
                
            for (int i = 0; i < 256; i++)
            { sum2 += i * histogram[i]; }

            for (int t = 0; t < 256; t++)
            {
                q1 += histogram[t];
                if (q1 == 0) { continue; }
                q2 = N - q1;

                sumB += t * histogram[t];
                if (q1 == 0) { q1 = 1; }
                if (q2 == 0) { q2 = 1; }
                Muo1 = sumB / q1;
                Muo2 = (sum2 - sumB) / q2;

                segma_sqr = q1 * q2 * (Muo1 - Muo2) * (Muo1 - Muo2);
            }

                for (int i = 0; i < 256; i++)
                {
                    // make histogram ready to graph
                    
                    histogram[i] *= 15;
                    sum += histogram[i];
                    histogram[i] /= graph_height;
                   
                    Pen pen2 = new Pen(new SolidBrush(Color.Black));
                    pen2.Width = 2;
                    g.DrawLine(pen2,this.bitmap.Width+(i*2),medicalDataExtraction.Height- font_dist,this.bitmap.Width+(i*2),medicalDataExtraction.Height-(histogram[i]+font_dist));
                }
                double mean = sum / N;
                string data = "Count:"+(N)+"                       Min:0\n";
                data += "Mean:"+mean+"                             Max:255\n";
                data += "Mio1:" + Muo1 + "\b                       Mio2:"+Muo2+"\n";
 //               data += "std. dev.:" + segma_sqr;// +"\b                       Mio2:" + Mio2 + "\n";

                g.DrawString(data, new Font("Times New Roman", 9), new SolidBrush(Color.Black), this.bitmap.Width, this.bitmap.Height - font_dist);


                this.bitmap = new Bitmap((Image)medicalDataExtraction);
                this.graphicsBitmap = Graphics.FromImage(this.bitmap);
                this.undoRedo.AddToArray(this.bitmap);

                //this.fitCenterToolStripMenuItem_Click(sender, e);
                this.pictureBox1.Refresh();

  }
            catch (Exception exp)
            {
                lbl_info.Text = exp.Message;
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap b = this.undoRedo.Undo();
            if (b == null) return;
            this.bitmap = new Bitmap(b);
            this.graphicsBitmap = Graphics.FromImage((Image)bitmap);
            this.pictureBox1.Refresh();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap b = this.undoRedo.Redo();
            if (b == null) return;
            this.bitmap = new Bitmap(b);
            this.graphicsBitmap = Graphics.FromImage((Image)bitmap);
            this.pictureBox1.Refresh();
        
        }

        private void InvertToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                this.bitmapProcessor.posX = this.posX;
                this.bitmapProcessor.posY = this.posY;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                this.bitmap = this.bitmapProcessor.ProcessUsingLockBitsInvert(this.bitmap);
                sw.Stop();
                this.undoRedo.AddToArray(this.bitmap);
                this.graphicsBitmap = Graphics.FromImage(this.bitmap);
                lbl_info.Text = "Efficient Invert took time:" + sw.ElapsedMilliseconds+"ms";
                this.pictureBox1.Refresh();
            }
            catch (Exception exp)
            {
                lbl_info.Text = exp.Message;
            }

        }

        private void grayScaleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            this.bitmapProcessor.posX = this.posX;
            this.bitmapProcessor.posY = this.posY;

            this.bitmap = this.bitmapProcessor.ProcessUsingLockBitsGrayScale(this.bitmap);
            this.undoRedo.AddToArray(this.bitmap);
            sw.Stop();
            lbl_info.Text = "Efficient Gray Scale took time: "+sw.ElapsedMilliseconds+"ms";
            this.graphicsBitmap = Graphics.FromImage(this.bitmap);
            this.pictureBox1.Refresh();
            
        }

        private void zoomInToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            this.posX = this.posX - (int)((this.bitmap.Width * zoom_factor - this.bitmap.Width) / 2);
            this.posY = this.posY - (int)((this.bitmap.Height * zoom_factor - this.bitmap.Height) / 2);

            this.bitmap = new Bitmap(this.bitmap,new Size((int)(this.bitmap.Width * zoom_factor),(int)(this.bitmap.Height * zoom_factor)));
            this.graphicsBitmap = Graphics.FromImage((Image)this.bitmap);

            this.undoRedo.AddToArray(this.bitmap);
            sw.Stop();
            lbl_info.Text = "Efficient Zoom In took time:" + sw.ElapsedMilliseconds + "ms";
            this.pictureBox1.Refresh();

        }

        private void zoomOutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            this.posX = this.posX - (int)((this.bitmap.Width * zoomout_factor - this.bitmap.Width) / 2);
            this.posY = this.posY - (int)((this.bitmap.Height * zoomout_factor - this.bitmap.Height) / 2);

            
            this.bitmap = new Bitmap(this.bitmap, new Size((int)(this.bitmap.Width * zoomout_factor), (int)(this.bitmap.Height * zoomout_factor))); 
            this.graphicsBitmap = Graphics.FromImage((Image)this.bitmap);

            this.undoRedo.AddToArray(this.bitmap);
            sw.Stop();
            lbl_info.Text = "Efficient Zoom out took time: "+sw.ElapsedMilliseconds+"ms";
            this.pictureBox1.Refresh();

        }

        private void stretchToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            this.posX = 0;
            this.posY = 0;

            //could be done correctly by 1 line
            
           this.bitmap = new Bitmap((Image)bitmap, new Size(this.pictureBox1.Width, this.pictureBox1.Height));
            this.graphicsBitmap = Graphics.FromImage((Image)this.bitmap);

            this.undoRedo.AddToArray(this.bitmap);
            sw.Stop();
            lbl_info.Text = "Efficient Stretch took time: " + sw.ElapsedMilliseconds + "ms";
            this.pictureBox1.Refresh();

        }

        private void fitCenterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int aspect_ratio = (this.bitmap.Width * 100 / this.bitmap.Height);
            int box_aspect = (this.pictureBox1.Width * 100 / this.pictureBox1.Height);

            int new_width; int new_height;
            if (box_aspect > aspect_ratio)
            {
                new_height = this.pictureBox1.Height;
                new_width = (int)((new_height * aspect_ratio) / 100);
                this.posY = 0;
                this.posX = (int)((this.pictureBox1.Width - new_width) / 2);
                this.lbl_info.Text += "\n box aspect > aspect ratio";
            }
            else
            {
                new_width = this.pictureBox1.Width;
                new_height = (int)100 * this.pictureBox1.Width / aspect_ratio;
                this.posX = 0;
                this.posY = (int)((this.pictureBox1.Height - new_height) / 2);
                this.lbl_info.Text += "\n  aspect ratio > box aspect";
            }

            this.bitmap = new Bitmap((Image)bitmap, new Size(new_width,new_height));
            this.graphicsBitmap = Graphics.FromImage((Image)this.bitmap);
            sw.Stop();
            lbl_info.Text = "Efficient Fit and Center took time: "+sw.ElapsedMilliseconds+"ms";
            this.undoRedo.AddToArray(this.bitmap);
            this.pictureBox1.Refresh();

        }
    }
}