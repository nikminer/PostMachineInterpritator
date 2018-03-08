using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Windows;
using Post_Machine_Intepretator.Classes;

namespace Post_Machine_Intepretator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Creat0r creator;
        Help helper;
        public MainWindow()
        {
            InitializeComponent();
            EmergencySave.init(input);
        }
        int len; //длина отрезка (нужна для отрисовки)
        int karretpos; //позиция карретки
        Dictionary<int, int> listcells=new Dictionary<int, int>(); //отрезок
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EmergencySave.loadSave();
            len = EmergencySave.len;
            karretpos = EmergencySave.karretpos;
            listcells = EmergencySave.listcells;

            
            //Отрисовка в интерпритаторе отрезка памяти
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
        private void start_Click(object sender, RoutedEventArgs e)
        {
            EmergencySave.Save(len, karretpos, listcells, input);
            CommandProcessor.karretpos = karretpos;
            CommandProcessor.listcells= new Dictionary<int, int>();
            foreach (KeyValuePair<int, int> i in listcells)
                CommandProcessor.listcells.Add(i.Key,i.Value);
            CommandProcessor.inputCode(input);
            CommandProcessor.len = len;

            //отрисовка изменений
            Graphics.drawAll(canvas, CommandProcessor.listcells, CommandProcessor.karretpos, len);
            Stauscode.Visibility = Visibility;
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            EmergencySave.deleteSave();

            //закрываем приложение при закрытие главной формы
            Environment.Exit(0);
        }
        private void help(object sender, RoutedEventArgs e)
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
        private void input_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Stauscode.Visibility = Visibility.Hidden;
        }
    }
}
