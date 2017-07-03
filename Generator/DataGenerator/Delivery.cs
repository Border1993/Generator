using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.DataGenerator
{
    class Delivery
    {
        static Delivery()
        {
            
        }

        public void GenerateRandom()
        {
            imie = Tools.RandomName();
            nazwisko = Tools.RandomLastName();


            adres = "ADDR_" + Tools.RandomString(20, 30) + " " 
                + Tools.random.Next(1, 50) + "/" 
                + Tools.random.Next(1, 10);
            telefon = Tools.random.Next(100000000, 999999999 + 1).ToString();

            numerDostawy = index;
            index++;           
        }

        public override string ToString()
        {
            return numerDostawy.ToString() + "|" + adres + "|" + nazwisko + "|" + imie + "|" + telefon;
        }

        public int numerDostawy;
        public string imie;
        public string nazwisko;
        public string adres;
        public string telefon;

        private static int index = 0;
    }
}
