using System;
using System.Drawing;
using System.IO;
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
        public void WriteToFile()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                $"failedTests/{TestContext.CurrentContext.Test.FullName}.bmp");
            CloudDrawer.DrawToBmp(path, cloud);
        }

        private CircularCloudLayouter cloud;

        [TestCase(0, TestName = "NoElements_AfterCreating")]
        [TestCase(1, TestName = "OneElement_AfterOneAddition")]
        [TestCase(2, TestName = "TwoElements_AfterTwoAdditions")]
        [Timeout(1000)]
        public void CircularCloudLayouter_ShouldHave(int count)
        {
            cloud = new CircularCloudLayouter(new Point(200, 200));

            FillCloudWithRandomRectangles(count);

            cloud.Rectangles.Count.Should().Be(count);
        }

        [Test]
        [Timeout(1000)]
        public void CircularCloudLayouter_FirstRectangle_HaveRightPosition()
        {
            cloud = new CircularCloudLayouter(new Point(200, 200));

            FillCloudWithRandomRectangles(1);

            cloud.Rectangles.First().Location.Should().Be(new Point(200, 200));
        }

        [Test]
        public void CircularCloudLayouter_Rectangles_AreTight()
        {
            cloud = new CircularCloudLayouter(new Point(200, 200));

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
        public void Rectangles_ShouldNotIntersect_WhenMoreThanOneRectangle()
        {
            cloud = new CircularCloudLayouter(new Point(200, 200));

            FillCloudWithRandomRectangles(100);
            for (var i = 0; i < cloud.Rectangles.Count; i++)
                for (var j = 0; j < cloud.Rectangles.Count; j++)
                {
                    if (i == j) continue;
                    cloud.Rectangles[i].Should()
                        .Match(x => !((Rectangle)x).IntersectsWith(cloud.Rectangles[j]),
                            cloud.Rectangles[j].ToTestString());
                }
        }

        [TestCase(10, 10, TestName = "When positive size")]
        [TestCase(0, 0, TestName = "When zero size")]
        [TestCase(-10, -10, TestName = "When negative size")]
        public void Rectangles_ShouldHave_RightSize(int width, int height)
        {
            cloud = new CircularCloudLayouter(new Point(200, 200));
            cloud.PutNextRectangle(new Size(width, height));
            cloud.Rectangles.First().Size.Should().Be(new Size(width, height));
        }
        
        private void FillCloudWithRandomRectangles(int rectangleCount, int seed = 1)
        {
            var rand = new Random(seed);
            for (var i = 0; i < rectangleCount; i++)
            {
                var width = rand.Next(5, 20);
                var heigth = rand.Next(5, 20);
                cloud.PutNextRectangle(new Size(width, heigth));
            }
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