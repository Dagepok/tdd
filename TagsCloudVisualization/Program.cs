using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var cloud = new CircularCloudLayouter(new Point(400, 400));
            var rand = new Random();
            for (var i = 0; i < 100; i++)
            {
                var width = rand.Next(5, 20);
                var heigth = rand.Next(5, 20);
                cloud.PutNextRectangle(new Size(width, heigth));

            }
            cloud.CreateBMP("1.bmp");
        }
    }
}

