using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Spiral = new LogarithmicSpiral(Center);
            rectangles = new List<Rectangle>();
        }

        private ISpiral Spiral { get; }
        public Point Center { get; }
        public IReadOnlyList<Rectangle> Rectangles => rectangles;
        private readonly List<Rectangle> rectangles;

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = GetNextRectangle(rectangleSize);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            Rectangle rectangle;
            do
            {
                rectangle = new Rectangle(Point.Round(Spiral.GetNextPoint()), rectangleSize);
            } while (IsRectangleIntersectsWithOther(rectangle));
            return GetMovedToCenter(rectangle);
        }

        private Rectangle GetMovedToCenter(Rectangle rectangle)
        {
            while (true)
            {
                var lastRectangle = rectangle;
                rectangle = TryMoveToCenter(rectangle);
                if (lastRectangle.Equals(rectangle)) break;
            }
            return rectangle;
        }

        private Rectangle TryMoveToCenter(Rectangle rectangle)
        {
            var vectorToCenter = new Point(Center.X - rectangle.X, Center.Y - rectangle.Y);
            if (vectorToCenter.X != 0)
                rectangle = TryMove(rectangle, new Point(Math.Sign(vectorToCenter.X), 0));
            if (vectorToCenter.Y != 0)
                rectangle = TryMove(rectangle, new Point(0, Math.Sign(vectorToCenter.Y)));
            return rectangle;
        }

        private Rectangle TryMove(Rectangle rectangle, Point offset)
        {
            rectangle.Offset(offset);
            if (IsRectangleIntersectsWithOther(rectangle))
                rectangle.Offset(new Point(-offset.X, -offset.Y));
            return rectangle;
        }

        private bool IsRectangleIntersectsWithOther(Rectangle rectangle)
        {
            return rectangles.Any(rectangle.IntersectsWith);
        }
    }
}