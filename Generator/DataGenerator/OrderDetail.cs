using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.DataGenerator
{
    class OrderDetail
    {

        public bool GenerateRandom(List<Dish> dishes, List<OrderDetail> currentDetails, DateTime data, int index)
        {
            GetRandomDish(dishes, currentDetails);
            if (nazwaPotrawy == "")
            {
                ilosc = 0;
                return false;
            }

            nrZamowienia = index;
            dataZamowienia = new DateTime(data.Ticks);
            dataRealizacji = new DateTime(data.Ticks);

            float diceRoll = (float)Tools.random.NextDouble();
            if (diceRoll <= DataGenerator.lateChance)
            {
                int minutes = Tools.random.Next(10, 19 + 1);
                int seconds = Tools.random.Next(0, 59 + 1);


                dataRealizacji = dataRealizacji.AddMinutes(minutes);
                dataRealizacji = dataRealizacji.AddSeconds(seconds);

                if (dataRealizacji.Day != dataZamowienia.Day) dataRealizacji = new DateTime(dataZamowienia.Year, dataZamowienia.Month, dataZamowienia.Day, 23, 59, 59);
            }
            else
            {
                int minutes = 0;
                int seconds = 0;

                int hours = Tools.random.Next(0, 100 + 1) - 99; // 1/100 szansy że zamówienie będzie realizowane ponad godzinę
                if(hours == 1)
                {
                    minutes = Tools.random.Next(0, 59 + 1);
                    seconds = Tools.random.Next(0, 59 + 1);
                }
                else
                {
                    minutes = Tools.random.Next(20, 49 + 1);
                    seconds = Tools.random.Next(0, 59 + 1);
                }

                dataRealizacji = dataRealizacji.AddMinutes(minutes);
                dataRealizacji = dataRealizacji.AddSeconds(seconds);
                if(hours > 0) dataRealizacji = dataRealizacji.AddHours(hours);

                if (dataRealizacji.Day != dataZamowienia.Day) dataRealizacji = new DateTime(dataZamowienia.Year, dataZamowienia.Month, dataZamowienia.Day, 23, 59, 59);
            }

            ilosc = Tools.random.Next(1, 4);

            diceRoll = (float)Tools.random.NextDouble();
            if (diceRoll <= 0.05f) komentarz = Tools.RandomComment(250, 500);
            else komentarz = "";

            diceRoll = (float)Tools.random.NextDouble();
            if (diceRoll <= DataGenerator.cancelChance) anulowane = true;
            else anulowane = false;

            if(!anulowane)
            {
                diceRoll = (float)Tools.random.NextDouble();
                if (diceRoll <= DataGenerator.returnChance) zwrocone = true;
                else zwrocone = false;
            }

            if (!anulowane && !zwrocone) GenerateRandomGrade();
            else ocena = 1;

            return true;
        }

        private void GetRandomDish(List<Dish> dishes, List<OrderDetail> currentDetails)
        {
            List<int> availableDishes = new List<int>();

            for (int i = 0; i < dishes.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < currentDetails.Count; j++)
                {
                    if (currentDetails[j].nazwaPotrawy == dishes[i].nazwaPotrawy)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    availableDishes.Add(i);
                }
            }

            if (availableDishes.Count == 0) return;
            int rand = Tools.random.Next(0, availableDishes.Count);
            int index = availableDishes[rand];
            nazwaPotrawy = dishes[index].nazwaPotrawy;
        }

        private void GenerateRandomGrade()
        {
            float diceRoll = (float)Tools.random.NextDouble();
            if (diceRoll > DataGenerator.gradeChance)
            {
                ocena = 0;
                return;
            }

            int sum = 0;
            ocena = 1;
            for(int i = 0; i < DataGenerator.gradeChances.Length; i++)
            {
                sum += DataGenerator.gradeChances[i];
            }

            int rand = Tools.random.Next(0, sum + 1);

            for(int i = 0; i < DataGenerator.gradeChances.Length; i++)
            {
                if (rand - DataGenerator.gradeChances[i] < 0)
                {
                    break;
                }
                else
                {
                    rand -= DataGenerator.gradeChances[i];
                    ocena++;
                }
            }
        }

        public override string ToString()
        {
            string ocena = "";
            if (this.ocena > 0) ocena = this.ocena.ToString();
            else ocena = "";

            return nrZamowienia
                + "|" + nazwaPotrawy
                + "|" + ilosc
                + "|" + ocena
                + "|" + Tools.ToWord(zwrocone)
                + "|" + Tools.ToWord(anulowane)
                + "|" + dataZamowienia.Year + "-" + dataZamowienia.Month + "-" + dataZamowienia.Day 
                + " " + dataZamowienia.Hour + ":"+ dataZamowienia.Minute + ":" + dataZamowienia.Second
                + "|" + dataRealizacji.Year + "-" + dataRealizacji.Month + "-" + dataRealizacji.Day
                + " " + dataRealizacji.Hour + ":" + dataRealizacji.Minute + ":" + dataRealizacji.Second
                + "|" + komentarz;
        }

        public string komentarz;
        public string nazwaPotrawy;
        public int ilosc;
        public int ocena;
        public int nrZamowienia;
        public bool zwrocone;
        public bool anulowane;
        public DateTime dataZamowienia;
        public DateTime dataRealizacji;
    }
}
