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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (CheckInternet().Equals(2))
            {
                MessageBox.Show("Проверьте доступ к Интернету", "Внимание", MessageBoxButton.OKCancel);
            }
            //else
            //{
            //    MessageBox.Show("все окэй", "Внимание", MessageBoxButton.OKCancel);
            //}
            
            DateTime thisDay = DateTime.Today;
            MessageBox.Show(thisDay.ToString());
            DP.SelectedDate = thisDay;
            //DP.BlackoutDates.AddDatesInPast();
            //DP.BlackoutDates.Add(new CalendarDateRange(DateTime.Now, DateTime.Now.AddDays(8)));

            Rate data = JsonConvert.DeserializeObject<Rate>(json);
           
                LB.Items.Add(new ListBoxItem() { Content = data });
            

            int curIDStrCount = GetCurIDCount(pathToCurIDFile);
            if (curIDStrCount == 0)
            {
                MessageBox.Show("Файл, содержащий список валют, необходимых к загрузке пуст/не существует/поврежден. \rPath: " + pathToCurIDFile, "Error", MessageBoxButton.OK);
                Environment.Exit(0);
            }
            string[] curIDStrArray = new string[curIDStrCount];
            for (int i = 0; i < curIDStrCount; i++)
            {
                curIDStrArray[i] = GetCurIDList(pathToCurIDFile)[i];
                TempLB.Items.Add(curIDStrArray[i]);
            }

            string testUrl = "https://www.nbrb.by/api/exrates/rates/USD?ondate=2020-03-13&parammode=2";
            TB.Text = GetJSON(testUrl);

        }

        public string GetDataPickerDate()
        {
            string date = DP.Text;
            return 0.ToString();
        }

        static string GetJSON(string uri)
        {
            WebRequest request = WebRequest.Create(uri);
            return uri.ToString();
        }

        public string CreateApiLink(string curRateCode)
        {
            string date = GetDataPickerDate();
           // string link = "https://www.nbrb.by/api/exrates/rates/" + curRateCode.ToString() + "?ondate" +
            return 0.ToString();
        }

        public enum ConnectionStatus
        {
            NotConnected,
            LimitedAccess,
            Connected
        }

        public static ConnectionStatus CheckInternet()
        {
            // Проверить загрузку документа ncsi.txt
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://www.msftncsi.com/ncsi.txt");
            try
            {
                HttpWebResponse responce = (HttpWebResponse)request.GetResponse();

                if (responce.StatusCode != HttpStatusCode.OK)
                {
                    return ConnectionStatus.LimitedAccess;
                }
                using (StreamReader sr = new StreamReader(responce.GetResponseStream()))
                {
                    if (sr.ReadToEnd().Equals("Microsoft NCSI"))
                    {
                        return ConnectionStatus.Connected;
                    }
                    else
                    {
                        return ConnectionStatus.LimitedAccess;
                    }
                }
            }
            catch
            {
                return ConnectionStatus.NotConnected;
            }

        }


        const string pathToCurIDFile = @"D:\work\svitanak\svtnk_exchangeRates\docs\rates.list";

        public static int GetCurIDCount(string path)
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

        public static string[] GetCurIDList(string path)
        {
            int arrayLength = GetCurIDCount(pathToCurIDFile);
            string[] codeList = new string[arrayLength];

            codeList = File.ReadAllLines(path);
            return codeList;
        }

        //https://www.nbrb.by/api/exrates/rates/840?parammode=1
        //https://www.nbrb.by/api/exrates/rates/840?ondate=2020-03-13&parammode=1
        //https://www.nbrb.by/api/exrates/rates/298?ondate=2016-7-5
        //https://www.nbrb.by/api/exrates/rates/USD?ondate=2016-7-5         выгоднее юзать это, применив знание госта в файле


        public class Rate
        {
            //[Key]
            public int Cur_ID;
            public DateTime Date;
            public string Cur_Abbreviation;
            public int Cur_Scale;
            public string Cur_Name;
            public decimal? Cur_OfficialRate;

            public override string ToString()
            {
                return string.Format("{0} ({1}, {2}): {3}, Date: {4}", Cur_Name, Cur_Abbreviation, Cur_ID, Cur_OfficialRate, Date);
            }
        }

        //string json = @"";
        string json = @"{
                ""Cur_ID"": 170,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""AUD"",
                ""Cur_Scale"": 1,
                ""Cur_Name"": ""Австралийский доллар"",
                ""Cur_OfficialRate"": 1.5039}";

    };
}