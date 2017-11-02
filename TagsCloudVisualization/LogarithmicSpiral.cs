using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{//пустой перенос ниже!?

    public interface ISpiral //вынеси интерфейс в отдельный файл
    {
        IEnumerable<Point> PossiblePoints(); //этот метод не нужен в интерфейсе, если он не используется публично
        Point GetNextPoint();
    }
    //Почему именно логорифмическая спираль, а не простая архимедова?
    public class LogarithmicSpiral : ISpiral
    {//пустой перенос ниже!?

        public Point Center { get; set; } //Раз Center не используется публично, то можно сделать его приватным
        private readonly IEnumerable<Point> possiblePoints;
        private readonly IEnumerator<Point> enumerator;
        private Point LastPoint { get; set; } //Нафиг это поле, если можно завести переменную). Снизу много пустых переносов, хватит одного)


        public LogarithmicSpiral(Point center)
        {
            Center = center;
            possiblePoints = PossiblePoints();
            enumerator = possiblePoints.GetEnumerator();
        }

        public Point GetNextPoint()
        {
            enumerator.MoveNext();
            var point = enumerator.Current;
            return new Point(point.X + Center.X, point.Y + Center.Y);
        }//А вот тут неплохо бы добавить перенос
        public IEnumerable<Point> PossiblePoints() //этот метод должен стать приватным
        {
            const double turnsRadius = 1;
            const double turnsDistance = 0.015;
            const int angleShift = 1;
            var angle = -angleShift; //почему начинаем с -1 градуса, выглядит криво
            while (true)
            {   //пустой перенос ниже!?

                angle = angle + angleShift; //angle += angleShift
                var rad = angle * Math.PI / 180; //В принципе можно сразу в радианах считать, если удобнее в градусах то ок (можно в отдельный метод вынести)
                var x = (int)(turnsRadius * Math.Pow(Math.E, rad * turnsDistance) * Math.Cos(rad)); //CTRL+C
                //лучше использовать Math.Round и Math.Exp
                var y = (int)(turnsRadius * Math.Pow(Math.E, rad * turnsDistance) * Math.Sin(rad)); //CTRL+V
                var point = new Point(x, y);
                if (LastPoint.Equals(point)) continue; //что за костыль? Может angleShift увеличить?
                yield return point;
                LastPoint = point; //пустой перенос ниже!?

            }

        }
    }

}