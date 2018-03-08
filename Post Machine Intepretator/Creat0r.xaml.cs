using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Post_Machine_Intepretator.Classes;

namespace Post_Machine_Intepretator
{
    /// <summary>
    /// Логика взаимодействия для Creat0r.xaml
    /// </summary>
    public partial class Creat0r : Window
    {
        public Creat0r()
        {
            InitializeComponent();
        }
        public int len = 5;
        public int karretpos = 0;
        public Dictionary<int, int> listcells;
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Graphics.drawAll(canvas, listcells, karretpos, len);//нарисовать всё заново

            try
            {
                ArrLen.Content = Math.Round(e.NewValue);
                if (Math.Round(e.OldValue) < Math.Round(e.NewValue))
                    for (int i = (int)Math.Round(e.OldValue); i < Math.Round(e.NewValue); listcells.Add(i++, 0)) ;
                else
                    for (int i = (int)Math.Round(e.NewValue); i < Math.Round(e.OldValue); listcells.Remove(i++)) ;
            }
            catch { }
            len = (int)Math.Round(e.NewValue);
            canvas.Width = 50 * len + 30;
        }
        private void canvas_Loaded(object sender, RoutedEventArgs e)
        {
            listcells = new Dictionary<int, int>();
            for (int i = 0; i < len; listcells.Add(i++, 0)) ;


            canvas.Width = 50 * len + 30;
            Graphics.drawAll(canvas, listcells, karretpos, len);//нарисовать всё заново
        }
        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int index = (int)Math.Floor((e.GetPosition(canvas).X - 10) / 50);
                if (listcells.ElementAt(index).Value == 0)
                {
                    listcells.Remove(index);
                    listcells.Add(index, 1);
                }
                else
                {
                    listcells.Remove(index);
                    listcells.Add(index, 0);
                }


                Graphics.drawAll(canvas, listcells, karretpos, len);//нарисовать всё заново
            }
            catch { }
        }
        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            karretpos = (int)Math.Floor((e.GetPosition(canvas).X - 10) / 50);
            if (karretpos < 0) karretpos = 0;
            if (karretpos > len - 1) karretpos = len - 1;
            Graphics.drawAll(canvas, listcells, karretpos, len);//нарисовать всё заново
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

