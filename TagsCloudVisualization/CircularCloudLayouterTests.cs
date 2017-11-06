using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using FluentAssertions;
using FluentAssertions.Common;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {


        [TestCase(0, TestName = "NoElements_AfterCreating")]
        [TestCase(1, TestName = "OneElement_AfterOneAddition")]
        [TestCase(2, TestName = "TwoElements_AfterTwoAdditions")]
        [Timeout(1000)]
        public void CircularCloudLayouter_ShouldHave(int count)
        {
            var rectangles = new List<Rectangle>();
            var cloud = new CircularCloudLayouter(new Point(0, 0));

            for (var i = 0; i < count; i++)
                rectangles.Add(cloud.PutNextRectangle(new Size(10, 10)));

            rectangles.Count.Should().Be(count);
        }


        [Test]
        [Timeout(1000)]
        public void CircularCloudLayouter_FirstRectangle_HaveRightPosition()
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));

            var rectangle = cloud.PutNextRectangle(new Size(10, 10));

            rectangle.Location.Should().Be(new Point(0, 0));
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
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 100; i++)
                rectangles.Add(cloud.PutNextRectangle(new Size(10, 10)));

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
            return $"rectangle IntersectsWith  X={rectangle.X},Y={rectangle.Y},Width={rectangle.Width},Height={rectangle.Height}";
        }
    }
}