using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHD
{
    public class TransportType
    {

        string _name;
        List<Transport> _transports;

        public TransportType() 
        {
            Transports = new List<Transport>();
        }

        public TransportType(string name)
        {
            Transports = new List<Transport>();
            Name = name;
        }

        public void AddTransport(Transport t)
        {
            Transports.Add(t);
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public List<Transport> Transports
        {
            get { return _transports; }
            set { _transports = value; }
        }

        public override string ToString() 
        {
            return Name;
        }
    }
}
