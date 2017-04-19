using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace IDPP.LocalServer
{
    public class DashboardForm : PanelForm
    {
        public DashboardForm(AppWindow owner) : base(owner)
        {
            this.Load += delegate { this.RefreshDashboard(); };
        }

        void RefreshDashboard()
        {
            IdpDb db = new IdpDb();
            var persons = db.GetPersons();

            ChartArea caGender = new ChartArea()
            {
                Name = "caGender"
            };
            Legend leGender = new Legend()
            {
                BackColor = Color.Green,
                ForeColor = Color.Black,
                Name = "leGender",
                Title = "Gender"
            };
            Chart chGender = new Chart()
            {
                BackColor = Color.LightYellow,
                Name = "chGender",
                Location = new Point(100, 100),
                Size = new Size(400, 400),
            };
            Series serGender = new Series()
            {
                Name = "serGender",
                Color = Color.Green,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Pie
            };
            double females = persons.Count(p => p.Gender.ToLower() == "female");
            double males = persons.Count(p => p.Gender.ToLower() == "male");
            serGender.Points.Add(females);
            serGender.Points.Add(males);
            serGender.Points[0].AxisLabel = females + " (" + (females / (females + males)) * 100 + "%)";
            serGender.Points[0].LegendText = "Female";
            serGender.Points[1].AxisLabel = males + " (" + (males / (females + males)) * 100 + "%)";
            serGender.Points[1].LegendText = "Male";

            chGender.ChartAreas.Add(caGender);
            chGender.ChartAreas[0].BackColor = Color.Transparent;
            chGender.Legends.Add(leGender);
            chGender.Series.Clear();
            chGender.Palette = ChartColorPalette.Fire;
            chGender.Titles.Add("Gender Distribution");
            chGender.Series.Add(serGender);
            chGender.Invalidate();
            this.Controls.Add(chGender);

            ChartArea caMarital = new ChartArea()
            {
                Name = "caMarital"
            };
            Legend leMarital = new Legend()
            {
                BackColor = Color.Blue,
                ForeColor = Color.White,
                Name = "leMarital",
                Title = "Marital Status"
            };
            Chart chMarital = new Chart()
            {
                BackColor = Color.LightBlue,
                Name = "chMarital",
                Location = new Point(chGender.Right + 100, chGender.Top),
                Size = chGender.Size,
            };
            Series serMarital = new Series()
            {
                Name = "serMarital",
                Color = Color.Green,
                IsVisibleInLegend = true,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Pie
            };
            double single = persons.Count(p => p.MaritalStatus.ToLower() == "single");
            double married = persons.Count(p => p.MaritalStatus.ToLower() == "married");
            double widow = persons.Count(p => p.MaritalStatus.ToLower() == "widow");
            double widower = persons.Count(p => p.MaritalStatus.ToLower() == "widower");
            double divorced = persons.Count(p => p.MaritalStatus.ToLower() == "divorced");
            double totalMarital = single + married + widow + widower + divorced;
            serMarital.Points.Add(single);
            serMarital.Points.Add(married);
            serMarital.Points.Add(widow);
            serMarital.Points.Add(widower);
            serMarital.Points.Add(divorced);
            serMarital.Points[0].AxisLabel = single + " (" + (single / totalMarital) * 100 + "%)";
            serMarital.Points[0].LegendText = "Single";
            serMarital.Points[1].AxisLabel = married + " (" + (married / totalMarital) * 100 + "%)";
            serMarital.Points[1].LegendText = "Married";
            serMarital.Points[2].AxisLabel = widow + " (" + (widow / totalMarital) * 100 + "%)";
            serMarital.Points[2].LegendText = "Widow";
            serMarital.Points[3].AxisLabel = widower + " (" + (widower / totalMarital) * 100 + "%)";
            serMarital.Points[3].LegendText = "Widower";
            serMarital.Points[4].AxisLabel = divorced + " (" + (divorced / totalMarital) * 100 + "%)";
            serMarital.Points[4].LegendText = "Divorced";

            chMarital.ChartAreas.Add(caMarital);
            chMarital.ChartAreas[0].BackColor = Color.Transparent;
            chMarital.Legends.Add(leMarital);
            chMarital.Series.Clear();
            //chMarital.Palette = ChartColorPalette.Berry;
            chMarital.Titles.Add("Marital Status Distribution");
            chMarital.Series.Add(serMarital);
            chMarital.Invalidate();
            this.Controls.Add(chMarital);

            ChartArea caAge = new ChartArea()
            {
                Name = "caAge"
            };
            Legend legAge = new Legend()
            {
                BackColor = Color.Green,
                ForeColor = Color.Black,
                Name = "legAge",
                Title = "Age Group"
            };
            Chart chAge = new Chart()
            {
                BackColor = Color.LightGreen,
                Name = "chAge",
                Location = new Point(chGender.Left, chGender.Bottom + 100),
                Size = new Size(900, chGender.Height),
            };
            Series serAge = new Series()
            {
                Name = "serAge",
                Color = Color.Green,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Column
            };
            double zeroTo4 = persons.Count(p => age(p) >= 0 && age(p) < 5);
            double fiveTo12 = persons.Count(p => age(p) >= 5 && age(p) < 13);
            double thirteenTo19 = persons.Count(p => age(p) >= 13 && age(p) < 20);
            double twentyTo35 = persons.Count(p => age(p) >= 20 && age(p) < 36);
            double thirtysixTo70 = persons.Count(p => age(p) >= 36 && age(p) < 71);
            double above70 = persons.Count(p => age(p) >= 71);
            double totalAge = zeroTo4 + fiveTo12 + thirteenTo19 + twentyTo35 + thirtysixTo70 + above70;
            serAge.Points.Add(zeroTo4);
            serAge.Points.Add(fiveTo12);
            serAge.Points.Add(thirteenTo19);
            serAge.Points.Add(twentyTo35);
            serAge.Points.Add(thirtysixTo70);
            serAge.Points.Add(above70);
            serAge.Points[0].AxisLabel = "0-4";
            serAge.Points[0].LegendText = "0-4";
            serAge.Points[0].Label = zeroTo4 + " (" + (zeroTo4 / totalAge) * 100 + "%)";
            serAge.Points[1].AxisLabel = "5-12";
            serAge.Points[1].LegendText = "5-12";
            serAge.Points[1].Label = fiveTo12 + " (" + (fiveTo12 / totalAge) * 100 + "%)";
            serAge.Points[2].AxisLabel = "13-19";
            serAge.Points[2].LegendText = "13-19";
            serAge.Points[2].Label = thirteenTo19 + " (" + (thirteenTo19 / totalAge) * 100 + "%)";
            serAge.Points[3].AxisLabel = "20-35";
            serAge.Points[3].LegendText = "20-35";
            serAge.Points[3].Label = twentyTo35 + " (" + (twentyTo35 / totalAge) * 100 + "%)";
            serAge.Points[4].AxisLabel = "36-70";
            serAge.Points[4].LegendText = "36-70";
            serAge.Points[4].Label = thirtysixTo70 + " (" + (thirtysixTo70 / totalAge) * 100 + "%)";
            serAge.Points[5].AxisLabel = "70-~";
            serAge.Points[5].LegendText = "70-~";
            serAge.Points[5].Label = above70 + " (" + (above70 / totalAge) * 100 + "%)";

            chAge.ChartAreas.Add(caAge);
            chAge.ChartAreas[0].BackColor = Color.Transparent;
            chAge.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chAge.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            chAge.Legends.Add(legAge);
            chAge.Series.Clear();
            chAge.Palette = ChartColorPalette.Excel;
            chAge.Titles.Add("Age Distribution");
            chAge.Series.Add(serAge);
            chAge.Invalidate();
            this.Controls.Add(chAge);

            Dictionary<DateTime, int> datesOfReg = new Dictionary<DateTime, int>();
            //we are interested in the past 30 days only
            for(int i = 0; i < 30; i++)
            {
                DateTime dt = DateTime.Now.Subtract(new TimeSpan(i, 0, 0, 0, 0));
                datesOfReg.Add(new DateTime(dt.Year, dt.Month, dt.Day), 0);
            }
            foreach(var person in persons)
            {
                DateTime dt = person.DateRegistered;
                if (datesOfReg.ContainsKey(dt))
                {
                    datesOfReg[dt] += 1;
                }
            }
            var xvals = datesOfReg.Keys;
            var yvals = datesOfReg.Values;

            // create the chart
            var chart = new Chart()
            {
                BackColor = Color.LightGreen,
                Name = "chart",
                Location = new Point(chAge.Left, chAge.Bottom + 100),
                Size = new Size(chAge.Width, chAge.Height),
            };
            chart.Titles.Add("Enrollments/Day");

            var chartArea = new ChartArea();
            chartArea.AxisX.LabelStyle.Format = "dd/MMM\nhh:mm";
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
            chart.ChartAreas.Add(chartArea);

            var series = new Series();
            series.Name = "Series1";
            series.ChartType = SeriesChartType.FastLine;
            series.XValueType = ChartValueType.Date;
            chart.Series.Add(series);

            // bind the datapoints
            chart.Series["Series1"].Points.DataBindXY(xvals, yvals);

            // draw!
            chart.Invalidate();
            this.Controls.Add(chart);
        }

        private double age(IdpPerson person)
        {
            if (person.YoB > 0)
            {
                return DateTime.Now.Year - person.YoB;
            }
            else
            {
                return DateTime.Now.Year - person.DoB.Year;
            }
        }
    }
}
