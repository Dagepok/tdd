using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{

    public interface ISpiral
    {
        IEnumerable<Point> PossiblePoints();
        Point GetNextPoint();
    }
    public class LogarithmicSpiral : ISpiral
    {

        public Point Center { get; set; }
        private readonly IEnumerable<Point> possiblePoints;
        private readonly IEnumerator<Point> enumerator;

        public LogarithmicSpiral(Point center)
        {
            Center = center;
            possiblePoints = PossiblePoints();
            enumerator = possiblePoints.GetEnumerator();
        }

        public Point GetNextPoint()
        {
            enumerator.MoveNext();
            var point = enumerator.Current;
            return new Point(point.X + Center.X, point.Y + Center.Y);
        }
        public IEnumerable<Point> PossiblePoints()
        {
            const int a = 10;
            const double b = 0.15;
            const int angleShift = 6;
            var angle = 0;
            while (true)
            {
                var rad = angle * Math.PI / 180;
                var x = (int)(a * Math.Pow(Math.E, rad * b) * Math.Cos(rad));
                var y = (int)(a * Math.Pow(Math.E, rad * b) * Math.Sin(rad));
                var point = new Point(x, y);
                yield return point;
                angle = angle + angleShift;

            }

        }
    }

}