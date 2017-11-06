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
        //Приватные методы должны быть внизу

        //CalculateDistanceBetween хорошее название для этого метода
        private static double DictanceToCenter(Point point, Point center)
        {
            return Math.Sqrt(Math.Pow(point.X - center.X, 2) + Math.Pow(point.Y - center.Y, 2));
        }

        //Что-то сложное
        private static double DeflectionAngle(double b, double a, double r)
        {
            return 1 / b * Math.Log(r / a);
        }

        [Test]
        public void GetNextPoint_FirstPoint_ShouldBeCenter()
        {
            var center = new Point(1, 1);
            var spiral = new LogarithmicSpiral(center);
            var point = spiral.GetNextPoint();
            point.Should().Be(center);
        }

        [Test]
        public void GetNextPoint_ShouldGetPoints_FromSpiral()
        {
            //Будет больно, если захотим поправить константы, придётся править в двух местах
            double turnsRadius = 10;
            var turnsDistance = 0.015;
            var center = new Point(1, 1);
            var spiral = new LogarithmicSpiral(center);
            var pointAngles = new List<double>();
            var pointDistances = new List<double>();

            for (var i = 0; i < 1000; i++)
            {
                pointDistances.Add(DictanceToCenter(spiral.GetNextPoint(), center));
                pointAngles.Add(DeflectionAngle(turnsDistance, turnsRadius, pointDistances[i]));
            }

            for (var i = 0; i < pointAngles.Count - 1; i++)
            {
                pointAngles[i].Should().BeLessOrEqualTo(pointAngles[pointAngles.Count - 1]);
                pointDistances[i].Should().BeLessOrEqualTo(pointDistances[pointDistances.Count - 1]);
            }

            //Почему нельзя просто "BeLessThan"?
            //Может стоит проверять, что следующая точка дальше от центра чем предыдущая?
            //А предыдущий угол больше следующего, сделать это можно только для первой координатной четверти (это избавит от сложных расчётов в методе DeflectionAngle)
            //Идея проверки крутая!
        }

        [Test]
        public void GetNextPoint_ShouldNot_GetEqualPoints()
        {
            var center = new Point(1, 1);
            var spiral = new LogarithmicSpiral(center);
            var points = new List<Point>();
            for (var i = 0; i < 100; i++)
            {
                var point = spiral.GetNextPoint();
                points.Should().NotContain(point);
                points.Add(point);
            }
        }
    }
}