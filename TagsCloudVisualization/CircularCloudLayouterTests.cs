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
            //Используй оператор '==' вместо Equals, это нагляднее, а иногда позволяет не наступить на NullReferenceException
            if (!TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Failed)) return;
            //Для склейки путей нужно использовать Path.Combine(...)
            var path = AppDomain.CurrentDomain.BaseDirectory + $"/failedTests/{TestContext.CurrentContext.Test.FullName}.bmp";
            CloudDrawer.DrawToBmp(path, cloud);
        }

        private CircularCloudLayouter cloud;

        //С рандомом можно получить не стабильные тесты, подумай, что с этим можно сделать
        //Приватный метод - вниз
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
            cloud = new CircularCloudLayouter(new Point(200,200));

            FillCloudWithRandomRectangles(count);

            cloud.Rectangles.Count.Should().Be(count);
        }


        [Test]
        [Timeout(1000)]
        public void CircularCloudLayouter_FirstRectangle_HaveRightPosition()
        {
            cloud = new CircularCloudLayouter(new Point(200,200));

            FillCloudWithRandomRectangles(1);

            cloud.Rectangles.First().Location.Should().Be(new Point(200, 200));
        }

        [Test]
        public void CircularCloudLayouter_Rectangles_AreTight()
        {
            cloud = new CircularCloudLayouter(new Point(200,200));

            for (var i = 0; i < 9; i++)
                cloud.PutNextRectangle(new Size(20, 20));

            foreach (var rectangle in cloud.Rectangles)
            {
                rectangle.X.Should().BeGreaterOrEqualTo(180);
                rectangle.Y.Should().BeGreaterOrEqualTo(160);
                rectangle.X.Should().BeLessOrEqualTo(240);
                rectangle.Y.Should().BeLessOrEqualTo(220);
            }
        }

        [Test]
        public void CircularCloudLayouter_ShouldNotGet_SameRectangles()
        {
            cloud = new CircularCloudLayouter(new Point(200,200));

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
            cloud = new CircularCloudLayouter(new Point(200,200));

            FillCloudWithRandomRectangles(100);

            //Можно сделать циклы for, как в предыдущем тесте, и просто проверять что прямоугольники не пересекаются,
            //тогда и предыдущий тест будет не нужен.
            foreach (var rectangle in cloud.Rectangles)
                foreach (var otherRectangle in cloud.Rectangles)
                {
                    if (rectangle.Equals(otherRectangle)) continue; //'=='
                    rectangle.Should().Match(x => !((Rectangle)x).IntersectsWith(otherRectangle),
                        otherRectangle.ToTestString());
                }
        }

        // не проверил, что прямоугольники соответсвуют размеру
    }

    public static class TestExtensions
    {
        public static string ToTestString(this Rectangle rectangle)
        {
            //У Rectangle и так хороший ToString(), формируй эту строчку на месте, не нужнен этот Extensions
            return
                $"rectangle IntersectsWith  X={rectangle.X},Y={rectangle.Y},Width={rectangle.Width},Height={rectangle.Height}";
        }
    }
}