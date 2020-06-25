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

            int strRatesFileCount = GetRatesCount(pathToRatesListFile);
            string[] curIDStrArray = new string[strRatesFileCount];
            for (int i = 0; i < strRatesFileCount; i++)
            {
                curIDStrArray[i] = GetRatesList(pathToRatesListFile)[i];
                SetRatesLB.Items.Add(curIDStrArray[i]);
            }
        }

        const string pathToRatesListFile = @"D:\work\svtnk_exchangeRates\docs\rates.list";

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
                    }
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
            int arrayLength = GetRatesCount(pathToRatesListFile);
            string[] codeList = new string[arrayLength];

            codeList = File.ReadAllLines(path);
            return codeList;
        }

        private void SaveCurListBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddLineBtn_Click(object sender, RoutedEventArgs e)
        {
            SetRatesLB.Items.Insert(1, "Введите код валюты");
        }

        private void DeleteLineBtn_Click(object sender, RoutedEventArgs e)
        {
            SetRatesLB.Items.RemoveAt(SetRatesLB.SelectedIndex);
        }
    }
}
