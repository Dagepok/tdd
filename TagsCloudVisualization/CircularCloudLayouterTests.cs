using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        [TearDown]
        public void RightToFile()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Failed))
            {
            }
        }

        private static List<Rectangle> GetRectangles(CircularCloudLayouter cloud, int rectangleCount)
        {
            var rectangles = new List<Rectangle>();
            var rand = new Random();
            for (var i = 0; i < rectangleCount; i++)
            {
                var width = rand.Next(5, 20);
                var heigth = rand.Next(5, 20);
                rectangles.Add(cloud.PutNextRectangle(new Size(width, heigth)));
            }
            return rectangles;
        }

        [TestCase(0, TestName = "NoElements_AfterCreating")]
        [TestCase(1, TestName = "OneElement_AfterOneAddition")]
        [TestCase(2, TestName = "TwoElements_AfterTwoAdditions")]
        [Timeout(1000)]
        public void CircularCloudLayouter_ShouldHave(int count)
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = GetRectangles(cloud, count);

            rectangles.Count.Should().Be(count);
        }


        [Test]
        [Timeout(1000)]
        public void CircularCloudLayouter_FirstRectangle_HaveRightPosition()
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));

            var rectangle = GetRectangles(cloud, 1).First();

            rectangle.Location.Should().Be(new Point(0, 0));
        }

        [Test]
        public void CircularCloudLayouter_Rectangles_AreTight()
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = new List<Rectangle>();

            for (var i = 0; i < 9; i++)
                rectangles.Add(cloud.PutNextRectangle(new Size(20, 20)));

            foreach (var rectangle in rectangles)
            {
                rectangle.X.Should().BeGreaterOrEqualTo(-20);
                rectangle.Y.Should().BeGreaterOrEqualTo(-40);
                rectangle.X.Should().BeLessOrEqualTo(40);
                rectangle.Y.Should().BeLessOrEqualTo(20);
            }
        }

        [Test]
        public void CircularCloudLayouter_ShouldNotGet_SameRectangles()
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = new List<Rectangle>();

            for (var i = 0; i < 100; i++)
            {
                var rectangle = cloud.PutNextRectangle(new Size(10, 10));

                rectangles.Should().NotContain(rectangle);

                rectangles.Add(rectangle);
            }
        }

        [Test]
        public void Rectangles_ShouldNotIntersect_WhenMoreThanOneRectangle()
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));

            var rectangles = GetRectangles(cloud, 100);

            foreach (var rectangle in rectangles)
                foreach (var otherRectangle in rectangles)
                {
                    if (rectangle.Equals(otherRectangle)) continue;
                    rectangle.Should().Match(x => !((Rectangle)x).IntersectsWith(otherRectangle),
                        otherRectangle.ToTestString());
                }
            /*
                Не все требования вычитываются из тестов:
                    - Форма итогового облака должна быть близка к кругу с центром в точке center.
                    +- Прямоугольники не должны пересекаться друг с другом.
                    - Облако должно быть плотным, чем плотнее, тем лучше.

                + На TDD опять же слабо тянет, тесты пройдут, если всегда возвращать один и тот же прямоугольник

                Ещё есть 'Задача 3'

                Если хочешь вау эффект, можно слова рисовать

                Отделяй пустой строкой: Arange \n Act \n Assert, чтоб видно было где какая 'A'
             */
        }
    }

    public static class TestExtensions
    {
        public static string ToTestString(this Rectangle rectangle)
        {
            return
                $"rectangle IntersectsWith  X={rectangle.X},Y={rectangle.Y},Width={rectangle.Width},Height={rectangle.Height}";
        }
    }
}