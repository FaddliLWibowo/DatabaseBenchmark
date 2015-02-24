﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DatabaseBenchmark.Charts
{
    public partial class LineChartFrame : UserControl
    {
        private ChartArea chartArea;
        private Series cache;

        public LineChartFrame()
        {
            InitializeComponent();

            chartArea = chart1.ChartAreas[0];

            // Chart area
            chartArea.BackColor = Color.Black;

            // Axes
            chartArea.AxisX.MajorGrid.LineColor = Color.DimGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.DimGray;

            chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
        }

        /// <summary>
        /// Creates a series in the chart with the specified name and color.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        public void CreateSeries(string name, Color color)
        {
            cache = chart1.Series.Add(name);

            cache.Color = color;
            cache.BorderWidth = 2;
            cache.ChartType = SeriesChartType.Line;
        }

        public void AddPoint(string series, long x, double y)
        {
            if (cache.Name != series)
                cache = chart1.Series.FindByName(series);

            cache.Points.AddXY(x, y);
        }

        /// <summary>
        /// Gets or sets a flag that indicates if the Y axis is logarithmic.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsLogarithmic
        {
            get { return chartArea.AxisY.IsLogarithmic; }
            set { chartArea.AxisY.IsLogarithmic = value; }
        }

        public string Title
        {
            set
            {
                chart1.Titles.Clear();

                string name = value;

                Title title = chart1.Titles.Add(name);
                title.Text = name;
                title.Font = new Font(title.Font.FontFamily, 16, FontStyle.Bold);
                title.Visible = true;
            }
        }

        /// <summary>
        /// Returns the number of points for the specified series.
        /// </summary>
        public int GetPointsCount(string series)
        {
            return chart1.Series[series].Points.Count;
        }

        /// <summary>
        /// Clears all created series.
        /// </summary>
        public void Clear()
        {
            chart1.Series.Clear();
            cache = null;
        }
    }
}