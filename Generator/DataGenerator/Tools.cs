using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;

namespace Generator.DataGenerator
{
    static class Tools
    {
        static Tools()
        {
            random = new Random();
            names = new List<string>();
            lastNames = new List<string>();

            StreamReader reader = new StreamReader("firstNames.txt");
            while (!reader.EndOfStream)
            {
                names.Add(reader.ReadLine());
            }
            reader.Close();

            reader = new StreamReader("lastNames.txt");
            while (!reader.EndOfStream)
            {
                lastNames.Add(reader.ReadLine());
            }
            reader.Close();
        }

        static public string RandomString(int length)
        {
            return RandomString(length, length);

        }

        static public string RandomComment(int minLength, int maxLength)
        {
            string str = "";
            int count = random.Next(minLength, maxLength + 1);

            while(str.Length < count)
            {
                str += RandomString(3, 15) + " ";
            }

            return str;
        }

        static public string RandomString(int minLength, int maxLength)
        {
            string str = "";
            int length = random.Next(minLength, maxLength + 1);

            for (int i = 0; i < length; i++)
            {
                int asciiCode = random.Next((int)'a', (int)'z');
                if (i == 0) asciiCode -= (int)'a' - (int)'A'; 
                str += (char)asciiCode;
            }

            return str;
        }

        static public void OutputProducts(List<Product> products)
        {
            StreamWriter writer = new StreamWriter("Dane/skladnik.BULK");
            writer.AutoFlush = true;

            foreach (var product in products)
            {
                writer.WriteLine(product.ToString());
            }

            writer.Close();
        }

        static public void OutputDishes(List<Dish> dishes)
        {
            StreamWriter dishWriter = new StreamWriter("Dane/potrawa.BULK");
            StreamWriter componentWriter = new StreamWriter("Dane/skladPotrawy.BULK");

            dishWriter.AutoFlush = true;
            componentWriter.AutoFlush = true;

            foreach(var dish in dishes)
            {
                foreach(var component in dish.skladniki)
                {
                    componentWriter.WriteLine(component.ToString());
                }
                dishWriter.WriteLine(dish.ToString());
            }

            dishWriter.Close();
            componentWriter.Close();
        }

        static public void ClearFiles()
        {
            if (!Directory.Exists("Dane")) Directory.CreateDirectory("Dane");

            if(File.Exists("Dane/skladnik.BULK")) File.Delete("Dane/skladnik.BULK");
            if (File.Exists("Dane/potrawa.BULK")) File.Delete("Dane/potrawa.BULK");
            if (File.Exists("Dane/skladPotrawy.BULK")) File.Delete("Dane/skladPotrawy.BULK");

            if (File.Exists("Dane/zamowienie.BULK")) File.Delete("Dane/zamowienie.BULK");
            if (File.Exists("Dane/szczegolyZamowienia.BULK")) File.Delete("Dane/szczegolyZamowienia.BULK");
            if (File.Exists("Dane/dostawa.BULK")) File.Delete("Dane/dostawa.BULK");

            if (File.Exists("Dane/zamowienie2.BULK")) File.Delete("Dane/zamowienie2.BULK");
            if (File.Exists("Dane/szczegolyZamowienia2.BULK")) File.Delete("Dane/szczegolyZamowienia2.BULK");
            if (File.Exists("Dane/dostawa2.BULK")) File.Delete("Dane/dostawa2.BULK");
        }

        static public string ToWord(bool b)
        {
            if (b) return "TAK";
            else return "NIE";
        }

        static public string RandomName()
        {
            int index = random.Next(0, names.Count);
            return names[index];
        }

        static public string RandomLastName()
        {
            int index = random.Next(0, lastNames.Count);
            return lastNames[index];
        }

