using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Threading.Tasks;

namespace WebridgeConTest
{
    public class SerialPortReader
    {
        public long ScaleValue;
        private SerialPort _serialPort;
        private Timer tm;
        private string strCardNo = "";

        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                ScaleValue++;
                if (!_serialPort.IsOpen)
                    return;
                string str1 = _serialPort.ReadExisting();
                if (!string.IsNullOrEmpty(str1))
                {

                    //string str2 = _strCardNo.Substring(1, 6);
                    if (str1.Length < Convert.ToInt32(ConfigurationManager.AppSettings["stringLeng"]))
                    {
                        return;
                    }
                    using (StreamWriter sw = new StreamWriter("C:\\Texts.txt", true))
                    {
                        sw.WriteLine(str1 + "  " + DateTime.Now);
                    }
                    //string[] aa = str1.Split(',');

                    //string strimValue = aa[0].Trim();
                    //if (strimValue.Length >= 5 && Regex.IsMatch(strimValue, @"^\d+$"))
                    //{
                    //    using (StreamWriter sw = new StreamWriter("C:\\Texts.txt", true))
                    //    {
                    //        sw.WriteLine(strimValue + "  " + DateTime.Now + " : A0");
                    //    }
                    //}
                    //else if (aa.Length > 1)
                    //{
                    //    if (aa[1].Trim().Length >= 5 && Regex.IsMatch(aa[1], @"^\d+$"))
                    //    {
                    //        using (StreamWriter sw = new StreamWriter("C:\\Texts.txt", true))
                    //        {
                    //            sw.WriteLine(aa[1] + "  " + DateTime.Now + " : A1");
                    //        }
                    //    }
                    //}
                    //Console.WriteLine(str1);
                    //Console.ReadLine();
                    //int num1 = Convert.ToInt32(str2);
                    //ScaleValue = num1;
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
                Console.WriteLine("Nigga is an error here " + ex.Message);
            }
        }

        public bool Open()
        {

            try
            {

                if (_serialPort == null)
                {
                    Parity printy = Parity.Even;
                    StopBits stopBit = StopBits.One;
                    if (ConfigurationManager.AppSettings["parity"] == "0")
                    {
                        printy = Parity.None;
                    }
                    else if (ConfigurationManager.AppSettings["parity"] == "1")
                    {
                        printy = Parity.Odd;
                    }
                    else if (ConfigurationManager.AppSettings["parity"] == "2")
                    {
                        printy = Parity.Even;
                    }
                    //--------------------------
                    if (ConfigurationManager.AppSettings["stopBits"] == "0")
                    {
                        stopBit = StopBits.None;
                    }
                    if (ConfigurationManager.AppSettings["stopBits"] == "1")
                    {
                        stopBit = StopBits.One;
                    }
                    else if (ConfigurationManager.AppSettings["stopBits"] == "1.5")
                    {
                        stopBit = StopBits.OnePointFive;
                    }
                    else if (ConfigurationManager.AppSettings["stopBits"] == "2")
                    {
                        stopBit = StopBits.Two;
                    }
                    _serialPort = new SerialPort(ConfigurationManager.AppSettings["SerialPort"],
                        Convert.ToInt32(ConfigurationManager.AppSettings["bitsPerSec"]),
                        printy, Convert.ToInt32(ConfigurationManager.AppSettings["dataBits"]), stopBit)
                    {
                        WriteBufferSize = 256,
                        DtrEnable = true,
                        RtsEnable = true
                    };
                    _serialPort.Open();
                    _serialPort.BreakState = true;
                    _serialPort.BreakState = false;
                    _serialPort.DataReceived += Sp_DataReceived;
                    tm = new Timer(600);
                    tm.Start();
                    tm.Elapsed += new ElapsedEventHandler(this.tm_Elapsed);
                }
                else
                {
                    _serialPort.Close();
                }

                return true;
            }
            catch (IOException exception)
            {
                Console.WriteLine("Nigga is an error here " + exception.Message);
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

        private void tm_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                this.tm.Stop();
                try
                {
                    if (!this._serialPort.IsOpen)
                        return;
                    string str1 = this._serialPort.ReadExisting();

                    if (!string.IsNullOrEmpty(str1))
                    {

                        if (str1.Length < Convert.ToInt32(ConfigurationManager.AppSettings["stringLeng"]))
                        {
                            return;
                        }
                        using (StreamWriter sw = new StreamWriter("C:\\Texts.txt", true))
                        {
                            sw.WriteLine(str1 + "  " + DateTime.Now);
                        }
                        //string[] aa = str1.Split(',');

                        //string strimValue = aa[0].Trim();
                        //if (strimValue.Length >= 5 && Regex.IsMatch(strimValue, @"^\d+$"))
                        //{

                        //    using (StreamWriter sw = new StreamWriter("C:\\Texts.txt", true))
                        //    {
                        //        sw.WriteLine(strimValue + "  " + DateTime.Now + " : A0");
                        //    }
                        //}
                        //else if (aa.Length > 1)
                        //{
                        //    if (aa[1].Trim().Length >= 5 && Regex.IsMatch(aa[1], @"^\d+$"))
                        //    {
                        //        using (StreamWriter sw = new StreamWriter("C:\\Texts.txt", true))
                        //        {
                        //            sw.WriteLine(aa[1] + "  " + DateTime.Now + " : A1");
                        //        }
                        //    }
                        //}
                        //Console.WriteLine(str1);
                        //this.strCardNo = str1;
                        //if (this.strCardNo.Length < 15)
                        //    return;
                        //string cardNo = this.strCardNo.Substring(1, 10);
                        //int int32_1 = Convert.ToInt32(this.strCardNo.Substring(11, 1));
                        //int num = 0;
                        //for (int startIndex = 1; startIndex < 11; ++startIndex)
                        //{
                        //    string str2 = this.strCardNo.Substring(startIndex, 1);
                        //    num += (int)str2[0];
                        //}
                        //int int32_2 = Convert.ToInt32(num.ToString().Substring(num.ToString().Length - 1, 1));
                        //using (StreamWriter sw = new StreamWriter("C:\\Texts.txt", true))
                        //{
                        //    sw.WriteLine(int32_2);
                        //}
                        //if (int32_1 != int32_2)
                        //    return;
                    }
                    else
                    {
                        this._serialPort.BreakState = true;
                        this._serialPort.BreakState = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Nigga is an error here " + ex.Message);
                }
            }
            finally
            {
                this.tm.Start();
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
}
