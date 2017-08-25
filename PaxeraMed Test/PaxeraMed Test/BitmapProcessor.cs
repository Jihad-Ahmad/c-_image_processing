using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
//using System.linq;
//using System.Threading.Tasks;

namespace PaxeraMed_Test
{
    class BitmapProcessor
    {
        public int posX;
        public int posY;

        //constructor
        public BitmapProcessor(){ }

        public Bitmap ProcessUsingLockBitsWithDelegates(Bitmap toBeProcessed/*, delegate works per pixel x1,y1,x2,y2*/)
        {
            return toBeProcessed;
        }

        public Bitmap ProcessUsingLockBitsZoomIn(Bitmap toBeProcessed)
        {
            try
            {
                float zoom_factor = 1.1f;
                this.posX = this.posX - (int)((toBeProcessed.Width * zoom_factor - toBeProcessed.Width) / 2);
                this.posY = this.posY - (int)((toBeProcessed.Height * zoom_factor - toBeProcessed.Height) / 2);

                BitmapData bitmapData = toBeProcessed.LockBits(new Rectangle(0, 0, toBeProcessed.Width, toBeProcessed.Height), ImageLockMode.ReadWrite, toBeProcessed.PixelFormat);
                int bytesPerPixel = Bitmap.GetPixelFormatSize(toBeProcessed.PixelFormat) / 8;
                int byteCount = bitmapData.Stride * toBeProcessed.Height;
                byte[] pixels = new byte[byteCount];
                IntPtr ptrFirstPixel = bitmapData.Scan0;

                Bitmap outputBitmap = new Bitmap((int)(toBeProcessed.Width*zoom_factor),(int)(toBeProcessed.Height*zoom_factor));
                BitmapData bitmapData2 = outputBitmap.LockBits(new Rectangle(0, 0, outputBitmap.Width, outputBitmap.Height), ImageLockMode.ReadWrite, outputBitmap.PixelFormat);
                int bytesPerPixel2 = Bitmap.GetPixelFormatSize(outputBitmap.PixelFormat) / 8;
                int byteCount2 = bitmapData2.Stride * outputBitmap.Height;
                byte[] pixels2 = new byte[byteCount2];
                IntPtr ptrFirstPixel2 = bitmapData2.Scan0;


                Marshal.Copy(ptrFirstPixel, pixels, 0, byteCount);
                
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
               
               
                for (int x = 0; x < widthInBytes; x += bytesPerPixel)
                {
                        for (int y = 0; y < heightInPixels; y++)
                        {
                            //count++;
                            int currentLine = y * bitmapData.Stride;
            
                            byte oldRed = pixels[currentLine + x ];// = (byte)oldBlue;
                            byte oldBlue = pixels[currentLine + x+1 ];// = (byte)oldBlue;
                            byte oldGreen = pixels[currentLine + x + 2];//; = (byte)oldBlue;
                            byte oldalpha = pixels[currentLine + x + 3]; //= (byte)oldBlue;

                             for (int k = (int)((x) * zoom_factor); k < ((int)((x + 1) * zoom_factor)); k++)
                             {
                                 for (int l = (int)((y) * zoom_factor); l < ((int)((y + 1) * zoom_factor)); l++)
                                 {
                                     //toBeProcessed.SetPixel(k, l, temp);
                                     int currentLine2 = l * bitmapData2.Stride;
                                     pixels2[currentLine2 + k] = 255;// oldRed;
                                     pixels2[currentLine2 + k + 1] = 255;// oldBlue;
                                     pixels2[currentLine2 + k + 2] = 255;// oldGreen;
                                     pixels2[currentLine2 + k + 3] = 255;// oldalpha;
                                     
                                 }
                             }
                              /*
                            pixels2[currentLine + x ] = firstByte;
                            pixels2[currentLine + x +1] = oldBlue;
                            pixels2[currentLine + x + 2] = oldBlue;
                            pixels2[currentLine + x + 3] = oldBlue;
                            */
                        }
                    }


                Marshal.Copy(pixels2, 0, ptrFirstPixel2, byteCount2);
                toBeProcessed.UnlockBits(bitmapData);
                outputBitmap.UnlockBits(bitmapData2);


                // debugging line
                //throw new Exception("bitmap.Width:" + toBeProcessed.Width + ", height:" + toBeProcessed.Height + ", MXN:" + toBeProcessed.Width * toBeProcessed.Height + "\n Pixels and Bytes story count:" + count);

                return outputBitmap;

  /*
  
                //detailed image zooming processing 
                Bitmap oldBitmap = (Bitmap)toBeProcessed.Clone();
                toBeProcessed = new Bitmap((int)(toBeProcessed.Width * zoom_factor), (int)(toBeProcessed.Height * zoom_factor));
                //this.graphicsBitmap = Graphics.FromImage((Image)this.bitmap);

                for (int i = 0; i < oldBitmap.Width; i++)
                {
                    for (int j = 0; j < oldBitmap.Height; j++)
                    {
                        Color temp = oldBitmap.GetPixel(i, j);
                        for (int k = (int)((i) * zoom_factor); k < ((int)((i + 1) * zoom_factor)); k++)
                        {
                            for (int l = (int)((j) * zoom_factor); l < ((int)((j + 1) * zoom_factor)); l++)
                            {
                                toBeProcessed.SetPixel(k, l, temp);
                            }
                        }
                    }
                }
  */          }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message ));
            }
           // this.undoRedo.AddToArray(this.bitmap);

           // this.graphicsPictureBox.Clear(this.backgroundColor);
           // this.graphicsPictureBox.DrawImage(this.bitmap, this.posX, this.posY);
            
          }

        public Bitmap ProcessUsingLockBitsInvert(Bitmap toBeProcessed)
        {
            try
            {
                
                BitmapData bitmapData = toBeProcessed.LockBits(new Rectangle(0, 0, toBeProcessed.Width, toBeProcessed.Height), ImageLockMode.ReadWrite, toBeProcessed.PixelFormat);
                int bytesPerPixel = Bitmap.GetPixelFormatSize(toBeProcessed.PixelFormat) / 8;
                int byteCount = bitmapData.Stride * toBeProcessed.Height;
                byte[] pixels = new byte[byteCount];
                IntPtr ptrFirstPixel = bitmapData.Scan0;

                Bitmap outputBitmap = new Bitmap(toBeProcessed.Width , toBeProcessed.Height );
                BitmapData bitmapData2 = outputBitmap.LockBits(new Rectangle(0, 0, outputBitmap.Width, outputBitmap.Height), ImageLockMode.ReadWrite, outputBitmap.PixelFormat);
                int bytesPerPixel2 = Bitmap.GetPixelFormatSize(outputBitmap.PixelFormat) / 8;
                int byteCount2 = bitmapData2.Stride * outputBitmap.Height;
                byte[] pixels2 = new byte[byteCount2];
                IntPtr ptrFirstPixel2 = bitmapData2.Scan0;


                Marshal.Copy(ptrFirstPixel, pixels, 0, byteCount);

                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;

                for (int x = 0; x < widthInBytes; x += bytesPerPixel)
                {
                   // Parallel.For (0,heightInPixels,y =>
                    for (int y = 0; y < heightInPixels; y++)
                    {
                        //count++;
                        int currentLine = y * bitmapData.Stride;

                        byte oldBlue = pixels[currentLine + x];// = (byte)oldBlue;
                        byte oldGreen = pixels[currentLine + x + 1];// = (byte)oldBlue;
                        byte oldRed = pixels[currentLine + x + 2];//; = (byte)oldBlue;
                        byte oldalpha = pixels[currentLine + x + 3]; //= (byte)oldBlue;

                        pixels2[currentLine + x] = (byte)(255 - (int)oldBlue);
                        pixels2[currentLine + x + 1] = (byte)(255 - (int)oldGreen);
                        pixels2[currentLine + x + 2] =  (byte)(255 - (int)oldRed);
                        pixels2[currentLine + x + 3] = 255;// (byte)(255 - (int)oldalpha);

                    }
                }


                Marshal.Copy(pixels2, 0, ptrFirstPixel2, byteCount2);
                toBeProcessed.UnlockBits(bitmapData);
                outputBitmap.UnlockBits(bitmapData2);
                return outputBitmap;
            }
            catch (Exception exp)
            {
                throw (new Exception(exp.Message));
            }

        }

        public Bitmap ProcessUsingLockBitsGrayScale(Bitmap toBeProcessed)
        {       BitmapData bitmapData = toBeProcessed.LockBits(new Rectangle(0, 0, toBeProcessed.Width, toBeProcessed.Height), ImageLockMode.ReadWrite, toBeProcessed.PixelFormat);
                int bytesPerPixel = Bitmap.GetPixelFormatSize(toBeProcessed.PixelFormat) / 8;
                int byteCount = bitmapData.Stride * toBeProcessed.Height;
                byte[] pixels = new byte[byteCount];
                IntPtr ptrFirstPixel = bitmapData.Scan0;

                Marshal.Copy(ptrFirstPixel, pixels, 0, byteCount);


                Bitmap outputBitmap = new Bitmap(toBeProcessed.Width , toBeProcessed.Height );
                BitmapData bitmapData2 = outputBitmap.LockBits(new Rectangle(0, 0, outputBitmap.Width, outputBitmap.Height), ImageLockMode.ReadWrite, outputBitmap.PixelFormat);
                int bytesPerPixel2 = Bitmap.GetPixelFormatSize(outputBitmap.PixelFormat) / 8;
                int byteCount2 = bitmapData2.Stride * outputBitmap.Height;
                byte[] pixels2 = new byte[byteCount2];
                IntPtr ptrFirstPixel2 = bitmapData2.Scan0;



                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;

                for (int x = 0; x < widthInBytes; x += bytesPerPixel)
                {
                    for (int y = 0; y < heightInPixels; y++)
                    {
                        //count++;
                        int currentLine = y * bitmapData.Stride;
                        
                        byte oldBlue = pixels[currentLine + x];// = (byte)oldBlue;
                        byte oldGreen = pixels[currentLine + x + 1];// = (byte)oldBlue;
                        byte oldRed = pixels[currentLine + x + 2];//; = (byte)oldBlue;
                        byte oldalpha = pixels[currentLine + x + 3]; //= (byte)oldBlue;

                        int c = (int)((0.07 * (int)oldBlue) + (0.72 * (int)oldGreen) + (0.21 * (int)oldRed));
                        pixels2[currentLine + x] = (byte)c;
                        pixels2[currentLine + x + 1] = (byte)c;
                        pixels2[currentLine + x + 2] = (byte)c;
                        pixels2[currentLine + x + 3] = (byte)255;// (byte)(255 - (int)oldalpha);

                    }
                }


                Marshal.Copy(pixels2, 0, ptrFirstPixel2, byteCount2);
                toBeProcessed.UnlockBits(bitmapData);
                outputBitmap.UnlockBits(bitmapData2);
                return outputBitmap;
}
    };//end of class
}//end of namespace