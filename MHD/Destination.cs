using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace MHD
{
    public class Destination
    {
        string _name;
        List<Stop> _stops;

        public Destination(string name)
        {
            Stops = new List<Stop>();
            Name = name;
        }

        public Destination(HtmlNode node)
        {
            Stops = new List<Stop>();

            Load(node);
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public List<Stop> Stops
        {
            get { return _stops; }
            set { _stops = value; }
        }

        public void Load(HtmlNode node)
        {
            Name = node.SelectSingleNode("table/tr[position() = 1]/td").InnerHtml.Substring(2);
            //Console.WriteLine(this);

            var stopNodes = node.SelectNodes("table/tr[position() > 2]/td[position() = 1]");
            foreach (var sn in stopNodes)
            {
                var stopNode = sn.SelectSingleNode(".//a");
                Stop stop;
                if (stopNode != null)
                {
                    stop = new Stop(stopNode.InnerHtml, stopNode.Attributes["href"].Value);
                }
                else
                {
                    stopNode = sn.SelectSingleNode(".//b");
                    stop = new Stop(stopNode.InnerHtml);
                }
                Stops.Add(stop);
                //Console.WriteLine(stop.ToString());
                //break;
            }
        }

        public override string ToString()
        {
            return string.Format("        {0}", Name);
        }
    }
}
