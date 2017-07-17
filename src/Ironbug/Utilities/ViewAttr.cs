﻿using System;
using System.Drawing;
using GH = Grasshopper;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Attributes;
using System.Collections.Generic;
using Grasshopper.Kernel.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Linq;

//using Embryo.Generic;

namespace Ironbug
{
    //Reference Embryo https://github.com/johnharding/Embryo/blob/master/src/embryoA/Visulise/ImageFromPathAttrib.cs

    public class ImageFromPathAttrib : GH_ComponentAttributes
    {
        //String myPath;
        const int rawSize = 320;
        const int offsetTop = 60;
        //float XYRatio =1;
        private double scale;

        //public string imgPath = string.Empty;
        List<Point> coordinates = new List<Point>();
        //List<string> currentValues = new List<string>();
        Bitmap imgBitmap;
        Graphics MyGraphics;
        
        public ImageFromPathAttrib(View owner)
            : base(owner)
        {
            this.imgBitmap = owner.DisplayImage;
            this.scale = owner.Scale;
            
        }

        
        protected override void Layout()
        {
            RectangleF @in = new RectangleF(this.Pivot, this.Bounds.Size);
            if ((double)@in.Width < (double)rawSize)
                @in.Width = (float)(double)rawSize;
            if ((double)@in.Height < (double)(double)rawSize)
                @in.Height = (float)(double)rawSize;
            this.Bounds = (RectangleF)GH_Convert.ToRectangle(@in);
            

        }
        
        private RectangleF getBounds(PointF location, SizeF imgSizeXY, int topOffset, double scale)
        {

            RectangleF rec = new RectangleF();
            rec.Location = location;
            rec.Width = imgSizeXY.Width*(float)scale;
            rec.Height = imgSizeXY.Height * (float)scale + topOffset;
            rec.Inflate(2f, 2f);

            return (RectangleF)GH_Convert.ToRectangle(rec);
        }

        private RectangleF getImgBounds(RectangleF bounds, int topOffset)
        {
            RectangleF rec = bounds;
            rec.Y += topOffset;
            rec.Height -= topOffset;

            rec.Inflate(-2f,-2f);
            return rec;
        }

        protected override void PrepareForRender(GH_Canvas canvas)
        {
            base.PrepareForRender(canvas);
            
            var owner = (View)this.Owner;
            this.scale = owner.Scale;
            this.imgBitmap = owner.DisplayImage;
            this.coordinates = owner.ExtrCoordinates;

            if (this.imgBitmap == null)
            {
                this.Bounds = getBounds(this.Pivot, new SizeF(rawSize, rawSize - offsetTop), offsetTop, 1);
            }
            else
            {
                ////dynamic size
                //this.Bounds = getBounds(this.Pivot, this.imgBitmap.Size, offsetTop, scale);

                //Fixed size
                //double XYRatio = this.imgBitmap.Width / this.imgBitmap.Height;
                var size = new SizeF(rawSize, (float)((double)rawSize / (double)this.imgBitmap.Width * (double)this.imgBitmap.Height));
                this.Bounds = getBounds(this.Pivot, size, offsetTop, scale);
            }

            //locate the inputs outputs
            RectangleF inputRect = new RectangleF(Pivot, new SizeF(100f, 54f));
            inputRect.X += 68;

            RectangleF outRect = new RectangleF(Pivot, new SizeF(100f, 36f));
            outRect.X += Bounds.Width - 165;

            LayoutInputParams(Owner, inputRect);
            LayoutOutputParams(Owner, outRect);

            this.ExpireLayout();

        }
        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);

            MyGraphics = graphics;

