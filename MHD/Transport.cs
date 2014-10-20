﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MHD.Properties;
using HtmlAgilityPack;
using System.Xml;

namespace MHD
{
    public class Transport
    {
        string _url;
        string _name;
        //TransportType _type;
        List<Destination> _destinations;

        public Transport() 
        {
            Destinations = new List<Destination>();
        }

        public Transport(string url, string name)
        {
            Destinations = new List<Destination>();
            Url = url;
            Name = name;
        }

        public string Url 
        {
            get { return _url; }
            set { _url = value; }
        }

        public string Name 
        {
            get { return _name; }
            set { _name = value; }
        }

        public List<Destination> Destinations
        {
            get { return _destinations; }
            set { _destinations = value; }
        }

        //public TransportType Type 
        //{
        //    get { return _type; }
        //    set { _type = value; }
        //}

        public override string ToString()
        {
            return string.Format("{0} - {1}", Name, Url);
        }

        public void Load() 
        {
            var webGet = new HtmlWeb();
            HtmlDocument document = webGet.Load(Resources.IMHD_URL + Url);

            try
            {
                var node = document.DocumentNode;
                var cells = node.SelectNodes("//div[@id='content']/div/table[position()=1]/tr/td");

                Destination d1 = new Destination(cells[0]);
                Destinations.Add(d1);

                if (cells.Count > 1)
                {
                    Destination d2 = new Destination(cells[2]);
                    Destinations.Add(d2);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Save()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(Name + ".xml", settings);

            writer.WriteStartDocument();

            writer.WriteComment("This file is generated by the program.");

            writer.WriteStartElement("transport");
            writer.WriteAttributeString("name", Name);
            writer.WriteAttributeString("url", Url);

            foreach (var d in Destinations)
            {
                writer.WriteStartElement("destination");
                writer.WriteAttributeString("name", d.Name);

                foreach (var s in d.Stops)
                {
                    writer.WriteStartElement("stop");
                    writer.WriteAttributeString("name", s.Name);
                    writer.WriteAttributeString("url", s.Url);
                    writer.WriteAttributeString("customInfo", s.CustomInfo);

                    if (s.Departures != null)
                    {
                        foreach (var pair in s.Departures)
                        {
                            writer.WriteStartElement("dayType");
                            writer.WriteAttributeString("type", pair.Key.ToString());

                            foreach (var hour in pair.Value)
                            {
                                writer.WriteStartElement("hour");
                                writer.WriteAttributeString("value", hour.Key);

                                foreach (var minute in hour.Value)
                                {
                                    writer.WriteStartElement("minute");
                                    writer.WriteAttributeString("value", minute);
                                    writer.WriteEndElement();
                                }

                                writer.WriteEndElement();
                            }

                            writer.WriteEndElement();
                        }
                    }

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }
    }
}
