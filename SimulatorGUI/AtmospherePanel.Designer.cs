namespace SimulatorGUI
{
    partial class AtmospherePanel
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
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.AtmosphereChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.AtmosphereChart)).BeginInit();
            this.SuspendLayout();
            // 
            // AtmosphereChart
            // 
            this.AtmosphereChart.BackColor = System.Drawing.Color.DimGray;
            chartArea1.AxisX.Title = "Time";
            chartArea1.AxisY.Title = "Resource Consumption (kg)";
            chartArea1.Name = "Top";
            chartArea2.AxisX.Title = "Time";
            chartArea2.AxisY.Title = "Power Consumption (kJ)";
            chartArea2.Name = "Bottom";
            this.AtmosphereChart.ChartAreas.Add(chartArea1);
            this.AtmosphereChart.ChartAreas.Add(chartArea2);
            this.AtmosphereChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "EnvironmentLegend";
            this.AtmosphereChart.Legends.Add(legend1);
            this.AtmosphereChart.Location = new System.Drawing.Point(0, 0);
            this.AtmosphereChart.Name = "AtmosphereChart";
            series1.ChartArea = "Top";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Legend = "EnvironmentLegend";
            series1.Name = "ResourceUsage";
            series2.ChartArea = "Bottom";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series2.Legend = "EnvironmentLegend";
            series2.Name = "Energy Usage";
            this.AtmosphereChart.Series.Add(series1);
            this.AtmosphereChart.Series.Add(series2);
            this.AtmosphereChart.Size = new System.Drawing.Size(786, 597);
            this.AtmosphereChart.TabIndex = 1;
            this.AtmosphereChart.Text = "chart1";
            this.AtmosphereChart.Click += new System.EventHandler(this.EnvironmentChart_Click);
            // 
            // AtmospherePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.AtmosphereChart);
            this.Name = "AtmospherePanel";
            this.Size = new System.Drawing.Size(786, 597);
            ((System.ComponentModel.ISupportInitialize)(this.AtmosphereChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart AtmosphereChart;
    }
}
