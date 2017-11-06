using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class LogarithmicSpiralTests
    {

        [Test]
        public void GetNextPoint_FirstPoint_ShouldBeCenter()
        {
            var center = new PointF(1, 1);
            var spiral = new LogarithmicSpiral(center);
            var point = spiral.GetNextPoint();
            point.Should().Be(center);
        }

        [Test]
        public void GetNextPoint_ShouldGetPoints_FromSpiral()
        {
            var center = new PointF(0, 0);
            var spiral = new LogarithmicSpiral(center);
            var pointAngles = new List<double>();
            var pointDistances = new List<double>();

            for (var i = 0; i < 1000; i++)
            {
                pointDistances.Add(CalculateDistanceBetween(spiral.GetNextPoint(), center));
                pointAngles.Add(DeflectionAngle(spiral.TurnsDistance, spiral.TurnsRadius, pointDistances[i]));
            }
            for (var i = 1; i < pointAngles.Count; i++)
                pointAngles[i].Should().BeApproximately(spiral.AngleShift * (i - 1), 1e-5);
            //pointAngles[i].Should().BeGreaterThan(pointAngles[i - 1]);
            //pointDistances[i].Should().BeGreaterThan(pointDistances[i - 1]);

        }

        [Test]
        public void GetNextPoint_ShouldNot_GetEqualPoints()
        {
            var center = new PointF(1, 1);
            var spiral = new LogarithmicSpiral(center);
            var points = new List<PointF>();
            for (var i = 0; i < 100; i++)
            {
                var point = spiral.GetNextPoint();
                points.Should().NotContain(point);
                points.Add(point);
            }
        }

        private static double CalculateDistanceBetween(PointF point, PointF center)
            => Math.Sqrt(Math.Pow(point.X - center.X, 2) + Math.Pow(point.Y - center.Y, 2));

        //Что-то сложное
        private static double DeflectionAngle(double b, double a, double r) => 1 / b * Math.Log(r / a);
    }
}