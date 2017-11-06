using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CloudDrawer
    {
        public static void DrawToBmp(string path, List<Rectangle> rectangles, Point center)
        {
            var pens = new List<Pen>
            {

                Pens.Red,
                Pens.Blue,
                Pens.Aqua,
                Pens.BlueViolet,
                Pens.Chartreuse,
                Pens.Brown,
                Pens.DarkGreen,
                Pens.DarkBlue,
                Pens.DarkGoldenrod,
                Pens.DeepPink,
                Pens.Orange
            };

            var bitmap = new Bitmap(400, 400);
            var graphics = Graphics.FromImage(bitmap);
            for (var i = rectangles.Count - 1; i >= 0; i--)
                graphics.DrawRectangle(pens[i % pens.Count], rectangles[i]);
            bitmap.SetPixel(center.X, center.Y, Color.Black);
            bitmap.Save(path);
        }
    }
}