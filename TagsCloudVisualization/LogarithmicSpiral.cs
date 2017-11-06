using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class LogarithmicSpiral : ISpiral
    {
        private readonly IEnumerator<PointF> enumerator;

        public readonly double TurnsRadius;
        public readonly double TurnsDistance;
        public readonly double AngleShift;

        private PointF Center { get; }

        public LogarithmicSpiral(PointF center, double turnsRadius = 10, double turnsDistance = 0.015, int angleShiftGradus = 10)
        {

            Center = center;
            TurnsRadius = turnsRadius;
            TurnsDistance = turnsDistance;
            AngleShift = angleShiftGradus * Math.PI / 180; ;
            var possiblePoints = PossiblePoints();
            enumerator = possiblePoints.GetEnumerator();
        }


        public PointF GetNextPoint()
        {
            var point = enumerator.Current;
            enumerator.MoveNext();
            return new PointF(point.X + Center.X, point.Y + Center.Y);
        }

        private IEnumerable<PointF> PossiblePoints()
        {
            for (var angle = 0.0; ; angle += AngleShift)
                yield return ConvertToCartesianCoordinates(angle);
            // ReSharper disable once IteratorNeverReturns
        }

        private double GetRadiusByAngle(double angle) => TurnsRadius * Math.Exp(angle * TurnsDistance);

        private PointF ConvertToCartesianCoordinates(double angle)
        {
            var x = GetRadiusByAngle(angle) * Math.Cos(angle);
            var y = GetRadiusByAngle(angle) * Math.Sin(angle);
            return new PointF((float)x, (float)y);
        }
    }
}