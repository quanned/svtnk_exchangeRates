using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace svtnk_exchangeRates
{
    class Log
    {
        const string pathToLogFile = @"J:\NSIG\rates\log.txt";
        //J:\NSIG\rates
        //D:\work\svtnk_exchangeRates\docs

        public DateTime GetDateTimeNow()
        { 
            return DateTime.Now;
        }

        public string GetComputerName()
        {
            return Environment.MachineName;
        }

        public enum OperationType
        {
            LogIn,
            DatRaetrieval,
            DataRecording,
            LogOut,
            Error
        }

        public string CreateLogBlock()
        {
            string block = string.Format("Операция: {0}, {1} - {2}", 0, GetComputerName(), GetDateTimeNow());
            return block;
        }
    }
}
