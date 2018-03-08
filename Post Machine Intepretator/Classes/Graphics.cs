using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Post_Machine_Intepretator.Classes
{
    public class Graphics
    {
        //Этот модуль отвечает за отрисовку отрезка на поверхности Canvas
        static void drawTable(Canvas canvas, int len)
        {
            Line lineU = new Line()
            {
                Stroke = Brushes.Black,
                X1 = 10,
                X2 = canvas.Width - 20,
                Y1 = 10,
                Y2 = 10
            };
            canvas.Children.Add(lineU);
            Line lineD = new Line()
            {
                Stroke = Brushes.Black,
                X1 = 10,
                X2 = canvas.Width - 20,
                Y1 = canvas.Height - 50,
                Y2 = canvas.Height - 50
            };
            canvas.Children.Add(lineD);

            for (int i = 0; i != len + 1; i++)
            {
                Line line = new Line()
                {
                    Stroke = Brushes.Black,
                    X1 = 10 + 50 * i,
                    X2 = 10 + 50 * i,
                    Y1 = 10,
                    Y2 = canvas.Height - 50
                };
                canvas.Children.Add(line);
            }
        }
        static void drawMark(Canvas canvas, Dictionary<int, int> listcells)
        {
            try
            {
                foreach (KeyValuePair<int, int> item in listcells)
                {
                    if (item.Value == 1)
                    {
                        Line linel = new Line()
                        {
                            StrokeThickness = 5,
                            Stroke = Brushes.Red,
                            X1 = 20 + 50 * item.Key,
                            X2 = 35 + 50 * item.Key,
                            Y1 = 25,
                            Y2 = canvas.Height - 70
                        };
                        canvas.Children.Add(linel);
                        Line liner = new Line()
                        {
                            StrokeThickness = 5,
                            Stroke = Brushes.Red,
                            X1 = 50 + 50 * item.Key,
                            X2 = 35 + 50 * item.Key,
                            Y1 = 25,
                            Y2 = canvas.Height - 70
                        };
                        canvas.Children.Add(liner);
                    }
                }
            }
            catch { }
        }
        static void drawKarret(Canvas canvas,int karretpos)
        {
           
            try
            {
                PointCollection collection = new PointCollection();
                collection.Add(new Point(20 + 50 * karretpos, canvas.Height - 10));
                collection.Add(new Point(50 + 50 * karretpos, canvas.Height - 10));
                collection.Add(new Point(35 + 50 * karretpos, canvas.Height - 45));
                Polygon tangle = new Polygon()
                {
                    Fill = Brushes.Black,
                    Points = collection

                };
                canvas.Children.Add(tangle);
            }
            catch { }
        }
        public static void drawAll(Canvas canvas, Dictionary<int,int> listcells,int karretpos, int len)
        {
            canvas.Width = 50 * len + 30;
            canvas.Children.RemoveRange(0, canvas.Children.Count);//удалить всё
            drawTable(canvas, len);
            drawMark(canvas,listcells);
            drawKarret(canvas, karretpos);
            
        }
    }
}
