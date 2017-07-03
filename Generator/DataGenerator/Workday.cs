using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.DataGenerator
{
    class Workday
    {
        public Workday(DateTime day)
        {
            morning = new int[DataGenerator.employeesPerShift];
            midday = new int[DataGenerator.employeesPerShift];
            evening = new int[DataGenerator.employeesPerShift];
            this.day = day;
        }

        public bool SameMonth(DateTime monthYear)
        {
            DateTime a = new DateTime(monthYear.Year, monthYear.Month, 1);
            DateTime b = new DateTime(day.Year, day.Month, day.Day);

            if (a == b) return true;
            else return false;
        }

        public void GenerateRandom(List<Employee> employees, List<int> workload)
        {
            List<Employee> listOfEmployees = new List<Employee>(employees); //kopia, bo będziemy usuwać elementy


            List<Employee> morningShift = GetEmployeesWithSmallestWorkload(listOfEmployees, workload);
            int morningCount = 0;
            for(int i = 0; i < employees.Count; i++) //ważne - nie używać kopii listy pracowników! ważny jest oryginalny indeks!
            {
                foreach(var person in morningShift)
                {
                    if(employees[i] == person)
                    {
                        workload[i]++;
                        morning[morningCount] = person.identyfikator; //indeksy te same w ORYGINALNEJ liście pracowników i ich obciążeniu
                        morningCount++;
                        listOfEmployees.Remove(person); //nie pozwól żeby pracownik pracował 2 - 3 zmiany w ciągu dnia!
                    }
                }
            }

            List<Employee> middayShift = GetEmployeesWithSmallestWorkload(listOfEmployees, workload);
            int middayCount = 0;
            for (int i = 0; i < employees.Count; i++)
            {
                foreach (var person in middayShift)
                {
                    if (employees[i] == person)
                    {
                        workload[i]++;
                        midday[middayCount] = person.identyfikator;
                        middayCount++;
                        listOfEmployees.Remove(person);
                    }
                }
            }

            List<Employee> eveningShift = GetEmployeesWithSmallestWorkload(listOfEmployees, workload);
            int eveningCount = 0;
            for (int i = 0; i < employees.Count; i++)
            {
                foreach (var person in eveningShift)
                {
                    if (employees[i] == person)
                    {
                        workload[i]++;
                        evening[eveningCount] = person.identyfikator;
                        eveningCount++;
                        listOfEmployees.Remove(person);
                    }
                }
            }


        }

        static private List<Employee> GetEmployeesWithSmallestWorkload(List<Employee> employees, List<int> workload)
        {
            List<Employee> ret = new List<Employee>();
            List<Employee> candidates = new List<Employee>();
            List<Workload> employeesWorkload = new List<Workload>();

            for(int i = 0; i < employees.Count; i++)
            {
                Workload wl = new Workload();
                wl.workload = workload[i];
                wl.employee = employees[i];

                employeesWorkload.Add(wl);
            }

            employeesWorkload.Sort();

            int index = 0;
            int selectCount = DataGenerator.employeesPerShift;

            while (candidates.Count < selectCount)
            {
                int value = employeesWorkload[index].workload;
                while(index < employeesWorkload.Count && employeesWorkload[index].workload == value)
                {
                    candidates.Add(employeesWorkload[index].employee);
                    index++;
                }

                if(candidates.Count < selectCount)
                {
                    ret.AddRange(candidates);
                    selectCount -= candidates.Count;
                    candidates.Clear();
                }
            }

            for(int i = 0; i < selectCount; i++)
            {
                int rand = Tools.random.Next(0, candidates.Count);
                ret.Add(candidates[rand]);
                candidates.RemoveAt(rand);
            }

            return ret;
        }

        private class Workload : IComparable<Workload>
        {
            public int workload;
            public Employee employee;

            public int CompareTo(Workload other)
            {
                return workload - other.workload;
            }
        }

        public int[] morning;
        public int[] midday;
        public int[] evening;
        public DateTime day;
    }
}
