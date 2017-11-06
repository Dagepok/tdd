using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter
    {
        private ISpiral Spiral { get; }
        private Point Center { get; }
        private List<Rectangle> Rectangles { get; }
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Spiral = new LogarithmicSpiral(Center);
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = GetRectangle(rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GetRectangle(Size rectangleSize) => Rectangles.Count == 0
            ? GetFirstRectangle(rectangleSize)
            : GetNextRectangle(rectangleSize);

        private Rectangle GetFirstRectangle(Size rectangleSize)
            => new Rectangle(Center, rectangleSize);

        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            Rectangle rectangle;
            do
                rectangle = new Rectangle(Spiral.GetNextPoint(), rectangleSize);
            while (IsRectangleIntersectsWithOther(rectangle));
            return GetMovedToCenter(rectangle); //Не очень удачное название //поменял
        }
        private Rectangle GetMovedToCenter(Rectangle rectangle)
        {
            while (true)
            {
                var lastRectangle = rectangle;
                rectangle = TryApproximate(rectangle);
                if (lastRectangle.Equals(rectangle)) break;
            }
            return rectangle;

        }

        private Rectangle TryApproximate(Rectangle rectangle)
        {
            var vectorToCenter = new Point(Center.X - rectangle.X, Center.Y - rectangle.Y);
            if (vectorToCenter.X != 0)
                rectangle = TryMoveRectangle(rectangle, new Point(Math.Sign(vectorToCenter.X), 0));
            if (vectorToCenter.Y != 0)
                rectangle = TryMoveRectangle(rectangle, new Point(0, Math.Sign(vectorToCenter.Y)));
            return rectangle;
        }

        private Rectangle TryMoveRectangle(Rectangle rectangle, Point offset)
        {
            rectangle.Offset(offset);
            if (IsRectangleIntersectsWithOther(rectangle))
                rectangle.Offset(new Point(-offset.X, -offset.Y));
            return rectangle;
        }

        private bool IsRectangleIntersectsWithOther(Rectangle rectangle)
                    => Rectangles.Any(rectangle.IntersectsWith);


    }
}

