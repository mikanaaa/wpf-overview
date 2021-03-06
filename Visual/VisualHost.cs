﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfOverview.Visual
{
    public class VisualHost : FrameworkElement
    {
        double height;
        double width;
        private readonly VisualCollection _children;
        public VisualHost(double height, double width)
        {
            this.height = height;
            this.width = width;
            _children = new VisualCollection(this)
            {
                DrawLines()
            };
        }
        private DrawingVisual DrawLines()
        {
            DrawingVisual dv = new DrawingVisual();
            DrawingContext dc = dv.RenderOpen();

            Random random = new Random();

            System.Windows.Media.Brush brush = new System.Windows.Media.SolidColorBrush(Color.FromRgb(255, 0, 0));
            brush.Freeze();
            System.Windows.Media.Pen pen = new System.Windows.Media.Pen(brush, 1);
            pen.Freeze();
            for (int i = 0; i < 10000; i++)
            {
                dc.DrawLine(pen, new Point(random.NextDouble() * width, random.NextDouble() * height), new Point(random.NextDouble() * width, random.NextDouble() * height));
            }
            dc.Close();
            return dv;
        }
        protected override int VisualChildrenCount => _children.Count;
        protected override System.Windows.Media.Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return _children[index];
        }
    }
}
