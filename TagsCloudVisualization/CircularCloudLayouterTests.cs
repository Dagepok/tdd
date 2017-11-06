using System;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Execution;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        [TearDown]
        public void WriteToFile()
        {
            if (!TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Failed)) return;
            var path = Directory.GetCurrentDirectory() + $"/failedTests/{TestContext.CurrentContext.Test.FullName}.bmp";
            CloudDrawer.DrawToBmp(path, cloud);
            Console.WriteLine("Tag cloud visualization saved to file <path>");
        }

        private CircularCloudLayouter cloud;

        private void FillCloudWithRandomRectangles(int rectangleCount)
        {
            var rand = new Random();
            for (var i = 0; i < rectangleCount; i++)
            {
                var width = rand.Next(5, 20);
                var heigth = rand.Next(5, 20);
                cloud.PutNextRectangle(new Size(width, heigth));
            }
        }

        [TestCase(0, TestName = "NoElements_AfterCreating")]
        [TestCase(1, TestName = "OneElement_AfterOneAddition")]
        [TestCase(2, TestName = "TwoElements_AfterTwoAdditions")]
        [Timeout(1000)]
        public void CircularCloudLayouter_ShouldHave(int count)
        {
            cloud = new CircularCloudLayouter(new Point(0, 0));

            FillCloudWithRandomRectangles(count);

            cloud.Rectangles.Count.Should().Be(count);
        }


        [Test]
        [Timeout(1000)]
        public void CircularCloudLayouter_FirstRectangle_HaveRightPosition()
        {
            cloud = new CircularCloudLayouter(new Point(0, 0));

            FillCloudWithRandomRectangles(1);

            cloud.Rectangles.First().Location.Should().Be(new Point(0, 0));
        }

        [Test]
        public void CircularCloudLayouter_Rectangles_AreTight()
        {
            cloud = new CircularCloudLayouter(new Point(0, 0));

            for (var i = 0; i < 9; i++)
                cloud.PutNextRectangle(new Size(20, 20));

            foreach (var rectangle in cloud.Rectangles)
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
            cloud = new CircularCloudLayouter(new Point(0, 0));

            for (var i = 0; i < 100; i++)
                cloud.PutNextRectangle(new Size(10, 10));

            for (var i = 0; i < cloud.Rectangles.Count; i++)
                for (var j = 0; j < cloud.Rectangles.Count; j++)
                {
                    if (i == j) continue;
                    cloud.Rectangles[i].Should().NotBe(cloud.Rectangles[j]);
                }
        }

        [Test]
        public void Rectangles_ShouldNotIntersect_WhenMoreThanOneRectangle()
        {
            cloud = new CircularCloudLayouter(new Point(0, 0));

            FillCloudWithRandomRectangles(100);

            foreach (var rectangle in cloud.Rectangles)
                foreach (var otherRectangle in cloud.Rectangles)
                {
                    if (rectangle.Equals(otherRectangle)) continue;
                    rectangle.Should().Match(x => !((Rectangle)x).IntersectsWith(otherRectangle),
                        otherRectangle.ToTestString());
                }
            /*
                Не все требования вычитываются из тестов:
                    - Форма итогового облака должна быть близка к кругу с центром в точке center.
                    +- Прямоугольники не должны пересекаться друг с другом.
                    +   - Облако должно быть плотным, чем плотнее, тем лучше.

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