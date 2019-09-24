using MMK.SmartSystem.CNC.Host.CNC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var cncHandler = new CncHandler();
            cncHandler.ShowErrorLogEvent += CncHandler_ShowErrorLogEvent; ;
            cncHandler.GetResultEvent += CncHandler_GetResultEvent;
            cncHandler.Connect();
            CNCdata();
            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                try
                {
                    cncHandler.Execute();

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);

                }
            }
         //   Console.ReadLine();

        }

        private static void CncHandler_GetResultEvent(object obj)
        {
           
            Console.WriteLine(obj.ToString());
        }

        private static void CncHandler_ShowErrorLogEvent(string obj)
        {
            Console.WriteLine(obj);
        }

        static void CNCdata()
        {

            string info = System.IO.File.ReadAllText("d:\\json.txt");
            List<CncEventData> cncEvents = new List<CncEventData>();
            try
            {
                cncEvents = JsonConvert.DeserializeObject<List<CncEventData>>(info);
                foreach (var item in cncEvents)
                {
                    CncHandler.m_EventDatas.Add(item);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
