using System;
using System.Configuration;
using System.IO;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;

namespace WeighBridgeApplication.Util
{
    public class SerialPortReader
    {
        public long ScaleValue;
        private SerialPort _serialPort;
        private string strCardNo = "";
        public event EventHandler<SerialportEventArgs> ScaleValueEvent;

        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (!_serialPort.IsOpen)
                    return;
                string text = _serialPort.ReadExisting();

                if (!string.IsNullOrEmpty(text))
                {
                    strCardNo = strCardNo + text;

                    if (strCardNo.Length > 15 && strCardNo.Contains("|;"))
                    {
                        var split = strCardNo.Split(';');
                        string output = split[0];
                        if (output.Contains("S,"))
                        {
                            string strimValue = output.Substring(2, 6);

                            if (strimValue.IsNumeric())
                            {
                                //using (StreamWriter sw = new StreamWriter("C:\\WBText.txt", true))
                                //{
                                //    sw.WriteLine(strimValue);
                                //}
                                int num1 = Convert.ToInt32(strimValue);
                                ScaleValue = num1;
                                ScaleValueEvent(sender, new SerialportEventArgs { SerialPortNumber = ScaleValue });
                            }
                        }
                        strCardNo = "";
                        //Thread.Sleep(2000);
                    }
                }
                else
                {
                    _serialPort.BreakState = true;
                    _serialPort.BreakState = false;
                }
            }
            catch (Exception ex)
            {
                _serialPort.Close();
            }
        }

        public bool Open()
        {
            try
            {
                if (_serialPort == null)
                {
                    _serialPort = new SerialPort(ConfigurationManager.AppSettings["SerialPort"], 19200, Parity.None, 8, StopBits.One)
                    {
                        WriteBufferSize = 256,
                        DtrEnable = true,
                        RtsEnable = true
                    };
                    _serialPort.Open();
                    _serialPort.BreakState = true;
                    _serialPort.BreakState = false;
                    _serialPort.DataReceived += Sp_DataReceived;
                }

                return true;
            }
            catch (IOException exception)
            {
                //Exception ex = exception;
                //var exmgr = EnterpriseLibraryContainer.Current.GetInstance<ExceptionManager>();

                //exmgr.HandleException(exception, "WeighBridgeException", out ex);
                return false;
            }
        }

        public bool Close()
        {
            try
            {
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    _serialPort.Close();
                    _serialPort = null;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Release()
        {
            try
            {
                _serialPort = null;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public class SerialportEventArgs
    {
        public long SerialPortNumber { get; set; }
    }


    public static class StringExtensions
    {
        public static bool IsNumeric(this string input)
        {
            int number;
            return int.TryParse(input, out number);
        }
    }
}