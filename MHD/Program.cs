using System.CodeDom;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MHD.Properties;
using Newtonsoft.Json;

namespace MHD
{
    class Program
    {

        static void Main(string[] args)
        {
            var webGet = new HtmlWeb();
            HtmlDocument document = webGet.Load(Resources.IMHD_URL + Resources.IMHD_CP_URL);

            HtmlNodeCollection lines = null;
            try
            {
                var node = document.DocumentNode;
                var content = node.SelectSingleNode("//div[@id='content']/div/div[position()=2]");
                lines = content.SelectNodes("table/tr");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            List<Transport> Transports;
            if (lines != null)
            {
                Transports = new List<Transport>();
                int i = 0;
                foreach (var line in lines)
                {
                    if (lines.IndexOf(line).Equals(lines.Count - 1)) break;
                    try
                    {
                        //string typeName = line.SelectSingleNode("td[position()=1]/h2").InnerHtml.Split('&')[0];

//                        var type = new TransportType(typeName);
//                        TransportTypes.Add(type);

                        HtmlNodeCollection transportNodes = line.SelectNodes("td[position()=2]/a");

                        foreach (var transportNode in transportNodes)
                        {
                            string url = transportNode.Attributes["href"].Value;
                            string name = transportNode.InnerHtml;
                            var transport = new Transport(url, name);
//                            type.AddTransport(transport);
                            Transports.Add(transport);
                            i++;
                        }
                    }
                    catch (Exception e) 
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                try
                {
                    //int j = 1;
                    //foreach (TransportType type in TransportTypes)
                    //{
                        //Console.WriteLine(type.ToString());
                        //foreach (Transport t in type.Transports)
                        //{
                        //    Console.WriteLine("Processing line {0} of {1} ({2}%)", j, i, (int)((double)j/(double)i*100));
                        //    t.Load();
                        //    var json = JsonConvert.SerializeObject(t);
                        //    t.Save(json);
                        //    //j++;
                        //}
                    DateTime time = DateTime.Now;
                    Parallel.ForEach(Transports, new ParallelOptions() {MaxDegreeOfParallelism = 4}, (x) => ProcessTransport(x, Transports.IndexOf(x)));
                    Console.WriteLine(DateTime.Now - time);
                    Console.WriteLine("Done.");
                    //}
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                //Console.WriteLine(TransportTypes);
            }

            Console.ReadLine();
        }

        public static void ProcessTransport(Transport t, int index)
        {
            Console.WriteLine("Processing line {0}.", t.Name);
            if (t.Load())
            {
                var json = JsonConvert.SerializeObject(t);
                t.Save(json);
            }
            Console.WriteLine("Line {0} done.", t.Name);
            t = null;
            GC.Collect();
        }
    }
}
