using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseBeers
{
    public class Brewery
    {
        public string Name { get; set; }

        public string Number { get; set; }

        public List<Beer> Beers { get; }

        public Brewery()
        {
            this.Beers = new List<Beer>();
        }
    }
}
