namespace SimulatorGUI
{
    partial class EnvironmentPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.EnvironmentChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.EnvironmentChart)).BeginInit();
            this.SuspendLayout();
            // 
            // EnvironmentChart
            // 
            this.EnvironmentChart.BackColor = System.Drawing.Color.DimGray;
            chartArea1.AxisX.Title = "Time";
            chartArea1.AxisY.Title = "Pressure (kPa)";
            chartArea1.Name = "TopLeft";
            chartArea2.AxisX.Title = "Time";
            chartArea2.AxisY.Title = "Temperature (C)";
            chartArea2.Name = "BottomLeft";
            chartArea3.AxisX.Title = "Time";
            chartArea3.AxisY.Title = "Enthalpy (kJ)";
            chartArea3.Name = "TopRight";
            chartArea4.AxisX.Title = "Time";
            chartArea4.AxisY.Title = "Gas Mass (kg)";
            chartArea4.Name = "BottomRight";
            this.EnvironmentChart.ChartAreas.Add(chartArea1);
            this.EnvironmentChart.ChartAreas.Add(chartArea2);
            this.EnvironmentChart.ChartAreas.Add(chartArea3);
            this.EnvironmentChart.ChartAreas.Add(chartArea4);
            this.EnvironmentChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "EnvironmentLegend";
            this.EnvironmentChart.Legends.Add(legend1);
            this.EnvironmentChart.Location = new System.Drawing.Point(0, 0);
            this.EnvironmentChart.Name = "EnvironmentChart";
            series1.ChartArea = "TopLeft";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Legend = "EnvironmentLegend";
            series1.Name = "Pressure";
            series2.ChartArea = "BottomLeft";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series2.Legend = "EnvironmentLegend";
            series2.Name = "Temperature";
            series3.ChartArea = "TopRight";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series3.Legend = "EnvironmentLegend";
            series3.Name = "Enthalpy";
            series4.ChartArea = "BottomRight";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series4.Legend = "EnvironmentLegend";
            series4.Name = "Gas Distribution";
            this.EnvironmentChart.Series.Add(series1);
            this.EnvironmentChart.Series.Add(series2);
            this.EnvironmentChart.Series.Add(series3);
            this.EnvironmentChart.Series.Add(series4);
            this.EnvironmentChart.Size = new System.Drawing.Size(816, 523);
            this.EnvironmentChart.TabIndex = 0;
            this.EnvironmentChart.Text = "chart1";
            // 
            // EnvironmentPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.EnvironmentChart);
            this.Name = "EnvironmentPanel";
            this.Size = new System.Drawing.Size(816, 523);
            ((System.ComponentModel.ISupportInitialize)(this.EnvironmentChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart EnvironmentChart;
    }
}
