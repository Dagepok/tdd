
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Constraints;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        [Test]
        public void CircularCloudLayouter_ShouldHaveCenter_AfterCreating()
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            cloud.Center.Should().Be(new Point(0, 0));
        }

        [Test]
        public void CircularCloudLayouter_ShouldBeEmpty_AfterCreating()
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            cloud.RectanglesCount.Should().Be(0);
        }

        [Test]
        public void CircularCloudLayouter_ShouldHaveOneElement_AfterPuttingOneRect()
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            cloud.PutNextRectangle(new Size(10, 10));
            cloud.RectanglesCount.Should().Be(1);

        }

        [Test]
        public void CircularCloudLayouter_ShouldHaveTwoElements_AfterPuttingTwoRect()
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            cloud.PutNextRectangle(new Size(10, 10));
            cloud.PutNextRectangle(new Size(10, 10));
            cloud.RectanglesCount.Should().Be(2);
        }

        [Test]
        public void CircularCloudLayouter_FirstRectangle_HaveRightPosition()
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            cloud.PutNextRectangle(new Size(10, 10));
            cloud.Rectangles.First().Location.Should().Be(new Point(-5, -5));
        }
        [Test]
        [Timeout(1000)]
        public void Rectangles_ShouldNotIntersect_WhenRectanglesMoreThanOne()
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            for (var i = 0; i < 1000; i++)
                cloud.PutNextRectangle(new Size(10, 10));
            var isIntersects = false;
            foreach (var rectangle in cloud.Rectangles)
                isIntersects = cloud.Rectangles.Any(rect => !rect.Equals(rectangle) && rect.IntersectsWith(rectangle));
            Assert.False(isIntersects);
        }
    }
}