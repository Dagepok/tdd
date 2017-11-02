
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

        [TestCase(0, TestName = "NoElements_AfterCreating")]
        [TestCase(1, TestName = "OneElement_AfterOneAddition")]
        [TestCase(2, TestName = "TwoElements_AfterTwoAdditions")]
        [Timeout(1000)]
        public void CircularCloudLayouter_ShouldHave(int count)
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            for (var i = 0; i < count; i++)
                cloud.PutNextRectangle(new Size(10, 10));
            cloud.Rectangles.Count.Should().Be(count);
        }


        [Test]
        [Timeout(1000)]
        public void CircularCloudLayouter_FirstRectangle_HaveRightPosition()
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            cloud.PutNextRectangle(new Size(10, 10));
            cloud.Rectangles.First().Location.Should().Be(new Point(-5, -5));
        }
        [Test]
        [Timeout(1000)]
        public void Rectangles_ShouldNotIntersect_WhenMoreThanOneRectangle()
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