            if (channel == GH_CanvasChannel.Objects)
            {
                // Get the size to begin with

                if (this.imgBitmap != null)
                {
                    displayImg(this.imgBitmap);
                }
                else
                {
                    displayDefaultComponent();
                }

            }

        }



        private void displayImg(Bitmap inBitmap)
        {
            
            RectangleF rec = getImgBounds(this.Bounds, offsetTop);
            
            MyGraphics.DrawImage(imgBitmap, rec);

            //draw pixel coordinates
            if (this.coordinates.Count>0)
            {
                displayCoordinates(this.coordinates, this.MyGraphics);
            }
            
        }

        private void displayCoordinates(List<Point> coordinates, Graphics graphics)
        {
            int dotSize = 4;
            RectangleF rec = getImgBounds(this.Bounds, offsetTop);

            float img2ViewportRatio = (float)rec.Width / (float)this.imgBitmap.Width;

            foreach (var item in coordinates)
            {
                
                var relativePt = new PointF(item.X * img2ViewportRatio  + rec.X - dotSize/2 , item.Y * img2ViewportRatio + rec.Y - dotSize / 2);
                
                //SolidBrush myBrush = new SolidBrush(Color.White);
                Pen pen = new Pen(new SolidBrush(Color.White));
                //graphics.FillEllipse(myBrush, relativePt.X, relativePt.Y, dotSize, dotSize);
                graphics.DrawEllipse(pen, relativePt.X, relativePt.Y, dotSize, dotSize);
                
            }

            
        }

        private void displayDefaultComponent()
        {
            //reset the comonent
            imgBitmap = null;
            this.scale = 1;
            this.Owner.Message = null;

            RectangleF rec = getImgBounds(Bounds, offsetTop);
            
            Pen pen = new Pen(Color.Gray, 3);
            SolidBrush myBrush = new SolidBrush(Color.Gray);
            
            Font ubuntuFont = new Font("ubuntu", 8);
            StringFormat myFormat = new StringFormat();
            
            MyGraphics.FillRectangle(myBrush, Rectangle.Round(rec));
            //graphics.DrawRectangle(pen, Rectangle.Round(imgViewBounds));
            MyGraphics.DrawString("Please use a valid image file path.\nHDR, TIF, PNG, GIF, or JPG image", ubuntuFont, Brushes.White, new Point((int)rec.X + 12, (int)rec.Y + ((int)rec.Width * 2 / 3) + 10), myFormat);
            MyGraphics.DrawImage(Properties.Resources.Ladybug_Viewer_370, new RectangleF(rec.X, rec.Y, rec.Width, rec.Width*2/3));
            
            myBrush.Dispose();
            myFormat.Dispose();

        }

        public delegate void Button_Handler(object sender, Point clickedPtOnOriginalBitmap);

        private Button_Handler MouseDownEvent;
        public event Button_Handler mouseDownEvent
        {
            add
            {
                Button_Handler buttonHandler = MouseDownEvent;
                Button_Handler comparand;
                do
                {
                    comparand = buttonHandler;
                    buttonHandler = Interlocked.CompareExchange(ref this.MouseDownEvent, (Button_Handler)Delegate.Combine(comparand, value), comparand);
                }
                while (buttonHandler != comparand);
            }
            remove
            {
                Button_Handler buttonHandler = MouseDownEvent;
                Button_Handler comparand;
                do
                {
                    comparand = buttonHandler;
                    buttonHandler = Interlocked.CompareExchange(ref this.MouseDownEvent, (Button_Handler)Delegate.Remove(comparand, value), comparand);
                }
                while (buttonHandler != comparand);
            }
        }
        

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            
            if (e.Button == MouseButtons.Left)
            {
                RectangleF rec = getImgBounds(this.Bounds, offsetTop);
                var owner = (View)this.Owner;
                if (rec.Contains(e.CanvasLocation) && imgBitmap !=null && !owner.DisableClickable)
                {
                    
                    float img2ViewportRatio = (float)rec.Width / (float)this.imgBitmap.Width;

                    PointF clickedPt = PointF.Subtract(e.CanvasLocation,new SizeF(rec.X, rec.Y));
                    
                    //convert current pt location on grasshopper view back to original image size system
                    Point PixelPtOnOriginalBitmap = Point.Round(new PointF(clickedPt.X / img2ViewportRatio, clickedPt.Y / img2ViewportRatio));
                    
                    this.MouseDownEvent(this, PixelPtOnOriginalBitmap);
                    return GH_ObjectResponse.Handled;
                }
                else
                {
                    //MessageBox.Show("ImgBounds:" + ImgBounds + "\nclicked at: " + e.CanvasLocation + "\n" + Pivot + "\n" + ImgBounds.Contains(e.CanvasLocation));
                }
            }
            return base.RespondToMouseDown(sender, e);
        }

        

    }
}

