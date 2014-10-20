using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MHD.Properties;

namespace MHD
{
    public class Stop
    {
        string _name;
        string _url;
        string _customInfo;
        Dictionary<int, Dictionary<string, List<string>>> _departures;

        #region Constructors
        public Stop(string name, string url)
        {
            Name = name;
            Url = url;
            CustomInfo = string.Empty;
            Departures = new Dictionary<int, Dictionary<string, List<string>>>();

            Load();
        }

        public Stop(string name)
        {
            Name = name;
            Url = string.Empty;
            CustomInfo = string.Empty;
            Departures = new Dictionary<int, Dictionary<string, List<string>>>();
        }

        public Stop()
        {
            Departures = new Dictionary<int, Dictionary<string, List<string>>>();
        }

        #endregion

        #region Accessors
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string CustomInfo
        {
            get { return _customInfo; }
            set { _customInfo = value; }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public Dictionary<int, Dictionary<string, List<string>>> Departures
        {
            get { return _departures; }
            set { _departures = value; }
        }
        #endregion

        public override string ToString()
        {
            //return string.Format("        {0} - {1}", Name, Url);
            return string.Format("            {0} - {1}", Name, CustomInfo);
        }

        public void Load()
        {
            var webGet = new HtmlWeb();
            HtmlDocument document = webGet.Load(Resources.IMHD_URL + Url);

            try
            {
                var node = document.DocumentNode;
                var departureTable = node.SelectSingleNode("//div[@id='content']//table[@class='cp_obsah']");
                var dayTypes = departureTable.SelectNodes("tr[position() = 1]/td");

                if (dayTypes != null)
                {
                    var departureTables = departureTable.SelectNodes("tr[position() = 2]//td[@class='cp_odchody_tabulka']");
                    for (int i = 0; i < dayTypes.Count; i++)
                    {
                        int type = Settings.GetType(dayTypes[i].InnerHtml.Replace("&nbsp;", " "));
                        var table = new Dictionary<string, List<string>>();

                        var tableNode = departureTables[i];
                        var tableRows = tableNode.SelectNodes("table[@class='cp_odchody_tabulka_max']/tr[@class='cp_odchody']");
                        foreach (var row in tableRows)
                        {
                            var key = row.SelectSingleNode("td[position() = 1]").InnerHtml;   
                            table.Add(key, new List<string>());

                            var values = row.SelectNodes("td[(position() > 1) and not(@class='cp_odchody_doplnenie')]");
                            if (values != null)
                            {
                                foreach (var value in values)
                                {
                                    table[key].Add(value.InnerHtml);
                                }
                            }
                        }

                        Departures.Add(type, table);
                    }
                }
                else 
                {
                    var poznamky = node.SelectSingleNode("//div[@id='content']//table[@class='poznamky']/tr/td/b[position() = 2]");
                    CustomInfo = poznamky.InnerHtml;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
