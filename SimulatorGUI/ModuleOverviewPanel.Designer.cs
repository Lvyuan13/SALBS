namespace SimulatorGUI
{
    partial class ModuleOverviewPanel
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
            this.EnvironmentChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.EnvironmentChart)).BeginInit();
            this.SuspendLayout();
            // 
            // EnvironmentChart
            // 
            this.EnvironmentChart.BackColor = System.Drawing.Color.DimGray;
            chartArea1.AxisX.Title = "Tank Name";
            chartArea1.AxisY.Title = "Quantity (kg)";
            chartArea1.BackColor = System.Drawing.Color.White;
            chartArea1.Name = "TopLeft";
            chartArea2.AxisX.Title = "Module Name";
            chartArea2.AxisY.Title = "Load (kW)";
            chartArea2.Name = "BottomLeft";
            chartArea3.AxisX.Title = "Time";
            chartArea3.AxisY.Title = "Base Load (kW)";
            chartArea3.Name = "TopRight";
            chartArea4.AxisX.Title = "Time";
            chartArea4.AxisY.Title = "Gas Mass (kg)";
            chartArea4.BackColor = System.Drawing.Color.DimGray;
            chartArea4.BackSecondaryColor = System.Drawing.Color.Gray;
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
            series1.Legend = "EnvironmentLegend";
            series1.Name = "Tanks";
            series2.ChartArea = "BottomLeft";
            series2.Legend = "EnvironmentLegend";
            series2.Name = "Module Power Usage";
            series2.YValuesPerPoint = 2;
            this.EnvironmentChart.Series.Add(series1);
            this.EnvironmentChart.Series.Add(series2);
            this.EnvironmentChart.Size = new System.Drawing.Size(1098, 526);
            this.EnvironmentChart.TabIndex = 1;
            this.EnvironmentChart.Text = "chart1";
            // 
            // ModuleOverviewPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.EnvironmentChart);
            this.Name = "ModuleOverviewPanel";
            this.Size = new System.Drawing.Size(1098, 526);
            ((System.ComponentModel.ISupportInitialize)(this.EnvironmentChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart EnvironmentChart;
    }
}
