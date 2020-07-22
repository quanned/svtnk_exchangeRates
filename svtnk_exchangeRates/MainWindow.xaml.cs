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


            #region createConst
            const string pathToCurIDFile = @"J:\NSIG\rates\rates.list";
            //J:\NSIG\rates
            //D:\work\svtnk_exchangeRates\docs
            #endregion

            #region checkInternetConnection
            if (CheckInterneConection() == ConnectionStatus.NotConnected)
            {
                MessageBox.Show("Проверьте доступ к Интернету", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                StatusBar.Items.Add("Connection status: " + ConnectionStatus.NotConnected.ToString());
            }
            else if (CheckInterneConection() == ConnectionStatus.LimitedAccess)
            {
                MessageBox.Show("Проверьте доступ к Интернету", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                StatusBar.Items.Add("Connection status: " + ConnectionStatus.LimitedAccess.ToString());
            }
            else StatusBar.Items.Add("Connection status: " + ConnectionStatus.Connected.ToString());
            #endregion

            DateTime thisDay = DateTime.Today;
            //Console.WriteLine("date: {0:yyyy-MM-dd}", thisDay);
            DP.SelectedDate = thisDay;
            DP.SelectedDateFormat = DatePickerFormat.Short;

            //DP.BlackoutDates.AddDatesInPast();
            //DP.BlackoutDates.Add(new CalendarDateRange(DateTime.Now, DateTime.Now.AddDays(8)));

            Rate data = JsonConvert.DeserializeObject<Rate>(testJson);
            LB.Items.Add(new ListBoxItem() { Content = data });


            string testUrl = @"https://www.nbrb.by/api/exrates/rates/USD?ondate=2020-03-13&parammode=2";
            GetJSON(testUrl);
            //MessageBox.Show(GetFormatDataPickerDate());
            //MessageBox.Show(CreateRequestLink("USD"));

            int curIDStrCount = GetCurIDCount(pathToCurIDFile);
            if (curIDStrCount == 0)
            {
                MessageBox.Show("Файл, содержащий список валют, востребованных для загрузки пуст/не существует/поврежден. \rPath: " + pathToCurIDFile, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
            string[] curIDStrArray = new string[curIDStrCount];
            for (int i = 0; i < curIDStrCount; i++)
            {
                curIDStrArray[i] = GetCurIDList(pathToCurIDFile)[i];
                TempLB.Items.Add(curIDStrArray[i]);
            }

            //TB.Text = GetJSON(testUrl);

        }


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

        private void GetJSON(string url)
        //async void GetJSON(string uri)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WebRequest httpWebRequest = WebRequest.Create(url);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "GET"; //Можно GET

            WebResponse httpResponse = httpWebRequest.GetResponse();
            using(Stream stream = httpResponse.GetResponseStream())
            {
                using(StreamReader streamReader = new StreamReader(stream))
                {
                    TB.Text = streamReader.ReadToEnd();
                }
            }
            //HttpClient httpClient = new HttpClient();
            //HttpResponseMessage response = new HttpRequestMessage(HttpMethod.Get, request);
            // or = new HttpRequestMessage(HttpMethod.Get, request);
            // (await httpClient.GetAsync(request)).EnsureSuccessStatusCode();




            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    //ответ от сервера
            //    var result = streamReader.ReadToEnd();
                
            //    //Сериализация
            //    Dictionary<string, List<Rate>> JsonObject = JsonConvert.DeserializeObject<Dictionary<string, List<Rate>>>(httpResponse.ToString());
            //    List<string> Json_Array = JsonConvert.DeserializeObject<List<string>>(httpResponse.ToString());
            //}

            ////Dictionary < string, List<Rate>> JsonObject = JsonConvert.DeserializeObject<Dictionary<string, List<Rate>>>(response);
            ////List<string> Json_Array = JsonConvert.DeserializeObject<List<string>>(response);
            //for (int i = 0; i < Json_Array.Count; i++)
            //{
            //    TempLB.Items.Add(Json_Array.ElementAt(i));
            //}

            //или так
            //var client = new HttpClient(); // Add: using System.Net.Http;
            //var response = await client.GetAsync(new Uri("link"));
            //var result = await response.Content.ReadAsStringAsync();

        }

        public string GetFormatDataPickerDate()
        {
            DateTime datePickerSelectedDate = DP.DisplayDate;
            //Console.WriteLine("Format date: {0:yyyy-MM-dd}", date);       
            int year = datePickerSelectedDate.Year;
            int month = datePickerSelectedDate.Month;
            int day = datePickerSelectedDate.Day;

            string formatdate = string.Format("{0}-{1}-{2}", year, month, day);
            return formatdate;
        }

        public int GetParammode()
        {
            return 2;
        }

        public string CreateRequestLink(string curRateCode)
        {
            string ratesID = curRateCode.ToString();
            string date = GetFormatDataPickerDate().ToString();
            string parammode = GetParammode().ToString();
            string link;

            return link = string.Format("https://www.nbrb.by/api/exrates/rates/{0}?ondate={1}&parammode={2}", ratesID, date, parammode);          
        }

        #region ckeckInternetConnectionStatus
        public enum ConnectionStatus
        {
            NotConnected,
            LimitedAccess,
            Connected
        }

        //public static string CheckInterneConection()
        public static ConnectionStatus CheckInterneConection()
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
        #endregion


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
            int arrayLength = GetCurIDCount(path);
            string[] codeList = new string[arrayLength];

            codeList = File.ReadAllLines(path);
            return codeList;
        }

        //https://www.nbrb.by/api/exrates/rates/840?parammode=1
        //https://www.nbrb.by/api/exrates/rates/840?ondate=2020-03-13&parammode=1
        //https://www.nbrb.by/api/exrates/rates/298?ondate=2016-7-5
        //https://www.nbrb.by/api/exrates/rates/USD?ondate=2016-7-5         выгоднее юзать это, так получается быстрее




        //string json = "https://www.nbrb.by/api/exrates/rates/USD?ondate=2020-06-26&parammode=2";
        string testJson = @"{
                ""Cur_ID"": 170,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""AUD"",
                ""Cur_Scale"": 1,
                ""Cur_Name"": ""Австралийский доллар"",
                ""Cur_OfficialRate"": 1.5039}";

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow SW = new SettingsWindow();
            SW.Owner = this;
            SW.Show();
        }
    };
}