        static public void OutputOrderDay(List<Order> orders)
        {
            StreamWriter orderWriter2 = new StreamWriter("Dane/zamowienie2.BULK", true);
            StreamWriter orderWriter = new StreamWriter("Dane/zamowienie.BULK", true);
            StreamWriter orderDetailsWriter2 = new StreamWriter("Dane/szczegolyZamowienia2.BULK", true);
            StreamWriter orderDetailsWriter = new StreamWriter("Dane/szczegolyZamowienia.BULK", true);
            StreamWriter deliveryWriter2 = new StreamWriter("Dane/dostawa2.BULK", true);
            StreamWriter deliveryWriter = new StreamWriter("Dane/dostawa.BULK", true);

            foreach (var order in orders)
            {
                if(order.dostawa != null)
                {
                    deliveryWriter2.WriteLine(order.dostawa.ToString());
                    if(DataGenerator.currentDate <= DataGenerator.firstEndDate) deliveryWriter.WriteLine(order.dostawa.ToString());
                }

                orderWriter2.WriteLine(order.ToString());
                if (DataGenerator.currentDate <= DataGenerator.firstEndDate) orderWriter.WriteLine(order.ToString());

                foreach(var orderDetail in order.szczegolyZamowien)
                {
                    orderDetailsWriter2.WriteLine(orderDetail.ToString());
                    if (DataGenerator.currentDate <= DataGenerator.firstEndDate) orderDetailsWriter.WriteLine(orderDetail.ToString());
                }
            }


            orderWriter.Close();
            orderDetailsWriter.Close();
            deliveryWriter.Close();

            orderWriter2.Close();
            orderDetailsWriter2.Close();
            deliveryWriter2.Close();
        }

        static public void OutputCurrentDay(string filename, DateTime day)
        {
            StreamWriter sw = new StreamWriter(filename);

            sw.WriteLine(day.Year + " " + day.Month + " " + day.Day);

            sw.Flush();
            sw.Close();
        }

        static public void OutputExcel(List<Employee> employees, List<Workday> workdays)
        {
            OutputToDate(employees, workdays, DataGenerator.firstEndDate, "Dane/pracownicy i grafik1.xlsx");  
            OutputToDate(employees, workdays, DataGenerator.secondEndDate, "Dane/pracownicy i grafik2.xlsx");
        }

        static private void OutputToDate(List<Employee> employees, List<Workday> workdays, DateTime date, string filename)
        {
            DateTime currentDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);

            //----------------------------------- PRACOWNICY --------------------//
            if (File.Exists(filename)) File.Delete(filename);

            ExcelPackage package = new ExcelPackage();
            ExcelWorkbook wbook = package.Workbook;
            ExcelWorksheet ws = wbook.Worksheets.Add("Pracownicy");

            ws.Cells["A1"].Value = "Imię i nazwisko";
            ws.Cells["B1"].Value = "Identyfikator";
            ws.Cells["C1"].Value = "Stanowisko";
            ws.Cells["D1"].Value = "Data zatrudnienia";
            ws.Cells["E1"].Value = "Data zwolnienia";
            //ws.Cells["F1"].Value = "Data aktualizacji";


