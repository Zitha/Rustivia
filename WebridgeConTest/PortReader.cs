using System;
using System.Configuration;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Timers;

namespace WebridgeConTest
{
    public class PortReader
    {
        private SerialPort SerialPort1;
        private string strCardNo = "";


        private void _Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Time Elappesed and shit");
        }


        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.ReceivedText(this.SerialPort1.ReadExisting());
        }

        private void ReceivedText(string text)
        {
            try
            {
                //var nextstring = strCardNo + text;

                //string[] splitArray = nextstring.Split(';');
                strCardNo = strCardNo + text;

                if (strCardNo.Length > 15 && strCardNo.Contains("|;"))
                {
                    using (StreamWriter sw = new StreamWriter("C:\\PortReaderText.txt", true))
                    {
                        var split = strCardNo.Split(';');
                        string output = split[0];
                        if (output.Contains("S,"))
                        {
                            sw.WriteLine(output.Substring(2, 6));
                        }

                    }
                    strCardNo = "";
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Open()
        {
            try
            {
                SerialPort1 = new SerialPort()
                {
                    PortName = "COM4",
                    Parity = Parity.None,
                    BaudRate = 19200,
                    StopBits = StopBits.One,
                    DataBits = 8
                };
                SerialDataReceivedEventHandler receivedEventHandler = new SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);
                SerialPort1.DataReceived += receivedEventHandler;
                SerialPort1.Open();

                Console.WriteLine("Shit Opened");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
