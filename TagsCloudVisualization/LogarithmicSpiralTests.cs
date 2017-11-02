using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class LogarithmicSpiralTests
    {
        [Test]
        public void GetNextPoint_ShouldNotGetCenter() //Это странно, обычно спираль из центра начинается
        {
            var center = new Point(1, 1);
            var spiral = new LogarithmicSpiral(center);
            var point = spiral.GetNextPoint();
            point.Should().NotBe(center);
        }
        [Test]
        public void GetNextPoint_ShouldGet_NotEqualPoints()
        {
            var center = new Point(1, 1);
            var spiral = new LogarithmicSpiral(center);
            var firstPoint = spiral.GetNextPoint();
            var secondPoint = spiral.GetNextPoint();
            firstPoint.Should().NotBe(secondPoint);
        }

        //Кажется, маловато тестов, особенно, если ты писал в TDD (что надо было сделать).
        //Чтоб пройти эти тесты достаточно возвращать случайную точку))
    }
}