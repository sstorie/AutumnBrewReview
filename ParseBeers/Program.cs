using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParseBeers
{
    internal enum Section
    {
        Brewery,
        Beer
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            string path = @"../../../ABR_beers.txt";

            string line;
            var beers = new List<Beer>();
            var abvs = new List<ABV>();
            Brewery currentBrewery = new Brewery();
            Beer currentBeer = new Beer();
            var section = Section.Brewery;

            System.IO.StreamReader file = new System.IO.StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (Regex.IsMatch(line, @"^\d+$")) continue;
                if (Regex.IsMatch(line, @"• VISIT OUR BOOTH")) continue;


                // Match a brewery name
                var breweryMatch = Regex.Match(line, @"(.+) No (\d+)");

                if (breweryMatch.Success)
                {
                    currentBrewery = new Brewery
                    {
                        Name = breweryMatch.Groups[1].Value,
                        Number = breweryMatch.Groups[2].Value
                    };

                    section = Section.Brewery;

                    //Console.WriteLine("Found brewery - {0}", currentBrewery.Name);

                    continue;
                }

                // Look for beers
                var beerMatch = Regex.Match(line, @"• (.+)");

                if (beerMatch.Success)
                {
                    currentBeer = new Beer
                    {
                        Name = beerMatch.Groups[1].Value,
                        Brewery = currentBrewery
                    };

                    section = Section.Beer;

                    //Console.WriteLine("  Found beer - {0}", currentBeer.Name);

                    beers.Add(currentBeer);

                    continue;
                }

                // Look for ABV
                var abvMatch = Regex.Match(line, @".+\[(.*)%\].+\[(.*?)\].+\[(.*)\]");

                if (abvMatch.Success)
                {
                    // Just add it to the list so we can assign to beers 
                    //  at the end
                    abvs.Add(new ABV
                    {
                        Abv = abvMatch.Groups[1].Value,
                        Ibu = abvMatch.Groups[2].Value,
                        ServingType = abvMatch.Groups[3].Value
                    });

                    continue;
                }

                // Otherwise we've got a string of text that is part of a 
                //  description
                //
                switch (section)
                {
                    case Section.Beer:
                        currentBeer.Description += line;
                        break;
                }
            }

            // Now copy over the ABV values
            for (int i = 0; i < abvs.Count; i++)
            {
                beers[i].ABV = abvs[i].Abv;
                beers[i].IBU = abvs[i].Ibu;
                beers[i].ServingType = abvs[i].ServingType;
            }

            file.Close();

            using (var outFile = new StreamWriter(@"../../../ABR_beers_parsed.txt"))
            {
                foreach (var beer in beers)
                {
                    outFile.WriteLine("{0}|{1}|{2}|{3}", beer.Brewery.Name, beer.Brewery.Number, beer.Name, beer.Description);

                    //outFile.WriteLine("{0}|{1}|{2}|{3}|{4}|{5}|{6}", beer.Brewery.Name, beer.Brewery.Number, beer.Name, beer.ABV, beer.IBU, beer.ServingType, beer.Description);
                }
            }
        }
    }
}