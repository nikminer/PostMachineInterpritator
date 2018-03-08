using System.Collections.Generic;
using Microsoft.Win32;
namespace Post_Machine_Intepretator.Classes
{
    //Этот модуль отвечает за экстернное сохранение данных. Идея заключается в следующем, автоматически сохранять код и отрезок пользователя,
    //если вдруг пользователь напишет бесконечный цикл и программа вылетит. А если пользователь сам закроет программу, то данные должны быть удалены.
    static class EmergencySave
    {
        public static int len, karretpos;
        public static Dictionary<int, int> listcells = new Dictionary<int, int>();
        

        static public void init(System.Windows.Controls.TextBox input)
        {
            RegistryKey soft = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("PostMachineInterpritator", true);
            //проверяем есть ли записи о сохранениях и есть ли хоть что-то!
            try
            {
                input.Text = soft.GetValue("emergencySave").ToString();
            }
            catch
            {
                //если нет ничего то создаём всё заново
                Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("PostMachineInterpritator", true);
            }
        }
        static public void loadSave()
        {
            RegistryKey soft = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("PostMachineInterpritator", true);
            soft.CreateSubKey("Arr", true).Close();
            //загружаем всё что находится в реестре, если ничего там нет, то загружаем значения по умолчанию
            len = (int)soft.GetValue("len", 5);
            karretpos = (int)soft.GetValue("karretpos", 0);
            for (int i = 0; i < len; i++)
                listcells.Add(i, int.Parse(soft.OpenSubKey("Arr", true).GetValue(i.ToString(), 0).ToString()));
        }
        static public void Save(int len, int karretpos, Dictionary<int, int> listcells, System.Windows.Controls.TextBox input)
        {
            RegistryKey soft = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("PostMachineInterpritator", true);
            //Сохраняем все занчения на случай если вдруг вылетит программа, из-за бесконечного цикла!
            soft.SetValue("emergencySave", input.Text);

            soft.CreateSubKey("Arr");
            foreach (KeyValuePair<int, int> i in listcells)
                soft.OpenSubKey("Arr", true).SetValue(i.Key.ToString(), i.Value.ToString()); ;

            soft.SetValue("len", len);

            soft.SetValue("karretpos", karretpos);
            soft.Close();
        }
        static public void deleteSave()
        {
            RegistryKey soft = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("PostMachineInterpritator", true);
            //удаление экстренного сохранения если пользователь закрыл программу
            try
            {
                soft.DeleteValue("len");
                soft.DeleteValue("karretpos");
                soft.DeleteValue("emergencySave");
                soft.DeleteSubKey("Arr",false);
            }
            catch { }
            soft.Close();
        }
    }
}