            for (int i = 0; i < employees.Count; i++)
            {
                DateTime dataZatrudnienia = new DateTime(employees[i].dataZatrudnienia.Year, employees[i].dataZatrudnienia.Month, employees[i].dataZatrudnienia.Day, 0, 0, 0);
                DateTime dataAktualizacji = DateTime.MaxValue;
                if(employees[i].dataAktualizacji != DateTime.MaxValue) dataAktualizacji = new DateTime(employees[i].dataAktualizacji.Year, employees[i].dataAktualizacji.Month, employees[i].dataAktualizacji.Day, 0, 0, 0); 
                DateTime dataZwolnienia = new DateTime(employees[i].dataZwolnienia.Year, employees[i].dataZwolnienia.Month, employees[i].dataZwolnienia.Day, 0, 0, 0);

                if (dataZatrudnienia > currentDate) break;

                ws.Cells["A" + (i + 2).ToString()].Value = employees[i].imie + " " + employees[i].nazwisko;
                ws.Cells["B" + (i + 2).ToString()].Value = employees[i].identyfikator;

                if (employees[i].szefKuchni && dataAktualizacji == DateTime.MaxValue) ws.Cells["C" + (i + 2).ToString()].Value = "Szef kuchni";
                else if (employees[i].szefKuchni && dataAktualizacji <= currentDate) ws.Cells["C" + (i + 2).ToString()].Value = "Szef kuchni";
                else ws.Cells["C" + (i + 2).ToString()].Value = "Kucharz";

                ws.Cells["D" + (i + 2).ToString()].Value =
                    dataZatrudnienia.Day + "-"
                    + dataZatrudnienia.Month + "-"
                    + dataZatrudnienia.Year;

                if (dataZwolnienia <= currentDate)
                {
                    ws.Cells["E" + (i + 2).ToString()].Value =
                        dataZwolnienia.Day + "-"
                        + dataZwolnienia.Month + "-"
                        + dataZwolnienia.Year;
                }

                /*
                if (dataAktualizacji <= currentDate && dataAktualizacji != DateTime.MaxValue)
                {
                    ws.Cells["F" + (i + 2).ToString()].Value =
                        dataAktualizacji.Day + "-"
                        + dataAktualizacji.Month + "-"
                        + dataAktualizacji.Year;
                }
                */
            }

            ws.Column(1).Width = 30;
            ws.Column(2).Width = 14;
            ws.Column(3).Width = 20;
            ws.Column(4).Width = 20;
            ws.Column(5).Width = 20;

            //----------------------------------- GRAFIK ------------------------//
            ws = wbook.Worksheets.Add("Grafik");

            ws.Cells["A1"].Value = "Data";
            ws.Column(1).Width = 15;

            for (int i = 0; i < DataGenerator.employeesPerShift; i++)
            {
                string dayAdress = (char)('A' + i + 1 + 0 * DataGenerator.employeesPerShift) + "1";
                string middayAdress = (char)('A' + i + 1 + 1 * DataGenerator.employeesPerShift) + "1";
                string eveningAdress = (char)('A' + i + 1 + 2 * DataGenerator.employeesPerShift) + "1";

                ws.Cells[dayAdress].Value = "Poranna " + (i + 1);
                ws.Cells[middayAdress].Value = "Popołudniowa " + (i + 1);
                ws.Cells[eveningAdress].Value = "Wieczorna " + (i + 1);

                ws.Column(i + 2 + 0 * DataGenerator.employeesPerShift).Width = 15;
                ws.Column(i + 2 + 1 * DataGenerator.employeesPerShift).Width = 15;
                ws.Column(i + 2 + 2 * DataGenerator.employeesPerShift).Width = 15;
            }

            for(int i = 0; i < workdays.Count; i++)
            {
                DateTime day = new DateTime(workdays[i].day.Year, workdays[i].day.Month, workdays[i].day.Day, 0, 0, 0);
                if (day > date) break;

                string dateAdress = 'A' + (i+2).ToString();
                ws.Cells[dateAdress].Value = day.Day + "-" + day.Month + "-" + day.Year;

                for(int j = 0; j < DataGenerator.employeesPerShift; j++)
                {
                    string dayAdress = (char)('A' + 1 + j + 0 * DataGenerator.employeesPerShift) + (i + 2).ToString();
                    string middayAdress = (char)('A' + 1 + j + 1 * DataGenerator.employeesPerShift) + (i + 2).ToString();
                    string eveningAdress = (char)('A' + 1 + j + 2 * DataGenerator.employeesPerShift) + (i + 2).ToString();

                    ws.Cells[dayAdress].Value = workdays[i].morning[j];
                    ws.Cells[middayAdress].Value = workdays[i].midday[j];
                    ws.Cells[eveningAdress].Value = workdays[i].evening[j];
                }
            }



            //----------------------------------- ZAPISANIE DANYCH --------------//
            Stream stream = new FileStream(filename, FileMode.CreateNew);
            package.SaveAs(stream);
            stream.Close();
        }

        static public Random random;
        static private List<string> names;
        static private List<string> lastNames;
    }
}
