using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Windows;
using Post_Machine_Interpritator.Classes;
using Microsoft.Win32;
namespace Post_Machine_Intepretator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Creat0r creator;
        RegistryKey soft;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        //получение
        int len=5; //длина отрезка (нужна для отрисовки)
        int karretpos=0; //позиция корретки
        Dictionary<int, int> listcells=new Dictionary<int, int>(); //отрезок


        //вполнение копии
        Dictionary<int, string> inputcode; //введённый код
        int tempcar;
        Dictionary<int, int> templistcells;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Отрисовка в интерпритаторе отрезка памяти
            for (int i = 0; i < len; listcells.Add(i++, 0)) ;
            canvas.Width = 50 * len + 30;
            Graphics.drawAll(canvas, listcells, karretpos, len);
        }
        private void creator_Closed(object sender, EventArgs e)
        {
            //Действия при закрытии сощдателя   
            len =creator.len;
            karretpos = creator.karretpos;
            listcells = creator.listcells;
            this.Show();
            //Отрисовка в интерпритаторе отрезка памяти
            canvas.Width = 50 * len + 30;
            Graphics.drawAll(canvas, listcells, karretpos, len);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Открытия окна создателя
            creator = new Creat0r();
            this.Hide();
            creator.Closing += creator_Closed;
            creator.Owner = this;
            creator.Show();
        }
        void process(string command)
        {
            //выполнение программы пользователя рекурсивно
            try { 
            if (!command.Contains('!'))//проверяем конец ли это программы
            {
                    if (!command.Contains('?'))//условие ли это
                    {
                        List<string> temp = deleteEmpty(command.Split(' ').ToList<string>());//парсим комманды и указатели т.е всё что идёт после :
                        if (temp[1] == "PointeExcept") return; //если вдруг нету указателя, то выполнение комманды завершить
                        if (temp[0].ToLower() == "v")//рисует ли это метку
                        {
                            templistcells[tempcar] = 1;
                            nextCommand(int.Parse(temp[1]));
                            
                        }
                        if (temp[0].ToLower() == "x")//затирает ли это метку
                        {
                            templistcells[tempcar] = 0;
                            nextCommand(int.Parse(temp[1]));
                        }
                        if (temp[0] == "->")//сдвиг вправо
                        {
                            tempcar++;
                            if (tempcar > len - 1) tempcar = len - 1;
                            nextCommand(int.Parse(temp[1]));
                        }
                        if (temp[0] == "<-")//сдвиг влево
                        {
                            tempcar--;
                            if (tempcar < 0) tempcar = 0;
                            nextCommand(int.Parse(temp[1]));
                        }
                        int temper;
                        if (int.TryParse(temp[0],out temper)) nextCommand(temper); //забираем указатель из пустой строки
                    }
                    else
                    {
                        try {
                            List<string> temp = deleteEmpty(command.Split(' ').ToList<string>()); 
                            //приводим второй и третий агрумент к виду [arg2]:[arg3]
                            temp.Remove("?");
                            for (int i = 0; i < temp.Count; i++)
                                if (temp[i].Contains(";"))
                                {
                                    if (temp[i].LastIndexOf(';') == temp[i].Length - 1)
                                    {
                                        temp[i] += temp[i + 1];
                                        temp.RemoveAt(i + 1);
                                    }
                                    if (temp[i].IndexOf(';') == 0)
                                    {
                                        temp[i-1] += temp[i];
                                        temp.RemoveAt(i);
                                    }
                                    break;
                                }
                            if(temp[0]== "PointeExcept") return;
                            try
                            {    
                                if (templistcells[tempcar] == 1)
                                    nextCommand(int.Parse(temp[0].Split(';')[0]));
                                else
                                    nextCommand(int.Parse(temp[0].Split(';')[1]));
                            }
                            catch (FormatException)
                            {
                                MessageBox.Show("В условие \"" + command + "\" в строка "+curline+" отсутсвуют необходимы аргументы!" + " Добавьте их и попробуйте ещё раз.", "Ошибка синтаксиса", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            catch(IndexOutOfRangeException)
                            {
                                MessageBox.Show("В условие \"" + command + "\" в строка " + curline + " отсутсвуют необходимы аргументы!" + " Добавьте их и попробуйте ещё раз.", "Ошибка синтаксиса", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
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
            //отрисовка изменений
            Graphics.drawAll(canvas, templistcells, tempcar, len);
        }

        void nextCommand(int line)
        {
            curline = line;
            //здесь мы просто проверяем есть ли данная строка, а когда не найдём её прекратим выполнение и выдадим ошибку
            try {
                process(inputcode[line]);
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("Строки "+line+" не существует! Добавьте её и попробуйте ещё раз.", "Ошибка синтаксиса", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
        }
        int curline;
        List<string> deleteEmpty(List<string> temp)
        {
            //удаляем всю не нужную пустоту
            while (temp[0] == "")
            {
                temp.RemoveAt(0);
            }
            try
            {
                while (temp[1] == "")
                {
                    temp.RemoveAt(1);
                }
            }catch (ArgumentOutOfRangeException)
            {
                //если вдруг отстутсвует второй аргумент комманды, то есть два варианта: либо это строка без команды, либо пользователь не поставил указатель на следующую строку
                if (int.TryParse(temp[0], out int temper)) temp.Add("");
                else
                {
                    MessageBox.Show("Отсутсвует указатель на следующую строку в строке "+curline+"!\n Добавьте его и попробуйте ещё раз.", "Ошибка синтаксиса", MessageBoxButton.OK, MessageBoxImage.Error);
                    temp.Insert(1,"PointeExcept");
                }
            }
            return temp;
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {

            RegistryKey soft = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("PostMachineInterpritator", true); ;
            soft.SetValue("emergencySave", input.Text);
            soft.Close();

            tempcar = karretpos;
            templistcells=new Dictionary<int, int>();
            foreach (KeyValuePair<int, int> i in listcells) templistcells.Add(i.Key,i.Value);
            //парсинг ввдённого кода
            inputcode = new Dictionary<int, string>();
            string[] s = input.Text.Split('\n');
            foreach (string i in s)
            {
                try {
                    if (i != "\r")//отбрасываем пустые строки
                    {
                        string[] temp = i.Split(':');
                        inputcode.Add(int.Parse(temp[0]), temp[1]);
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Строка: \""+i+ "\"\n" + " не содержит указатель строки \':\'","Ошибка синтаксиса",MessageBoxButton.OK,MessageBoxImage.Error);
                    return;
                }
                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show("Строка: \"" + i + "\"\n" + " не содержит указатель строки \':\', а также комманд!", "Ошибка синтаксиса", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
            }
            nextCommand(1);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            RegistryKey soft = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("PostMachineInterpritator", true);
            try {
                soft.DeleteValue("emergencySave");
            }
            catch { }
            
            soft.Close();
            //закрываем приложение при закрытие главной формы
            Environment.Exit(0);
        }
        Help helper;
        private void start_Copy_Click(object sender, RoutedEventArgs e)
        {
            //создаём окно справки
            try {
                helper.Close();
            } catch { }
            
            helper = new Help();
            helper.Show();
        }

        private void github_Click(object sender, RoutedEventArgs e)
        {
            //при нажатии на кнопку пользователю открывается репозиторий программы в браузере по умолчанию
            Process.Start("https://github.com/nikminer/PostMachineInterpritator");
        }

        private void input_Loaded(object sender, RoutedEventArgs e)
        {
            soft = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("PostMachineInterpritator", true);
            try
            {
                input.Text = soft.GetValue("emergencySave").ToString();
            }
            catch
            {
                Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("PostMachineInterpritator", true).Close(); ;
            }
        }
    }
}
