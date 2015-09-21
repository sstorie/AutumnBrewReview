using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseBeers
{
    public class Beer
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ABV { get; set; }
        public string IBU { get; set; }
        public string ServingType { get; set; }

        public Brewery Brewery { get; set; }
    }
}
