using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class LogarithmicSpiral : ISpiral
    {
        private readonly IEnumerator<Point> enumerator;
        private readonly IEnumerable<Point> possiblePoints;  //Не нужное поле, решарпер подчёркивает


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
            //0.0174533 ~ Math.PI / 180 в коде лучше так и написать.
            const double angleShift = 25 * 0.0174533; // 0,0174533 rad = 1° 
            var angle = 0.0;
            while (true) //for (var angle = 0.0;;angle += angleShift) - нагляднее и короче
            {
                var x = GetPointCoordinate(angle, Math.Cos);
                var y = GetPointCoordinate(angle, Math.Sin);
                var point = new Point(x, y);
                yield return point;
                angle += angleShift;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        //Получился странный метод
        //Копипаста была в том, что 'turnsRadius * Math.Exp(angle * turnsDistance)' считалось два раза
        //Можно было выделить понятные и простые методы
        // 1. Получение радиуса по углу, по сути это функция r=ae^(b*angle)
        // 2. Перевод полярных координат в декартовы: Point ConvertToCartesianCoordinates(double radius, double angle)
        private static int GetPointCoordinate(double angle, Func<double, double> function)
        {
            const double turnsRadius = 10;
            const double turnsDistance = 0.015;
            return (int) Math.Round(turnsRadius * Math.Exp(angle * turnsDistance) * function(angle));
        }
    }
}