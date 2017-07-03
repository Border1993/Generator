using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Generator.DataGenerator;
using System.Windows.Forms.DataVisualization.Charting;

namespace Generator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            initialized = false;

            InitializeComponent();

            orders.Rows.Add(1, 2, 4, 6, 9, 4, 2, 7, 15, 7, 4, 2, 1, 2, 3, 1, 1);
            grades.Rows.Add(1, 2, 4, 5, 1);

            RebuildOrdersChart();
            RebuildGradesChart();

            dishesCount.Value = DataGenerator.DataGenerator.numberOfDishes;
            productsCount.Value = DataGenerator.DataGenerator.numberOfProducts;
            companiesCount.Value = DataGenerator.DataGenerator.numberOfCompanies;
            ordersDailyMin.Value = DataGenerator.DataGenerator.minDailyOrders;
            ordersDailyMax.Value = DataGenerator.DataGenerator.maxDailyOrders;
            componentsPerDishMin.Value = DataGenerator.DataGenerator.minDishComponents;
            componentsPerDishMax.Value = DataGenerator.DataGenerator.maxDishComponents;
            workerCount.Value = DataGenerator.DataGenerator.workerCount;
            chiefCount.Value = DataGenerator.DataGenerator.chiefCount;
            employeesPerShift.Value = DataGenerator.DataGenerator.employeesPerShift;

            deliveryChance.Value = (decimal)DataGenerator.DataGenerator.deliveryChance;
            lateChance.Value = (decimal)DataGenerator.DataGenerator.lateChance;
            returnChance.Value = (decimal)DataGenerator.DataGenerator.returnChance;
            cancelChance.Value = (decimal)DataGenerator.DataGenerator.cancelChance;
            employeeFireChance.Value = (decimal)DataGenerator.DataGenerator.employeeFireChance;
            gradeChance.Value = (decimal)DataGenerator.DataGenerator.gradeChance;

            startDate.Value = DataGenerator.DataGenerator.startDate;
            firstEndDate.Value = DataGenerator.DataGenerator.firstEndDate;
            secondEndDate.Value = DataGenerator.DataGenerator.secondEndDate;

            for(int i = 0; i < DataGenerator.DataGenerator.orderChances.Length; i++)
            {
                int value = DataGenerator.DataGenerator.orderChances[i];
                orders[i, 0].Value = value.ToString();
            }

            for (int i = 0; i < DataGenerator.DataGenerator.gradeChances.Length; i++)
            {
                int value = DataGenerator.DataGenerator.gradeChances[i];
                grades[i, 0].Value = value.ToString();
            }

            DataGenerator.DataGenerator.SetProgressBar(progressBar);

            initialized = true;
        }

        private void dishesCount_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            DataGenerator.DataGenerator.numberOfDishes = int.Parse(dishesCount.Value.ToString());
        }

        private void productsCount_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            DataGenerator.DataGenerator.numberOfProducts = int.Parse(productsCount.Value.ToString());
        }

        private void companiesCount_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            DataGenerator.DataGenerator.numberOfCompanies = int.Parse(companiesCount.Value.ToString());
        }

        private void ordersDailyMin_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            DataGenerator.DataGenerator.minDailyOrders = (int)ordersDailyMin.Value;
            if (ordersDailyMin.Value > ordersDailyMax.Value) ordersDailyMax.Value = ordersDailyMin.Value;
        }

        private void ordersDailyMax_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            DataGenerator.DataGenerator.maxDailyOrders = (int)ordersDailyMax.Value;
            if (ordersDailyMin.Value > ordersDailyMax.Value) ordersDailyMin.Value = ordersDailyMax.Value;
        }

        private void componentsPerDishMin_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            DataGenerator.DataGenerator.minDishComponents = (int)componentsPerDishMin.Value;
            if (componentsPerDishMin.Value > componentsPerDishMax.Value) componentsPerDishMax.Value = componentsPerDishMin.Value;
        }

        private void componentsPerDishMax_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            DataGenerator.DataGenerator.maxDishComponents = (int)componentsPerDishMax.Value;
            if (componentsPerDishMin.Value > componentsPerDishMax.Value) componentsPerDishMin.Value = componentsPerDishMax.Value;
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            DataGenerator.DataGenerator.Generate();
        }

        private void RebuildOrdersChart()
        {
            ordersChart.Series.Clear();

            Series series = ordersChart.Series.Add("orders");
            series.YValueType = ChartValueType.Int32;
            series.ChartType = SeriesChartType.Spline;
            series.Color = Color.Red;
            series.BorderWidth = 2;

            int i;
            for(i = 8; i < 23; i++)
            {
                series.Points.AddXY(i.ToString() + ":00", orders[i-8, 0].Value);
            }

            series.Points.AddXY("00:00", orders[i-8, 0].Value);


            int max = 0;
            int min = int.Parse(orders[0, 0].Value.ToString());
            for(i = 0; i < orders.ColumnCount; i++)
            {
                int value = int.Parse(orders[i, 0].Value.ToString());
                if (value > max) max = value;
                if (value < min) min = value;

            }


            for(i = 0; i < ordersChart.ChartAreas.Count; i++)
            {
                ordersChart.ChartAreas[i].AxisY.Maximum = max + 1;
                ordersChart.ChartAreas[i].AxisY.Minimum = Math.Max(min - 1, 0);
                ordersChart.ChartAreas[i].AxisX.Maximum = 18;
            }

            ordersChart.Invalidate();
            ordersChart.Update();
        }

        private void orders_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!initialized) return;
            if (orders.RowCount > 0)
            {
                for(int i = 0; i < orders.ColumnCount; i++)
                {
                    if (int.Parse(orders[i, 0].Value.ToString()) < 0) orders[i, 0].Value = "0";
                }
                RebuildOrdersChart();
            }
        }

        private void RebuildGradesChart()
        {
            gradesChart.Series.Clear();

            Series series = gradesChart.Series.Add("orders");
            series.YValueType = ChartValueType.Int32;
            series.ChartType = SeriesChartType.Column;
            series.Color = Color.Red;
            series.BorderWidth = 2;

            int i;
            for (i = 1; i <= 5; i++)
            {
                series.Points.AddXY(i.ToString(), grades[i - 1, 0].Value);
            }


            int max = 0;
            int min = int.Parse(grades[0, 0].Value.ToString());
            for (i = 0; i < grades.ColumnCount; i++)
            {
                int value = int.Parse(grades[i, 0].Value.ToString());
                if (value > max) max = value;
                if (value < min) min = value;

            }


            for (i = 0; i < gradesChart.ChartAreas.Count; i++)
            {
                gradesChart.ChartAreas[i].AxisY.Maximum = max + 1;
                gradesChart.ChartAreas[i].AxisY.Minimum = Math.Max(min - 1, 0);
                gradesChart.ChartAreas[i].AxisX.Maximum = 6;
                gradesChart.ChartAreas[i].AxisX.Minimum = 0;
            }

            gradesChart.Invalidate();
            gradesChart.Update();
        }

        private void grades_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!initialized) return;
            if (grades.RowCount > 0)
            {
                for (int i = 0; i < grades.ColumnCount; i++)
                {
                    if (int.Parse(orders[i, 0].Value.ToString()) < 0) grades[i, 0].Value = "0";
                }
                RebuildGradesChart();
            }
        }

        private void workerCount_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            DataGenerator.DataGenerator.workerCount = (int)workerCount.Value;
        }

        private void chiefCount_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            DataGenerator.DataGenerator.chiefCount = (int)chiefCount.Value;
        }

        private void startDatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            DataGenerator.DataGenerator.startDate = startDate.Value;
            firstEndDatePicker_ValueChanged(sender, e);
            secondEndDatePicker_ValueChanged(sender, e);
        }

        private void firstEndDatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            if (firstEndDate.Value < startDate.Value) firstEndDate.Value = startDate.Value;
            DataGenerator.DataGenerator.firstEndDate = firstEndDate.Value;
            secondEndDatePicker_ValueChanged(sender, e);
        }

        private void secondEndDatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            if (secondEndDate.Value < startDate.Value) secondEndDate.Value = startDate.Value;
            if (secondEndDate.Value < firstEndDate.Value) secondEndDate.Value = firstEndDate.Value;
            DataGenerator.DataGenerator.secondEndDate = secondEndDate.Value;
        }

        private void deliveryChance_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            DataGenerator.DataGenerator.deliveryChance = float.Parse(deliveryChance.Value.ToString());
        }

        private void lateChance_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            DataGenerator.DataGenerator.lateChance = float.Parse(lateChance.Value.ToString());
        }

        private void returnChance_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            DataGenerator.DataGenerator.lateChance = float.Parse(returnChance.Value.ToString());
        }

        private void cancelChance_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            DataGenerator.DataGenerator.lateChance = float.Parse(cancelChance.Value.ToString());
        }

        private void employeeFireChance_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            DataGenerator.DataGenerator.employeeFireChance = float.Parse(employeeFireChance.Value.ToString());
        }

        private void gradeChance_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            DataGenerator.DataGenerator.gradeChance = float.Parse(gradeChance.Value.ToString());
        }

        private bool initialized;

        private void employeesPerShift_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized) return;
            DataGenerator.DataGenerator.employeesPerShift = int.Parse(employeesPerShift.Value.ToString());
        }
    }
}
