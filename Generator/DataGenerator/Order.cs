using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.DataGenerator
{
    class Order
    {
        public Order()
        {
            szczegolyZamowien = new List<OrderDetail>(5);
        }

        public bool GenerateRandom(List<Dish> dishes, DateTime day, DateTime end, int index)
        {
            float deliveryDiceRoll = (float)Tools.random.NextDouble();
            if (deliveryDiceRoll <= DataGenerator.deliveryChance)
            {
                dostawa = new Delivery();
                dostawa.GenerateRandom();
            }
            else
            {
                dostawa = null;
            }

            stolik = Tools.random.Next(1, 20);
            nrZamowienia = index;

            data = new DateTime(day.Year, day.Month, day.Day, 8, 0, 0);


            bool result = RandomizeHour();
            if (!result) return false;

            int minute = Tools.random.Next(0, 59 + 1);
            int second = Tools.random.Next(0, 59 + 1);

            data = data.AddMinutes(minute);
            data = data.AddSeconds(second);


            int count = Tools.random.Next(1, 3 + 1);
            for(int i = 0; i < count; i++)
            {
                OrderDetail detail = new OrderDetail();
                detail.GenerateRandom(dishes, szczegolyZamowien, data, nrZamowienia);
                szczegolyZamowien.Add(detail);
            }

            return true;
        }

        private bool RandomizeHour()
        {
            int sum = 0;
            DateTime time = new DateTime(data.Ticks);
            for (int i = 0; i < DataGenerator.orderChances.Length; i++)
            {
                sum += DataGenerator.orderChances[i];
                time.AddHours(1);
            }

            if (sum == 0) return false;

            int diceRoll = Tools.random.Next(0, sum + 1);

            for(int i = 0; i < DataGenerator.orderChances.Length; i++)
            {
                if (diceRoll - DataGenerator.orderChances[i] <= 0)
                {
                    data = data.AddHours(i);
                    break;
                }
                else diceRoll -= DataGenerator.orderChances[i];
            }
            return true;

        }

        public override string ToString()
        {
            string str = nrZamowienia + "|";
            if (dostawa != null) str += dostawa.numerDostawy;
            str = str 
                + "|" + stolik
                + "|" + data.Year 
                + "-" + data.Month
                + "-" + data.Day
                + " " + data.Hour
                + ":" + data.Minute
                + ":" + data.Second; 
             
            return  str;
        }

        public int nrZamowienia;
        public Delivery dostawa;
        public int stolik;
        public DateTime data;
        public List<OrderDetail> szczegolyZamowien;
    }
}
