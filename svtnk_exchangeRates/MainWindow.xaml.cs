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

            Data data = JsonConvert.DeserializeObject<Data>(json);
            foreach (Item item in data.Rate.Items)
            {
                LB.Items.Add(new ListBoxItem() { Content = item });
            }

            int curIDStrCount = GetCurIDCount(pathToCurIDFile);
            if (curIDStrCount == 0)
            {
                MessageBox.Show("Введите значения в файл" + pathToCurIDFile);
            }

            string[] curIDStrList = GetCurIDList(pathToCurIDFile);        
            for (int i = 0; i <= curIDStrCount; i++)
            {
                TempLB.Items.Add(curIDStrList[i]);
            }        

            if (curIDStrCount == 0)
            {
                MessageBox.Show("Введите значения в файл" + pathToCurIDFile);
            }
        }

        const string pathToCurIDFile = @"J:\NSIG\rates\rates.config";

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
                    Console.WriteLine("Line counts:" + i.ToString());
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
    
            //using (StreamReader sr = new StreamReader(path))
            //{
            //    var list = new List<string>();
            //    while (!sr.EndOfStream)
            //    {
            //        string line = sr.ReadLine();
            //        list.Add(line);
            //        Console.WriteLine(list[list.Count - 1]);
            //    }

            //    //Массив string[]              
            //    var arrTheoria = list.ToArray();

            //    for (int i = 1; i <= 6; i++)
            //    {
            //        codeList[i] = arrTheoria[i];
            //    }
            //}

            return codeList;
        }

        //https://www.nbrb.by/api/exrates/rates/840?parammode=1
        //https://www.nbrb.by/api/exrates/rates/840?ondate=2020-03-13&parammode=1
        //https://www.nbrb.by/api/exrates/rates/298?ondate=2016-7-5


        class Data
        {
            public Rate Rate;
        }

        class Rate
        {
            public Item[] Items;
        }

        class Item
        {
            public int Cur_ID;
            public DateTime Date;
            public string Cur_Abbreviation;
            public int Cur_Scale;
            public string Cur_Name;
            public decimal? Cur_OfficialRate;

            public override string ToString()
            {
                return string.Format("{0}: {1}, {2}", Cur_Abbreviation, Date, Cur_OfficialRate);
            }
        }

        //string json = @"";
        string json = @"{
          ""rate"": {
            ""items"": [
              {
                ""Cur_ID"": 170,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""AUD"",
                ""Cur_Scale"": 1,
                ""Cur_Name"": ""Австралийский доллар"",
                ""Cur_OfficialRate"": 1.5039
              },
              {
                ""Cur_ID"": 191,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""BGN"",
                ""Cur_Scale"": 1,
                ""Cur_Name"": ""Болгарский лев"",
                ""Cur_OfficialRate"": 1.1420
              },
              {
                ""Cur_ID"": 290,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""UAH"",
                ""Cur_Scale"": 100,
                ""Cur_Name"": ""Гривен"",
                ""Cur_OfficialRate"": 8.0627
              },
              {
                ""Cur_ID"": 291,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""DKK"",
                ""Cur_Scale"": 10,
                ""Cur_Name"": ""Датских крон"",
                ""Cur_OfficialRate"": 3.0022
              },
              {
                ""Cur_ID"": 145,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""USD"",
                ""Cur_Scale"": 1,
                ""Cur_Name"": ""Доллар США"",
                ""Cur_OfficialRate"": 2.0048
              },
              {
                ""Cur_ID"": 292,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""EUR"",
                ""Cur_Scale"": 1,
                ""Cur_Name"": ""Евро"",
                ""Cur_OfficialRate"": 2.2354
              },
              {
                ""Cur_ID"": 293,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""PLN"",
                ""Cur_Scale"": 10,
                ""Cur_Name"": ""Злотых"",
                ""Cur_OfficialRate"": 5.0219
              },
              {
                ""Cur_ID"": 303,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""IRR"",
                ""Cur_Scale"": 100000,
                ""Cur_Name"": ""Иранских риалов"",
                ""Cur_OfficialRate"": 6.5540
              },
              {
                ""Cur_ID"": 294,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""ISK"",
                ""Cur_Scale"": 100,
                ""Cur_Name"": ""Исландских крон"",
                ""Cur_OfficialRate"": 1.6351
              },
              {
                ""Cur_ID"": 295,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""JPY"",
                ""Cur_Scale"": 100,
                ""Cur_Name"": ""Йен"",
                ""Cur_OfficialRate"": 1.9704
              },
              {
                ""Cur_ID"": 23,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""CAD"",
                ""Cur_Scale"": 1,
                ""Cur_Name"": ""Канадский доллар"",
                ""Cur_OfficialRate"": 1.5544
              },
              {
                ""Cur_ID"": 304,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""CNY"",
                ""Cur_Scale"": 10,
                ""Cur_Name"": ""Китайских юаней"",
                ""Cur_OfficialRate"": 3.0051
              },
              {
                ""Cur_ID"": 72,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""KWD"",
                ""Cur_Scale"": 1,
                ""Cur_Name"": ""Кувейтский динар"",
                ""Cur_OfficialRate"": 6.6482
              },
              {
                ""Cur_ID"": 296,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""MDL"",
                ""Cur_Scale"": 10,
                ""Cur_Name"": ""Молдавских леев"",
                ""Cur_OfficialRate"": 1.0119
              },
              {
                ""Cur_ID"": 286,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""NZD"",
                ""Cur_Scale"": 1,
                ""Cur_Name"": ""Новозеландский доллар"",
                ""Cur_OfficialRate"": 1.4445
              },
              {
                ""Cur_ID"": 297,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""NOK"",
                ""Cur_Scale"": 10,
                ""Cur_Name"": ""Норвежских крон"",
                ""Cur_OfficialRate"": 2.4049
              },
              {
                ""Cur_ID"": 298,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""RUB"",
                ""Cur_Scale"": 100,
                ""Cur_Name"": ""Российских рублей"",
                ""Cur_OfficialRate"": 3.1208
              },
              {
                ""Cur_ID"": 299,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""XDR"",
                ""Cur_Scale"": 1,
                ""Cur_Name"": ""СДР(Специальные права заимствования)"",
                ""Cur_OfficialRate"": 2.7986
              },
              {
                ""Cur_ID"": 119,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""SGD"",
                ""Cur_Scale"": 1,
                ""Cur_Name"": ""Сингапурcкий доллар"",
                ""Cur_OfficialRate"": 1.4855
              },
              {
                ""Cur_ID"": 300,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""KGS"",
                ""Cur_Scale"": 100,
                ""Cur_Name"": ""Сомов"",
                ""Cur_OfficialRate"": 2.9738
              },
              {
                ""Cur_ID"": 301,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""KZT"",
                ""Cur_Scale"": 1000,
                ""Cur_Name"": ""Тенге"",
                ""Cur_OfficialRate"": 5.9490
              },
              {
                ""Cur_ID"": 302,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""TRY"",
                ""Cur_Scale"": 10,
                ""Cur_Name"": ""Турецких лир"",
                ""Cur_OfficialRate"": 6.8747
              },
              {
                ""Cur_ID"": 143,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""GBP"",
                ""Cur_Scale"": 1,
                ""Cur_Name"": ""Фунт стерлингов"",
                ""Cur_OfficialRate"": 2.6340
              },
              {
                ""Cur_ID"": 305,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""CZK"",
                ""Cur_Scale"": 100,
                ""Cur_Name"": ""Чешских крон"",
                ""Cur_OfficialRate"": 8.2533
              },
              {
                ""Cur_ID"": 306,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""SEK"",
                ""Cur_Scale"": 10,
                ""Cur_Name"": ""Шведских крон"",
                ""Cur_OfficialRate"": 2.3705
              },
              {
                ""Cur_ID"": 130,
                ""Date"": ""2016-07-06T00:00:00"",
                ""Cur_Abbreviation"": ""CHF"",
                ""Cur_Scale"": 1,
                ""Cur_Name"": ""Швейцарский франк"",
                ""Cur_OfficialRate"": 2.0641
              }
            ]
          }
        }";
    };
}