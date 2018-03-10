using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Post_Machine_Intepretator
{
    /// <summary>
    /// Логика взаимодействия для Setting.xaml
    /// </summary>
    public partial class Setting : Window
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AV.Text = Syntax.Default.AddV;
            RV.Text = Syntax.Default.RemV;
            L.Text = Syntax.Default.left;
            R.Text = Syntax.Default.right;
            Ifer.Text = Syntax.Default.ifer;
            Point.Text = Syntax.Default.ifer_split.ToString();
        }

        private void Save_and_exit_Click(object sender, RoutedEventArgs e)
        {
            if (check_setting())
            {
                Syntax.Default.AddV = AV.Text;
                Syntax.Default.RemV = RV.Text;
                Syntax.Default.left = L.Text;
                Syntax.Default.right = R.Text;
                Syntax.Default.ifer = Ifer.Text;
                Syntax.Default.ifer_split = Point.Text[0];
                Syntax.Default.Save();
                this.Close();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (check_setting())
            {
                Syntax.Default.AddV = AV.Text;
                Syntax.Default.RemV = RV.Text;
                Syntax.Default.left = L.Text;
                Syntax.Default.right = R.Text;
                Syntax.Default.ifer = Ifer.Text;
                Syntax.Default.ifer_split = Point.Text[0];
                Syntax.Default.Save();
            }
        }

        bool check_setting()
        {
            List<string> str = new List<string> { AV.Text, RV.Text, L.Text, R.Text, Ifer.Text, Point.Text };
            int count = 0;
            try           
            {             
                if (Syntax.Default.ponter_line_split == Point.Text[0]) throw new Exception("Не может быть разделитель условия быть равным \":\"");
                foreach (string i in str)
                {
                    if(i=="") throw new Exception("Вы оставили поле пустым");
                    foreach (string e in str) if (i == e) count++;
                }
                    
                if (count > 6) throw new Exception("Синтаксис повторяется друг с другом");
            }             
            catch(Exception e) {
                MessageBox.Show(e.Message,"Setting ERROR",MessageBoxButton.OK,MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            Syntax.Default.AddV = "v";
            Syntax.Default.RemV = "x";
            Syntax.Default.left = "<-";
            Syntax.Default.right = "->";
            Syntax.Default.ifer = "?";
            Syntax.Default.ifer_split = ';';
            Syntax.Default.Save();
            AV.Text = Syntax.Default.AddV;
            RV.Text = Syntax.Default.RemV;
            L.Text = Syntax.Default.left;
            R.Text = Syntax.Default.right;
            Ifer.Text = Syntax.Default.ifer;
            Point.Text = Syntax.Default.ifer_split.ToString();
        }
    }
}
