using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter
    {//Опять эти переносы =\

        private ISpiral Spiral { get; }
        public Point Center { get; } //Здесь публичный центр видимо для того, чтоб протестировать его)) Странно
        public List<Rectangle> Rectangles { get; } //Если сделать это поле приватным, то не надо будет его тестировать)))
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

        private Rectangle GetFirstRectangle(Size rectangleSize) //Какие бонусы от размещение первого прямоугольника ровно по центру? А вот код усложнился((
            => new Rectangle(new Point(Center.X - rectangleSize.Width / 2,
                Center.Y - rectangleSize.Height / 2), rectangleSize);

        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            Rectangle rectangle;
            do
            {
                var point = MovePointToRectangleSize(Spiral.GetNextPoint(), rectangleSize);
                rectangle = new Rectangle(point, rectangleSize);
            } while (IsRectangleIntersectsWithOther(rectangle));
            return GetApproximatedRectangle(rectangle); //Не очень удачное название
        }

        private Point MovePointToRectangleSize(Point point, Size rectangleSize) //Честно говоря так и не понял, что этот метод делает и зачем ему Center
        {
            if (point.X <= Center.X)
                point.Offset(-rectangleSize.Width / 2, 0);
            if (point.Y <= Center.Y)
                point.Offset(0, -rectangleSize.Height / 2);
            return point;
        }

        private Rectangle GetApproximatedRectangle(Rectangle rectangle)
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
            var centerVector = new Point(Center.X - rectangle.X, Center.Y - rectangle.Y); //vectorToCenter
            if (centerVector.X != 0)
                rectangle = TryMoveRectangle(rectangle, new Point(Math.Sign(centerVector.X) * 1, 0)); //Что?? умножение на 1?
            if (centerVector.Y != 0)
                rectangle = TryMoveRectangle(rectangle, new Point(0, Math.Sign(centerVector.Y) * 1));
            return rectangle;
        }

        private Rectangle TryMoveRectangle(Rectangle rectangle, Point offset)
        {
            rectangle.Offset(offset);
            if (IsRectangleIntersectsWithOther(rectangle))
                rectangle.Offset(new Point(-offset.X, -offset.Y));
            return rectangle;
        }

        public bool IsRectangleIntersectsWithOther(Rectangle rectangle)
                    => Rectangles.Any(rectangle.IntersectsWith);

        public void SaveToBmp(string path) //Лучше отделять логику от графики, поэтому лучше это сделать в отдельном классе
        {
            var pens = new List<Pen>
            {

                Pens.Red,
                Pens.Blue,
                Pens.Aqua,
                Pens.BlueViolet,
                Pens.Chartreuse,
                Pens.Brown,
                Pens.DarkGreen,
                Pens.DarkBlue,
                Pens.DarkGoldenrod,
                Pens.DeepPink,
                Pens.Orange
            };

            var bitmap = new Bitmap(200, 200);
            var graphics = Graphics.FromImage(bitmap);
            for (var i = Rectangles.Count - 1; i >= 0; i--)
                graphics.DrawRectangle(pens[i % pens.Count], Rectangles[i]);
            bitmap.SetPixel(Center.X, Center.Y, Color.Black);
            bitmap.Save(path);
        }
    }
}

