using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.DataGenerator
{
    class Employee
    {
        static Employee()
        {
            index = 0;
        }

        public void GenerateRandom(bool isChef, DateTime employedDay)
        {
            employedDay = new DateTime(employedDay.Year, employedDay.Month, employedDay.Day, 0, 0, 0);
            szefKuchni = isChef;
            imie = Tools.RandomName();
            nazwisko = Tools.RandomLastName();
            dataZatrudnienia = new DateTime(employedDay.Ticks);
            dataZwolnienia = new DateTime(employedDay.Ticks);
            dataAktualizacji = DateTime.MaxValue;
            identyfikator = index;
            index++;



            int total = (int)(DataGenerator.secondEndDate - employedDay).TotalDays;
            for(int i = 0; i < total; i++)
            {
                float diceRoll = (float)Tools.random.NextDouble();
                if (diceRoll <= DataGenerator.employeeFireChance) return;
                else dataZwolnienia = dataZwolnienia.AddDays(1);
            }

            dataZwolnienia = dataZwolnienia.AddDays(100);
        }

        public bool IsEmloyed(DateTime day)
        {
            if (dataZatrudnienia <= day && dataZwolnienia > day) return true;
            else return false;
        }

        public int identyfikator;
        public string imie;
        public string nazwisko;
        public bool szefKuchni;
        public DateTime dataZatrudnienia;
        public DateTime dataAktualizacji;
        public DateTime dataZwolnienia;


        private static int index;
    }
}
