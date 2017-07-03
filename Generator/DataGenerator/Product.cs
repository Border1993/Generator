using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.DataGenerator
{
    class Product
    {
        public void GenerateRandom(int index)
        {
            nazwaSkladnika = "SKLADNIK_" + Tools.RandomString(5, 10);
            nrDostawcy = Tools.random.Next(DataGenerator.numberOfCompanies);
            skladnikID = index;

            int unitType = Tools.random.Next(0, 3);
            switch (unitType)
            {
                case 0:
                    jednostka = "gr";
                    break;

                case 1:
                    jednostka = "szt";
                    break;

                case 2:
                    jednostka = "ml";
                    break;
            }
        }
        
        public void RandomizeSpareComponent(List<Product> products)
        {
            List<Product> spare = new List<Product>(products);
            spare.Remove(this);

            int id = Tools.random.Next(0, spare.Count);
            skladnikZastepczyID = spare[id].skladnikID;
        }

        public int GetAmount()
        {
            int amount = 0;

            switch(jednostka)
            {
                case "gr":
                    amount = Tools.random.Next(5, 200);
                    break;

                case "szt":
                    amount = Tools.random.Next(1, 5);
                    break;

                case "ml":
                    amount = Tools.random.Next(100, 200);
                    break;
            }

            return amount;
        }

        public override string ToString()
        {
            return skladnikID +
                "|" + nazwaSkladnika +
                "|" + nrDostawcy +
                "|" + skladnikZastepczyID;
        }

        public string jednostka;
        public string nazwaSkladnika;
        public int nrDostawcy;
        public int skladnikID;
        public int skladnikZastepczyID;
    }
}
