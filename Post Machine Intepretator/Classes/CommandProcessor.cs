using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;

namespace Post_Machine_Intepretator.Classes
{
    //Этот модуль отвечает за рекурсивную обработку ввода и вывод результата
    class CommandProcessor
    {
        public static int karretpos;
        public static Dictionary<int, int> listcells;
        public static int curline;
        static Dictionary<int, string> inputcode; //введённый код
        public static int len;
        static void process(string command)
        {
            var args = CommandProcessor.deleteEmpty(command).Split(' ').ToList<string>();//парсим комманды и указатели т.е всё что идёт после :
            //выполнение программы пользователя рекурсивно
            if (args[0] != "!")//проверяем конец ли это программы
            {
                if (args[0].ToLower()!=Syntax.Default.ifer.ToLower())//условие ли это
                {
                    
                    try
                    {
                        //если вдруг отстутсвует второй аргумент комманды, то есть два варианта: либо это строка без команды, либо пользователь не поставил указатель на следующую строку
                        int temp;
                        if (int.TryParse(args[0], out temp)) args[1] = temp.ToString();
                        else
                            int.Parse(args[1]);
                    }
                    catch (Exception)
                    { 
                        MessageBox.Show("Отсутсвует указатель на следующую строку в строке " + curline + "!\n Добавьте его и попробуйте ещё раз.", "Ошибка синтаксиса", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (args[0].ToLower() == Syntax.Default.AddV.ToLower())//рисует ли тут метку
                    {
                        listcells[karretpos] = 1;
                        nextCommand(int.Parse(args[1]));
                    }
                    if (args[0].ToLower() == Syntax.Default.RemV.ToLower())//затирает ли тут метку
                    {
                        listcells[karretpos] = 0;
                        nextCommand(int.Parse(args[1]));
                    }
                    if (args[0].ToLower() == Syntax.Default.right.ToLower())//сдвиг вправо
                    {
                        karretpos++;
                        if (karretpos > len - 1) karretpos = len - 1;
                        nextCommand(int.Parse(args[1]));
                    }
                    if (args[0].ToLower() == Syntax.Default.left.ToLower())//сдвиг влево
                    {
                        karretpos--;
                        if (karretpos < 0) karretpos = 0;
                        nextCommand(int.Parse(args[1]));
                    }
                    int temper;
                    if (int.TryParse(args[0], out temper)) nextCommand(temper); //забираем указатель из пустой строки
                }
                else
                {
                    
                    try
                    {
                        if (listcells[karretpos] != 1)
                            nextCommand(int.Parse(args[1].Split(Syntax.Default.ifer_split)[0]));
                        else
                          nextCommand(int.Parse(args[1].Split(Syntax.Default.ifer_split)[1]));
                    }
                    catch (FormatException)
                    {
                        string text = "Условие \"" + command.Remove(command.Length - 1) + "\" строка " + curline + ",\n не содержит необходимые аргументы!\n Добавьте их и попробуйте ещё раз.";
                        MessageBox.Show(text, "Ошибка синтаксиса", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        string text = "Условие \"" + command.Remove(command.Length - 1) + "\" строка " + curline + ",\n не содержит необходимые аргументы!\n Добавьте их и попробуйте ещё раз.";
                        MessageBox.Show(text, "Ошибка синтаксиса", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                       
                }
            }
        }
        public static void inputCode(System.Windows.Controls.TextBox input)
        {
            //парсинг ввдённого кода
            inputcode = new Dictionary<int, string>();
            string[] s = input.Text.Split('\n');

            foreach (string i in s)
                try{
                    if (i != "\r")//отбрасываем пустые строки
                    {
                        string[] temp = i.Split(':');
                        inputcode.Add(int.Parse(temp[0]), temp[1]);
                    }
                }
                catch (FormatException){
                    MessageBox.Show("Строка: \"" + i + "\"\n" + " не содержит указатель строки \':\'", "Ошибка синтаксиса", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                catch (IndexOutOfRangeException){
                    MessageBox.Show("Строка: \"" + i + "\"\n" + " не содержит указатель строки \':\', а также комманд!", "Ошибка синтаксиса", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            nextCommand(1);
        }
        public static string deleteEmpty(string command)
        {
            //удаление лишних пробелов из программы пользователя
            List<string> BadStr = command.Split(' ').ToList<string>();
            string ConvToString = string.Empty;
            foreach (var i in BadStr)
                if (i != "") ConvToString += i + " ";
            return ConvToString;
        }
        static void nextCommand(int line)
        {
            curline = line;
            //здесь мы просто проверяем есть ли данная строка, а когда не найдём её прекратим выполнение и выдадим ошибку
            try
            {
                CommandProcessor.process(inputcode[line]);
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("Строки " + line + " не существует! Добавьте её и попробуйте ещё раз.", "Ошибка синтаксиса", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
