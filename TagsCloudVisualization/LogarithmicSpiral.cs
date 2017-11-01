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
            const double turnsRadius = 1;
            const double turnsDistance = 0.015;
            const int angleShift = 1;
            var angle = 0;
            while (true)
            {
                var rad = angle * Math.PI / 180;
                var x = (int)(turnsRadius * Math.Pow(Math.E, rad * turnsDistance) * Math.Cos(rad));
                var y = (int)(turnsRadius * Math.Pow(Math.E, rad * turnsDistance) * Math.Sin(rad));
                var point = new Point(x, y);
                yield return point;
                angle = angle + angleShift;

            }

        }
    }

}