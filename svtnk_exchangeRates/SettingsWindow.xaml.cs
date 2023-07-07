using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace svtnk_exchangeRates
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            string pathToRatesListFile = GetPathToRatesList();
            int strRatesFileCount = GetRatesCount(pathToRatesListFile);
            Console.WriteLine("strRatesFileCount" + strRatesFileCount);

            string[] curIDStrArray = new string[strRatesFileCount];
            for (int i = 0; i < strRatesFileCount; i++)
            {
                curIDStrArray[i] = GetRatesList(pathToRatesListFile)[i];
                SetRatesLB.Items.Add(curIDStrArray[i]);
            }
        }

        public string GetPathToRatesList()
        {
            return @PathToRatesListTB.Text.ToString();
        }

        private static int GetRatesCount(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    String line;
                    int i = 0;
                    while ((line = sr.ReadLine()) != null) //читаем по одной строке пока не вычитаем все из потока
                    {
                        i++;
                        Console.WriteLine(line);
                    }
                    Console.WriteLine("Line counts: " + i.ToString());
                    return i;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        public static string[] GetRatesList(string path)
        {
            //string pathToRatesListFile = PathToRatesListTB.Text.ToString();
            int arrayLength = GetRatesCount(path);
            string[] codeList = new string[arrayLength];

            codeList = File.ReadAllLines(path);
            return codeList;
        }

        private void SaveCurListBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] newRatesArray = new string[SetRatesLB.Items.Count-1];
            for (int i = 0; i<= SetRatesLB.Items.Count-1; i++)
            {
                newRatesArray[i] = SetRatesLB.Items[i].ToString();
            }
            System.IO.File.WriteAllLines(GetPathToRatesList(), newRatesArray.Select(i => i.ToString()).ToArray());  
        }

        private void AddLineBtn_Click(object sender, RoutedEventArgs e)
        {
            SetRatesLB.Items.Insert(1, "Введите код валюты");
            _ = new Log(false, "В перечень загружаемых курсов добавлен следующий элемент: " + SetRatesLB.SelectedItem.ToString(), true);
        }

        private void DeleteLineBtn_Click(object sender, RoutedEventArgs e)
        {
            _ = new Log(false, "Из перечня загружаемых курсов удален следующий элемент: " + SetRatesLB.SelectedItem.ToString(), true);
            SetRatesLB.Items.RemoveAt(SetRatesLB.SelectedIndex);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SecondCHB.IsEnabled = false;
        }
    }
}
