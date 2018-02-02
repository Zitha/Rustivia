using System;
using System.IO;

namespace WebridgeConTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SerialPortReader sp = new SerialPortReader();

            PortReader portReader = new PortReader();

            Console.WriteLine(" Start Reading .........");
            //using (StreamWriter sw = new StreamWriter("C:\\Texts.txt", true))
            //{
            //    sw.WriteLine("Event Trrigeredsss: ");
            //}


            //sp.Open();

            portReader.Open();

            //ReadString();
            Console.ReadLine();
        }

        private static void ReadString()
        {
            var text = string.Empty;
            using (StreamReader sr = new StreamReader("C:\\PortReaderText.txt", true))
            {
                while (sr.Peek() >= 0)
                {
                    text = text + sr.ReadLine();
                }

                var a = text.Split(';');
            }
        }
    }
}
