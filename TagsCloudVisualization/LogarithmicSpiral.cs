using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    //Почему именно логорифмическая спираль, а не простая архимедова? 
    //Нашёл формулу логарифмической раньше, и сделал её
    public class LogarithmicSpiral : ISpiral
    {
        private readonly IEnumerator<Point> enumerator;
        private readonly IEnumerable<Point> possiblePoints;


        public LogarithmicSpiral(Point center)
        {
            Center = center;
            possiblePoints = PossiblePoints();
            enumerator = possiblePoints.GetEnumerator();
        }

        private Point Center { get; }

        public Point GetNextPoint()
        {
            var point = enumerator.Current;
            enumerator.MoveNext();
            return new Point(point.X + Center.X, point.Y + Center.Y);
        }

        private static IEnumerable<Point> PossiblePoints()
        {
            const double angleShift = 25 * 0.0174533; // 0,0174533 rad = 1°
            var angle = 0.0;
            while (true)
            {
                var x = GetPointCoordinate(angle, Math.Cos);
                var y = GetPointCoordinate(angle, Math.Sin);
                var point = new Point(x, y);
                yield return point;
                angle += angleShift;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private static int GetPointCoordinate(double angle, Func<double, double> function)
        {
            const double turnsRadius = 10;
            const double turnsDistance = 0.015;
            return (int) Math.Round(turnsRadius * Math.Exp(angle * turnsDistance) * function(angle));
        }
    }
}