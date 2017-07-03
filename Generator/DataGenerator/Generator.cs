using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Generator.DataGenerator
{
    static public class DataGenerator
    {
        static DataGenerator()
        {
            numberOfDishes = 15;
            numberOfProducts = 5;
            numberOfCompanies = 3;
            maxDailyOrders = 50;
            minDailyOrders = 20;
            deliveryChance = 0.1f;
            lateChance = 0.05f;
            minDishComponents = 2;
            maxDishComponents = 4;
            employeesPerShift = 3;
            workerCount = 20;
            chiefCount = 2;
            returnChance = 0.003f;
            cancelChance = 0.001f;
            employeeFireChance = 0.01f;
            gradeChance = 0.5f;

            startDate = DateTime.Now;
            firstEndDate = DateTime.Now;
            firstEndDate = firstEndDate.AddDays(60);
            secondEndDate = DateTime.Now;
            secondEndDate = secondEndDate.AddDays(120);

            orderChances = new int[] { 1, 2, 4, 6, 9, 4, 2, 7, 15, 7, 4, 2, 1, 2, 3, 1};
            gradeChances = new int[] { 1, 2, 3, 4, 1 };


            products = new List<Product>(100);
            dishes = new List<Dish>(100);
            deliveries = new List<Delivery>(100);
            orders = new List<Order>(100);
            employees = new List<Employee>(100);
            workdays = new List<Workday>(1000);
        }

        static public void Generate()
        {
            Tools.ClearFiles();

            //------------------------ GENEROWANIE SKŁADNIKÓW -------------------//
            for (int i = 0; i < numberOfProducts; i++)
            {
                Product product = new Product();
                product.GenerateRandom(i);
                products.Add(product);
            }
            for (int i = 0; i < numberOfProducts; i++)
            {
                products[i].RandomizeSpareComponent(products);
            }

            Tools.OutputProducts(products);

            //------------------------ GENEROWANIE DAŃ -------------------------//
            for (int i = 0; i < numberOfDishes; i++)
            {
                Dish dish = new Dish();
                dish.GenerateRandom(products);
                dishes.Add(dish);
            }

            Tools.OutputDishes(dishes);

            //------------------------ GENEROWANIE ZAMÓWIEŃ ---------------------//
            int index = 0;
            float totalDays = (float)(secondEndDate - startDate).TotalDays;
            currentDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);

            for (int i = 0; i < (secondEndDate - startDate).TotalDays; i++)
            {
                orders.Clear();

                int count = Tools.random.Next(minDailyOrders, maxDailyOrders);

                for (int j = 0; j < count; j++)
                {
                    Order order = new Order();
                    order.GenerateRandom(dishes, currentDate, secondEndDate, index);
                    orders.Add(order);

                    index++;
                }

                Tools.OutputOrderDay(orders);
                currentDate = currentDate.AddDays(1);

                bar.Value = (int)(100 * i / totalDays);
            }

            //------------------------ GENEROWANIE PRACOWNIKÓW ------------------//
            //generuj początkowych pracowników
            for(int i = 0; i < workerCount; i++)
            {
                Employee employee = new Employee();
                employee.GenerateRandom(false, startDate);

                employees.Add(employee);
            }

            //generuj początkowych szefów kuchni
            for (int i = 0; i < chiefCount; i++)
            {
                Employee employee = new Employee();
                employee.GenerateRandom(true, startDate);

                employees.Add(employee);
            }


            //generuj dane przez kolejne dni, jeśli któryś pracownik został zwolniony - zastąp go kimś nowym
            currentDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            currentDate = currentDate.AddDays(1);
            for (int i = 0; i < (int)totalDays; i++)
            {
                int workers = CountWorkers(currentDate);
                int chiefs = CountChiefs(currentDate);

                while(workers < workerCount || chiefs < chiefCount)
                {
                    

                    while (chiefs < chiefCount && workers > 0)
                    {
                        List<Employee> workersList = GetWorkers(currentDate);
                        int id = Tools.random.Next(0, workersList.Count);
                        Employee e = employees.Find(x => x == workersList[id]);
                        e.szefKuchni = true;
                        e.dataAktualizacji = currentDate;
                        chiefs++;
                        workers--;

                        /*
                        Employee employee = new Employee();
                        employee.GenerateRandom(true, currentDate);

                        employees.Add(employee);
                        chiefs++;
                        */
                    }

                    while (workers < workerCount)
                    {
                        Employee employee = new Employee();
                        employee.GenerateRandom(false, currentDate);

                        employees.Add(employee);
                        workers++;
                    }

                    workers = CountWorkers(currentDate);
                    chiefs = CountChiefs(currentDate);
                }
                

                currentDate = currentDate.AddDays(1);
            }

            //------------------------ GENEROWANIE DNI PRACY --------------------//
            currentDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);

            Hashtable hashtable = new Hashtable();
            foreach(var person in employees)
            {
                hashtable.Add(person, 0); //osoba, obciążenie
            }

            while (currentDate < secondEndDate)
            {
                int currentMonth = currentDate.Month;

                //reset obciążenia co miesiąc
                foreach(var person in employees)
                {
                    hashtable[person] = 0;
                }

                while (currentMonth == currentDate.Month)
                {
                    //stwórz listę pracowników pracujących danego dnia
                    List<Employee> availablePeople = new List<Employee>();
                    List<int> workload = new List<int>();

                    foreach(var person in employees)
                    {
                        if(person.IsEmloyed(currentDate))
                        {
                            availablePeople.Add(person);
                            workload.Add((int)(hashtable[person]));
                        }
                    }

                    Workday workday = new Workday(currentDate);
                    workday.GenerateRandom(availablePeople, workload);
                    workdays.Add(workday);

                    for(int i = 0; i < availablePeople.Count; i++)
                    {
                        hashtable[availablePeople[i]] = workload[i];
                    }

                    currentDate = currentDate.AddDays(1);
                }
            }

            Tools.OutputExcel(employees, workdays);
            Tools.OutputCurrentDay("Dane/obecnyDzien1.txt", firstEndDate);
            Tools.OutputCurrentDay("Dane/obecnyDzien2.txt", secondEndDate);

            bar.Value = 0;


            MessageBox.Show("Dane zostały wygenerowane!");
        }

        static public void SetProgressBar(ProgressBar bar)
        {
            DataGenerator.bar = bar;
        }

        static private int CountWorkers(DateTime day)
        {
            int count = 0;
            foreach(var employee in employees)
            {
                if (employee.IsEmloyed(day) && !employee.szefKuchni) count++;
            }

            return count;
        }

        static private int CountChiefs(DateTime day)
        {
            int count = 0;
            foreach (var employee in employees)
            {
                if (employee.IsEmloyed(day) && employee.szefKuchni) count++;
            }

            return count;
        }

        static private List<Employee> GetWorkers(DateTime day)
        {
            List<Employee> workersList = new List<Employee>();
            foreach (var employee in employees)
            {
                if (employee.IsEmloyed(day) && !employee.szefKuchni)
                {
                    workersList.Add(employee);
                }
            }
            return workersList;
        }

        static private int GetWorkAmount(int workerIndex, DateTime yearMonth)
        {
            int count = 0;
            foreach(var workday in workdays)
            {
                if(workday.SameMonth(yearMonth))
                {
                    for(int i = 0; i < workday.morning.Length; i++)
                    {
                        if (workday.morning[i] == workerIndex) count++;
                    }
                }
            }
            return count;
        }

        static public int numberOfDishes;
        static public int numberOfProducts;
        static public int numberOfCompanies;
        static public int maxDailyOrders;
        static public int minDailyOrders;
        static public int maxDishComponents;
        static public int minDishComponents;
        static public int workerCount;
        static public int chiefCount;
        static public int employeesPerShift;
        static public float deliveryChance;
        static public float lateChance;
        static public float returnChance;
        static public float cancelChance;
        static public float employeeFireChance;
        static public float gradeChance;
        static public DateTime startDate;
        static public DateTime firstEndDate;
        static public DateTime secondEndDate;
        static public DateTime currentDate;
        static public int[] orderChances;
        static public int[] gradeChances;


        static List<Product> products;
        static List<Dish> dishes;
        static List<Delivery> deliveries;
        static List<Order> orders;
        static List<Employee> employees;
        static List<Workday> workdays;

        static private ProgressBar bar;
    }
}
