
using System.Drawing; //Пустота выше
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Constraints; //Не нужный using

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        [Test]
        public void CircularCloudLayouter_ShouldHaveCenter_AfterCreating() //Бесполезный тест, центр не обязательно делать публичным
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
        } //Нужна пустота
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
            Assert.False(isIntersects); //Здесь сообщение об ошибке будет не очень хорошее
        }
        /*
            Не все требования вычитываются из тестов:
                - Форма итогового облака должна быть близка к кругу с центром в точке center.
                - Прямоугольники не должны пересекаться друг с другом.
                - Облако должно быть плотным, чем плотнее, тем лучше.
        
            На TDD опять же слабо тянет, тесты пройдут, если всегда возвращать один и тот же прямоугольник

            Ещё есть 'Задача 3'

            Если хочешь вау эффект, можно слова рисовать

            Отделяй пустой строкой: Arange \n Act \n Assert, чтоб видно было где какая 'A'
         */
    }
}