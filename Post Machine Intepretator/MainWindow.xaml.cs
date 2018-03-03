using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Windows;
using Post_Machine_Interpritator.Classes;

namespace Post_Machine_Intepretator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Creat0r creator;
        public MainWindow()
        {
            InitializeComponent();
        }
        //получение
        int len; //длина отрезка (нужна для отрисовки)
        int karretpos; //позиция корретки
        Dictionary<int, int> listcells; //отрезок


        //вполнение копии
        Dictionary<int, string> inputcode;
        int tempcar;
        Dictionary<int, int> templistcells;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            creator = new Creat0r();
            this.Hide();
            creator.Closing += MainWindow_Closed;
            creator.Owner = this;
            creator.Show();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            len =creator.len;
            karretpos = creator.karretpos;
            listcells = creator.listcells;
            this.Show();
            canvas.Width = 50 * len + 30;
            Graphics.drawAll(canvas, listcells, karretpos, len);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            creator = new Creat0r();
            this.Hide();
            creator.Closing += MainWindow_Closed;
            creator.Owner = this;
            creator.Show();
        }

        void process(string command)
        {
            try { 
            if (!command.Contains('!'))
            {
                if (!command.Contains('?'))
                {
                    string[] temp = command.Split(' ');
                    if (temp[0].ToLower() == "v")
                    {
                        templistcells[tempcar] = 1;
                        process(inputcode[int.Parse(temp[1])]);
                    }
                    if (temp[0].ToLower() == "x")
                    {
                        templistcells[tempcar] = 0;
                        process(inputcode[int.Parse(temp[1])]);
                    }
                    if (temp[0] == "->")
                    {
                        tempcar++;
                        if (tempcar > len - 1) tempcar = len - 1;
                        process(inputcode[int.Parse(temp[1])]);
                    }
                    if (temp[0] == "<-")
                    {
                        tempcar--;
                        if (tempcar < 0) tempcar = 0;
                        process(inputcode[int.Parse(temp[1])]);
                    }
                }
                else
                {
                    try { 
                        string[] temp = command.Split(' ')[1].Split(';');

                        if (templistcells[tempcar] == 1)
                            process(inputcode[int.Parse(temp[0])]);
                        else
                            process(inputcode[int.Parse(temp[1])]);
                    }
                    catch (System.StackOverflowException)
                    {
                        return;
                    }
                }
            }
            }
            catch (StackOverflowException)
            {
                return;
            }
            Graphics.drawAll(canvas, templistcells, tempcar, len);
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            tempcar = karretpos;
            templistcells=new Dictionary<int, int>();
            foreach (KeyValuePair<int, int> i in listcells) templistcells.Add(i.Key,i.Value);
            inputcode = new Dictionary<int, string>();
            string[] s = input.Text.Split('\n');
            foreach (string i in s)
            {
                string[] temp = i.Split(':');
                inputcode.Add(int.Parse(temp[0]), temp[1]);
            }
            process(inputcode[1]);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        Help helper;
        private void start_Copy_Click(object sender, RoutedEventArgs e)
        {
            try { helper.Close(); } catch { }
            
            helper = new Help();
            helper.Show();
        }

        private void github_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/nikminer/PostMachineInterpritator");
        }
    }
}
