using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class LogarithmicSpiralTests
    {
        [Test]
        public void GetNextPoint_FirstPoint_ShouldBeCenter() //Это странно, обычно спираль из центра начинается
        {
            var center = new Point(1, 1);
            var spiral = new LogarithmicSpiral(center);
            var point = spiral.GetNextPoint();
            point.Should().Be(center);
        }

        [Test]
        public void GetNextPoint_ShouldNot_GetEqualPoints()
        {
            var center = new Point(1, 1);
            var spiral = new LogarithmicSpiral(center);
            var points = new List<Point>();
            for (var i = 0; i < 100; i++)
            {
               var  point = spiral.GetNextPoint();
                points.Should().NotContain(point);
                points.Add(point);
            }
        }

        [Test]
        public void GetNextPoint_ShouldGetPoints_FromSpiral()
        {
            double turnsRadius = 1;
            double turnsDistance = 0.03;
            var center = new Point(1, 1);
            var spiral = new LogarithmicSpiral(center);
            var pointAngles = new List<double>();
            for (var i = 0; i < 1000; i++)
            {
                var dictanceToCenter = DictanceToCenter(spiral.GetNextPoint(), center);
                pointAngles.Add(DeflectionAngle(turnsDistance, turnsRadius, dictanceToCenter));
            }
            for (var i = 1; i < 1000; i++)
            {
                var b = Math.Sin(pointAngles[i]) / Math.Cos(pointAngles[i]);
                b.Should().Be(turnsDistance);
            }


        }

        private static bool IsRightSpiralPoint(int iterator, IReadOnlyList<double> dictances, IReadOnlyList<double> angles)
            => dictances[iterator] > dictances[iterator - 1] && angles[iterator] > angles[iterator - 1];

        private static double DictanceToCenter(Point point, Point center) =>
            Math.Sqrt(Math.Pow(point.X - center.X, 2) + Math.Pow(point.Y - center.Y, 2));

        private double DeflectionAngle(double b, double a, double r) => 1 / b * Math.Log(r / a);
        //Кажется, маловато тестов, особенно, если ты писал в TDD (что надо было сделать).
        //Чтоб пройти эти тесты достаточно возвращать случайную точку))
    }
}