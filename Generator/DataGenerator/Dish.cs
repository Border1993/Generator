using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.DataGenerator
{
    class Dish
    {
        static Dish()
        {
            string[] typy = new string[]
            {
                "pizza",
                "rybne",
                "z makaronem",
                "owoce morza",
                "lody",
                "ciasta",
                "kawa",
                "wino"
            };

            typyPotraw = new List<string>(typy);
            

            string[] kategorie = new string[]
            {
                "pizza",
                "danie rybne",
                "danie z makaronem",
                "owoce morza",
                "ciasto",
                "lody",
                "kawa",
                "wino"
            };

            kategoriePotraw = new List<string>(kategorie);
        }

        public void GenerateRandom(List<Product> products)
        {
            float cena;
            nazwaPotrawy = "POTRAWA_" + Tools.RandomString(5, 15);
            cena = (float)Tools.random.NextDouble();
            cena *= 20;
            cena += 10.0f;
            this.cena = (decimal)cena;
            Math.Round(this.cena, 2, MidpointRounding.AwayFromZero);
            

            int index = Tools.random.Next(0, typyPotraw.Count);
            typ = typyPotraw[index];

            index = Tools.random.Next(0, kategoriePotraw.Count);
            kategoria = kategoriePotraw[index];

            skladniki = new List<Component>(DataGenerator.maxDishComponents);
            int productCount = Tools.random.Next(DataGenerator.minDishComponents, DataGenerator.maxDishComponents + 1);

            for(int i = 0; i < productCount; i++)
            {
                Component component = new Component();
                if(component.GenerateRandom(products, skladniki, nazwaPotrawy))
                {
                    skladniki.Add(component);
                }
            }
        }

        public override string ToString()
        {
            return nazwaPotrawy +
                "|" + typ +
                "|" + (Math.Round((decimal)cena, 2, MidpointRounding.AwayFromZero).ToString()).Replace(',', '.');
        }

        public string nazwaPotrawy;
        public string typ;
        public string kategoria;
        public decimal cena;
        public List<Component> skladniki;

        private static List<string> typyPotraw;
        private static List<string> kategoriePotraw;
    }
}
