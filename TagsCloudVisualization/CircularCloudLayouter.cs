using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{

    class CircularCloudLayouter
    {

        private ISpiral Spiral { get; }
        public Point Center { get; }
        public List<Rectangle> Rectangles { get; }
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Spiral = new LogarithmicSpiral(Center);
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = GetRectungle(rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GetRectungle(Size rectangleSize)
        {
            Rectangle rectangle;
            if (Rectangles.Count == 0)
                rectangle = new Rectangle(new Point(Center.X - rectangleSize.Width / 2,
                    Center.Y - rectangleSize.Height / 2), rectangleSize);
            else
            {
                rectangle = new Rectangle(Spiral.GetNextPoint(), rectangleSize);
                while (IsRectangleIntersectsWithOther(rectangle))
                {
                    rectangle = new Rectangle(Spiral.GetNextPoint(), rectangleSize);
                }
            }
            return rectangle;
        }

        public bool IsRectangleIntersectsWithOther(Rectangle rectangle)
        {
            return Rectangles.Any(rectangle.IntersectsWith);
        }


    }

}