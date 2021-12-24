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
using System.Diagnostics;
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
    public class Log
    {
        const string pathToLogFile = @"J:\NSIG\rates\log.txt";

        //J:\NSIG\rates
        //D:\work\svtnk_exchangeRates\docs

        public Log(bool error, string _operationText, bool notEmptyString)
        {
            WriteLogBlock(CreateLogBlock(error, _operationText, notEmptyString));
        }

        public DateTime GetDateTimeNow()
        {
            return DateTime.Now;
        }

        public string GetComputerName()
        {
            return Environment.MachineName;
        }

        public string CreateLogBlock(bool error, string operationText, bool notEmptyString)
        {
            //block;
            if (notEmptyString)
            {
                return string.Format(operationText);
            }
            else
            {
                if (error)
                {
                    return string.Format("Ошибка: {0}, user: {1} - time: {2}", operationText, GetComputerName(), GetDateTimeNow());
                }
                else
                {
                    return string.Format("Операция: {0}, user: {1} - time: {2}", operationText, GetComputerName(), GetDateTimeNow());

                }
            }
        }

        public void WriteLogBlock(string _message)
        {

            try
            {
                if (File.Exists(pathToLogFile))
                {
                    StreamWriter writer = new StreamWriter(pathToLogFile, true);
                    writer.WriteLine(_message);
                    writer.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка при подключении к файлу" + pathToLogFile.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (IOException e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Log lolog = new Log(true, e.ToString(), false);
            }
            
        }
        
    }
}
